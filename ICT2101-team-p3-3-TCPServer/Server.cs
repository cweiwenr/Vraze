using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

public class HandleClient
{
    public TcpClient Client { get; set; }
    public bool IsCar { get; set; }
    public int ThreadId { get; set; }

    public HandleClient(TcpClient client, int threadId, bool isCar = false)
    {
        this.Client = client;
        this.IsCar = isCar;
    }
}

class Server
{
    List<HandleClient> clientList = new List<HandleClient>(); //Stores all the TCP client connected to the server.
    TcpListener server = null;
    public Server(string ip, int port)
    {
        IPAddress localAddr = IPAddress.Parse(ip);
        server = new TcpListener(localAddr, port);
        server.Start();
        StartListener();
    }

    public void StartListener()
    {
        try
        {
            while (true)
            {
                //wait for esp8266 to do at+cipstart=
                // right hand side of equals, do "TCP", "your ip", 5000 <-since we are running this console app on port 5000
                // must have quotations for the ip and tcp...
                Console.WriteLine($"Waiting for a connection with {clientList.Count} connected clients...");
                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine("Connected!");

                //allow multuple connections, so that in the event this cant be integrated to mvc,
                //im assuming the asp net core mvc app can init a tcp connection to this console, so this
                // tcp service will be a backend for our project
                Thread t = new Thread(new ParameterizedThreadStart(HandleDeivce));
                t.Start(new HandleClient(client, t.ManagedThreadId));
            }
        }
        catch (SocketException e)
        {
            Console.WriteLine("SocketException: {0}", e);
            server.Stop();
        }
    }

    public void HandleDeivce(Object obj)
    {
        HandleClient status = (HandleClient)obj;
        TcpClient client = status.Client;
        status.ThreadId = Thread.CurrentThread.ManagedThreadId;
        // Add connected client to the clientList of the server
        clientList.Add(status);

        var stream = client.GetStream();
        string imei = String.Empty;

        string data = null;
        Byte[] bytes = new Byte[256];
        int i;
        try
        {
            var currentClient = clientList.Find(client => client.ThreadId == Thread.CurrentThread.ManagedThreadId);
            while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
            {
                string hex = BitConverter.ToString(bytes);
                data = Encoding.ASCII.GetString(bytes, 0, i);
                Console.WriteLine("Thread #{1} [{2}]: Received: {0}", data, Thread.CurrentThread.ManagedThreadId, (currentClient.IsCar) ? "Car" : "Web");

                foreach (var c in clientList)
                    {
                        Console.WriteLine($"Client #{c.ThreadId}\tCar: {c.IsCar}");
                    }
                if (data.Contains("client")) //data == "client")
                {
                    currentClient.IsCar = true;
                    var serverMsg = Encoding.ASCII.GetBytes("Hello");

                    Console.WriteLine($"Current Client Car Status: {currentClient.IsCar}");
                    stream.Write(serverMsg, 0, serverMsg.Length);
                }
                else if (data[0] == 'W') // Received from the Web Portal to be SENT to the Car
                {
                    // Get the TCP client of the car
                    var carClient = clientList.Find(client => client.IsCar == true);

                    // Using the Car's Tcp Client Network Stream to send the car commands from the Web Portal to the Car
                    using (var carStream = carClient.Client.GetStream())
                    {
                        // Create new byte array to store the car commands
                        byte[] carCommandBytes = new byte[bytes.Length];

                        // Copies the car commands without the prefix 'W' to the new byte array
                        Buffer.BlockCopy(bytes, 1, carCommandBytes, 0, carCommandBytes.Length-1);

                        // Send the data to the Car
                        carStream.Write(carCommandBytes, 0, carCommandBytes.Length);
                    }
                }
                else if (data[0] == 'C') // Received from the Car to be SENT to the Web Portal
                {
                    try
                    {
                        using (HttpClient httpClient = new HttpClient())
                        {
                            // Read the data sent from the Car and trim off the prefix 'C' and we will get the query parameter for the car statistic
                            string carStatistic = data.Substring(1, i - 1);

                            // Send the Car Statistics over Http GET
                            string updateCarStatApi = $"http://localhost:46381/Car/Update?{carStatistic}";
                            var responseString = httpClient.GetAsync(updateCarStatApi).ConfigureAwait(false);
                        }
                    }
                    catch (Exception ex)
                    { }
                }

                if (!client.Connected)
                    break;
                /*
                string str = "Hey Device!";
                Byte[] reply = System.Text.Encoding.ASCII.GetBytes(str);   
                stream.Write(reply, 0, reply.Length);
                Console.WriteLine("{1}: Sent: {0}", str, Thread.CurrentThread.ManagedThreadId);*/

            }
            Console.WriteLine("Disconnected");
            // while(true)
            // {
            //     string str;
            //     Console.Write("Enter some freaking shit: ");
            //     str = Console.ReadLine();
            //     Byte[] reply = System.Text.Encoding.ASCII.GetBytes(str);   
            //     stream.Write(reply, 0, reply.Length);
            //     Console.WriteLine("{1}: Sent: {0}", str, Thread.CurrentThread.ManagedThreadId);
            // }

        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: {0}", e.ToString());
            client.Close();
        }
    }
}