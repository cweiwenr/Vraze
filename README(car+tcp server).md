# File Structure in repository
+ There are 3 folders in this repository
+ First is the 2101WebPortal, please refer to README(webportal).md for installation instructions
+ Second is project version 1, sending commands using the web portal
+ Third is project version 2, sending commands using TCP CLI

# Project version 1
+ This folder contains a multithreaded TCP server and the CCS workspace folder for the car
+ CCS workspace in \LABS
+ do find the CCS code files from LABS\empty_MSP_EXP432P401R_nortos_ccs
+ This version is requires the instructions to be sent from web to TCP server before car's connection
+ This is becasuse of our inexperiences with development of a multithreaded TCP server and not the scope of ICT2104 and ICT2101
+ To run, run the web application using visual studio, then start the TCP server, and send the commands to the queue in TCP server. Subsequently on the car and wait for connection, ensure that wifi ips and ports has been changed accordingly.

# Project version 2
+ This folder contains a TCP server that can constantly communicate to car over wifi
+ CCS workspace in, ict2104_carProj-main\ict2104_carProj-main\LABS
+ do find the CCS code files from ict2104_carProj-main\ict2104_carProj-main\LABS\empty_MSP_EXP432P401R_nortos_ccs
+ This version works as per normal, start the TCP server, on the car and wait for connection. Once connected, car will be able to receive typed in commands from TCP server over wifi and execute accordingly.

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

