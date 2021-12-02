/*******************************************************************************
 * MSP432 Timer Interrupt
 *
 *
 *                MSP432P401
 *             ------------------
 *         /|\|                  |
 *          | |                  |
 *          --|RST         P3.6  |---> Trigger
 *            |                  |
 *            |            P3.7  |<--- Echo
 *            |                  |
 *            |                  |
 *
 ******************************************************************************/
#include <ti/devices/msp432p4xx/driverlib/driverlib.h>

#define MIN_DISTANCE    20.0f
#define TICKPERIOD      1000

uint32_t SR04IntTimes;
void Initalise_HCSR04(void);
void TA0_0_IRQHandler(void);
static uint32_t getHCSR04Time(void);
float getHCSR04Distance(void);
