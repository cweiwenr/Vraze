# File Structure in repository
+ LABS contain the code composer workspace where the code of the car resides in
    - Code file paths, LABS\empty_MSP_EXP432P401R_nortos_ccs
+ ICT2101-team-p3-3-TCPServer folder houses the multithreaded TCP server
    - To run, ensure that you have the dotnet framework installed
    - Run using 'dotnet watch run'

# Project architecture
![architecture](https://res.cloudinary.com/dc6eqgbc0/image/upload/v1638492773/Architecture_Diagram_wsdfcc.png)

# Features used
+ GPIO
+ Timer
+ UART
+ PWM
+ Interrupts
+ PID Controller

# Advance Feature
+ Multithreaded TCP Server

# Note
The architecture shows HTTP communication from TCP server to web portal, this is because we are not as experienced in developing servers. Furthermore, a multithreaded server. Thus, since there are only 2 communications the TCP server has to handle, we use the TCP channel to send any data comming to the server fro mthe portal to the car and use HTTP to send any data coming from car through TCP.
