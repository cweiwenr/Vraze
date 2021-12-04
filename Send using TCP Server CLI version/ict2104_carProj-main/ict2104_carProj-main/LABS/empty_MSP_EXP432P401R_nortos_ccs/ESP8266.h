#ifndef DEVICES_ESP8266_H_
#define DEVICES_ESP8266_H_

#include <string.h>
#include <stdlib.h>
#include <stdbool.h>
#include <UART_Driver.h>

#include <ti/devices/msp432p4xx/driverlib/driverlib.h>

#define ESP8266_BUFFER_SIZE     2048
#define ESP8266_RECEIVE_TRIES   10*20

#define TCP                     0
#define UDP                     1

bool ESP8266_WaitForAnswer(uint32_t Tries);
bool ESP8266_CheckConnection(void);
bool ESP8266_AvailableAPs(void);
bool ESP8266_ConnectToAP(char *SSID, char *Password);
bool ESP8266_EstablishConnection(char ID, uint8_t type, char *address, char *port);
bool ESP8266_EnableMultipleConnections(bool Enable);
bool ESP8266_SendData(char *Data, uint32_t DataSize);
bool ESP8266_startserver(void);
bool ESP8266_DisconnectToAP(void);

void ESP8266_SendATCommand(char *ATcommand);
char *ESP8266_GetBuffer(void);
void ESP8266_HardReset(void);

bool ESP8266_ChangeMode1(void);

#endif /* DEVICES_ESP8266_H_ */
