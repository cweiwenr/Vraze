#include <ti/devices/msp432p4xx/driverlib/driverlib.h>
#include <ESP8266.h>
#include <UART_Driver.h>

//this instructions buffer holds all instructions yay
char * Instructions;
uint8_t statess = 1;
/*
eUSCI_UART_ConfigV1 UART0Config =
{
     EUSCI_A_UART_CLOCKSOURCE_SMCLK,
     13,
     0,
     37,
     EUSCI_A_UART_NO_PARITY,
     EUSCI_A_UART_LSB_FIRST,
     EUSCI_A_UART_ONE_STOP_BIT,
     EUSCI_A_UART_MODE,
     EUSCI_A_UART_OVERSAMPLING_BAUDRATE_GENERATION
};*/

eUSCI_UART_ConfigV1 UART2Config =
{
     EUSCI_A_UART_CLOCKSOURCE_SMCLK,
     13,
     0,
     37,
     EUSCI_A_UART_NO_PARITY,
     EUSCI_A_UART_LSB_FIRST,
     EUSCI_A_UART_ONE_STOP_BIT,
     EUSCI_A_UART_MODE,
     EUSCI_A_UART_OVERSAMPLING_BAUDRATE_GENERATION
};
/*
void ESP8266_Terminal(void)
{
    while(1)
    {
        //* variable to store channel
        uint8_t ESP_channel = 5;    //get response channel that esp is using
        char * separator = "=";

        //* call function to populate ESP buffer with data
        // getData();
        UART_Gets(EUSCI_A2_BASE, ESP8266_Buffer, 2048);
        UART_Printf(EUSCI_A0_BASE, ESP8266_Buffer);
        __delay_cycles(48000000);
        if(strstr(ESP8266_Buffer, "+IPD,0") != NULL)
            ESP_channel = 0;
        else if(strstr(ESP8266_Buffer, "+IPD,1") != NULL)
            ESP_channel = 1;
        else if(strstr(ESP8266_Buffer, "+IPD,2") != NULL)
            ESP_channel = 2;
        else if(strstr(ESP8266_Buffer, "+IPD,3") != NULL)
            ESP_channel = 3;
        else if(strstr(ESP8266_Buffer, "+IPD,4") != NULL)
            ESP_channel = 4;


        if(strstr(ESP8266_Buffer, "instructions=") != NULL)
        {
            Instructions = strtok(ESP8266_Buffer, separator);
            Instructions = strtok(NULL, ESP8266_Buffer);
            UART_Printf(EUSCI_A0_BASE, Instructions);
        }
        if (ESP_channel != 5)
        {
            //*Data that will be sent to the HTTP server
            char HTTP_Request[] = "POST /test HTTP/1.1\r\nHost: 192.168.157.22:5000\r\nContent-Type: application/json\r\nContent-length: 11\r\n\r\n{\"test\": 3}";
            uint32_t HTTP_Request_Size = sizeof(HTTP_Request) - 1;
            ESP8266_SendData(ESP_channel, HTTP_Request, HTTP_Request_Size);
            __delay_cycles(48000000);
            UART_Printf(EUSCI_A2_BASE, "AT+CIPCLOSE=%i\r\n", ESP_channel);
            ESP_channel = 5;
        }
    }
}
*/

void main()
{
    /*Initialize required hardware peripherals for the ESP8266*/
    /*
    MAP_GPIO_setAsPeripheralModuleFunctionInputPin(GPIO_PORT_P1, GPIO_PIN2 | GPIO_PIN3, GPIO_PRIMARY_MODULE_FUNCTION);
    MAP_UART_initModule(EUSCI_A0_BASE, &UART0Config);
    MAP_UART_enableModule(EUSCI_A0_BASE);
    MAP_UART_enableInterrupt(EUSCI_A0_BASE, EUSCI_A_UART_RECEIVE_INTERRUPT);
    MAP_Interrupt_enableInterrupt(INT_EUSCIA0);
    */
    MAP_WDT_A_holdTimer();

    /*Ensure MSP432 is Running at 24 MHz*/
    FlashCtl_setWaitState(FLASH_BANK0, 2);
    FlashCtl_setWaitState(FLASH_BANK1, 2);
    PCM_setCoreVoltageLevel(PCM_VCORE1);
    CS_setDCOCenteredFrequency(CS_DCO_FREQUENCY_24);

    MAP_GPIO_setAsPeripheralModuleFunctionInputPin(GPIO_PORT_P3, GPIO_PIN2 | GPIO_PIN3, GPIO_PRIMARY_MODULE_FUNCTION);
    MAP_UART_initModule(EUSCI_A2_BASE, &UART2Config);
    MAP_UART_enableModule(EUSCI_A2_BASE);
    MAP_UART_enableInterrupt(EUSCI_A2_BASE, EUSCI_A_UART_RECEIVE_INTERRUPT);
    MAP_Interrupt_enableInterrupt(INT_EUSCIA2);

    /* switch 1.1 api post to thinkspeak test*/
    MAP_GPIO_setAsInputPinWithPullUpResistor(GPIO_PORT_P1, GPIO_PIN1);
    MAP_GPIO_interruptEdgeSelect(GPIO_PORT_P1, GPIO_PIN1, GPIO_LOW_TO_HIGH_TRANSITION);
    MAP_GPIO_clearInterruptFlag(GPIO_PORT_P1, GPIO_PIN1);
    MAP_GPIO_enableInterrupt(GPIO_PORT_P1, GPIO_PIN1);
    MAP_Interrupt_enableInterrupt(INT_PORT1);
    MAP_GPIO_setAsOutputPin(GPIO_PORT_P1, GPIO_PIN0);
    MAP_GPIO_setOutputLowOnPin(GPIO_PORT_P1, GPIO_PIN0);

    /*Reset GPIO of the ESP8266*/
    GPIO_setAsOutputPin(GPIO_PORT_P6, GPIO_PIN1);

    MAP_Interrupt_enableMaster();

    /* Reset ESP8266 to prevent timeout errors*/
    ESP8266_HardReset();
    __delay_cycles(48000000);
    UART_Flush(EUSCI_A2_BASE);

    /* Configure to start ESP8266 webserver*/
    ESP8266_ChangeMode1();
    //ESP8266_EnableMultipleConnections(1);
    //ESP8266_DisconnectToAP();
    //ESP8266_ConnectToAP("Reuben","lol12345678");
    //ESP8266_ConnectToAP("CHONG_Fam","ibeeciejie");
    //__delay_cycles(48000000);
    //ESP8266_EstablishConnection('0', TCP, "192.168.157.22", "5000");
    ESP8266_EstablishConnection('0', TCP, "172.20.10.2", "5000");
    //ESP8266_startserver();

    /*Start ESP8266 serial terminal, will not return*/
    //ESP8266_Terminal();
    /* Have to do this instruction first */
    printf("Ready\n");
    while(1)
    {
        while(ESP8266_WaitForAnswer(200))
        {
            // state 2 for instructions receive, please handle
            statess = 2;
        }
        if(statess == 2)
        {
            uint16_t i = 0;
            char c;
            char temp[2048];
            //handle instructions here
            Instructions = ESP8266_GetBuffer();
            Instructions = strtok(Instructions, ":");
            Instructions = strtok(NULL, ":");
            //we have to str cpy cannot just use sizeof(char ptr) cause its not accurate
            strcpy(temp, Instructions);
            uint16_t str_len = strlen(temp);
            // temporary loop to loop through string to read instructions
            for (i ; temp[i]; i++)
            {
                //remember to change the state of the the car aft reading each char, according to the char
                c = temp[i];
                // if (c == "W")
                    //printf("call while loop function");
                // when comparing the char, make sure its single quotation if not !=

                if (c == 'F')
                    printf("car is moving forward\n");
                else if (c == 'B')
                    printf("car is moving backward\n");
                else if (c == 'R')
                    printf("car is moving right\n");
                else if (c == 'L')
                    printf("car is moving left\n");
                else
                    printf("random/n");
            }
            //can start to read instructions buffer and do soft interrupts OR make function calls
            printf("%s\n", Instructions);
        }
        statess = 1;
        // Go back to low power mode after handling all interrupts
        PCM_gotoLPM0();
    }

}
/*
void ESP8266_Terminal(void)
{
    while(1)
    {
        UART_Gets(EUSCI_A0_BASE, ESP8266_Buffer, 128);
        UART_Printf(EUSCI_A2_BASE, ESP8266_Buffer);

        __delay_cycles(48000000);
        if(!ESP8266_WaitForAnswer(ESP8266_RECEIVE_TRIES))
        {
            UART_Printf(EUSCI_A0_BASE, "ESP8266 receive timeout error\r\n");
        }
        else
        {
            UART_Printf(EUSCI_A0_BASE, ESP8266_Buffer);
        }

    }
}*/

void PORT1_IRQHandler(void)
{
    // Placeholder interrupt to handle and send data, can be a function for when esp wants to send data, but
    // if msp is in low pwr mode then need interrupt first, probs need to call the func in the interrupt
    // or just apply send logic to that interrupt
    uint32_t status_for_switch1;

    status_for_switch1 = MAP_GPIO_getInterruptStatus(GPIO_PORT_P1, GPIO_PIN1);  //get status of switch 1's interrupt flag

    if(status_for_switch1 & GPIO_PIN1)
    {

        char dataSend[] = "debo\r\n\r\n";
        uint32_t t = sizeof(dataSend) - 1;
        ESP8266_SendData(dataSend, t);

        MAP_GPIO_toggleOutputOnPin(GPIO_PORT_P1, GPIO_PIN0);
    }

    MAP_GPIO_clearInterruptFlag(GPIO_PORT_P1, GPIO_PIN1);

}
