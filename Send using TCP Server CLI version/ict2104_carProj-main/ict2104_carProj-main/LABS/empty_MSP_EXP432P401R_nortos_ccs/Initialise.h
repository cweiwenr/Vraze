/*******************************************************************************
 * MSP432 Initialise
 *
 *
 *                MSP432P401
 *             ------------------
 *         /|\|            P1.0  |---> LED P1 RED
 *          | |            P2.0  |---> LED P2 RED
 *          --|RST         P2.1  |---> LED P2 GREEN
 *            |            P2.2  |---> LED P2 BLUE
 *            |            P5.4  |---> 3v3 for left Encoder's VCC
 *            |            P5.7  |---> 3v3 for right Encoder's VCC
 *            |            P4.5  |---> Output for Left wheel
 *            |            P4.4  |---> Output for Left wheel
 *            |            P4.0  |---> Output for Right wheel
 *            |            P4.2  |---> Output for Right wheel
 *            |            P2.4  |---> PWM output for Left wheel
 *            |            P2.5  |---> PWm output for Right wheel
 *            |            P3.3  |<--- UART Receiver
 *            |            P3.2  |---> UART Transceiver
 *            |            P2.6  |<--- Left wheel encoder interrupt input
 *            |            P2.7  |<--- Right wheel encoder interrupt input
 *            |                  |
 *
 ******************************************************************************/
#include <ti/devices/msp432p4xx/driverlib/driverlib.h>
#include <ESP8266.h>

/* Function prototypes*/
void Initialise_Encoder(void);
void Initialise_EspUART(void);
void Initialise_CarMotor(void);
void Initialise_IO(void);
void Initialise_TimerA1(void);
void enableInterrupts(void);
