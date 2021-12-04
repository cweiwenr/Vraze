using System;
using System.Threading;
class Program
{
    static void Main(string[] args)
    {
        Thread t = new Thread(delegate ()
        {
            // change ip to ur com's private ip
            Server myserver = new Server("172.20.10.2", 8080);
            //Server myserver = new Server("192.168.86.81", 8080);
        
        });
        t.Start();
        
        Console.WriteLine("Server Started...!");
    }
}