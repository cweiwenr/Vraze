#include <main.h>
#define MIN_DISTANCE 15.0f
#define TICKPERIOD      1000
uint32_t SR04IntTimes;
/******************************************************************************
 * PWM and Timer Configurations
 *
 * pwmConfig          - PWM Configuration for the right wheel
 * pwmConfig2         - PWM Configuration for the left wheel
 * upConfig           - Timer Configuration for Ultrasonic sensor
 *******************************************************************************/
Timer_A_PWMConfig pwmConfig =
{
    TIMER_A_CLOCKSOURCE_SMCLK,              // SMCLK Clock Source 24MHz
    TIMER_A_CLOCKSOURCE_DIVIDER_1,          // SMCLK/1 = 3MHz
    10000,                                  // Period = 10000
    TIMER_A_CAPTURECOMPARE_REGISTER_1,
    TIMER_A_OUTPUTMODE_RESET_SET,
    0                                       // Duty Cycle = 0%
};

Timer_A_PWMConfig pwmConfig2 =
{
    TIMER_A_CLOCKSOURCE_SMCLK,              // Same as pwmConfig
    TIMER_A_CLOCKSOURCE_DIVIDER_1,
    10000,
    TIMER_A_CAPTURECOMPARE_REGISTER_2,
    TIMER_A_OUTPUTMODE_RESET_SET,
    0
};

// -------------------------------------------------------------------------------------------------------------------

void main()
{
    /* Stop Watchdog Timer */
    MAP_WDT_A_holdTimer();

    /* Master CLK frequency*/
    CS_setDCOCenteredFrequency(CS_DCO_FREQUENCY_24);

    engineState = CAR_ENGINE_OFF;
    Initialise_IO();
    Initialise_CarMotor();
    Initialise_Encoder();
    //Initalise_HCSR04();
    Initialise_TimerA1();
    Initialise_EspUART();
    enableInterrupts();

    /* Ready Green LED*/
    GPIO_toggleOutputOnPin(GPIO_PORT_P2, GPIO_PIN0);

    /* Program's loop */
    while (1)
    {

        /* Fill up ESP8266 buffer before processing data*/
        while (ESP8266_WaitForAnswer(200))
        {
            /* Set wifi state if there are instructions*/
            wifiState = CAR_RECEIVED_INSTRUCTION;
        }
        if (wifiState == CAR_RECEIVED_INSTRUCTION)
        {
            /* Process instructions*/
            /* Start engine first*/
            engineState = CAR_ENGINE_ON;

            /* Variables for processing instructions*/
            uint16_t i = 0;
            char temp[2048];
            char c;

            /* Get instructions from ESP8266 Buffer and process it*/
            instructionBuffer = ESP8266_GetBuffer();
            instructionBuffer = strtok(instructionBuffer, ":");
            instructionBuffer = strtok(NULL, ":");

            /* String copy buffer so that we can iterate each character*/
            strcpy(temp, instructionBuffer);
            uint16_t str_len = strlen(temp);

            /* Iterate through each character and process it*/
            for (i; temp[i]; i++)
            {
                __delay_cycles(30000);
                c = temp[i];
                if (c == 'F')
                    moveCar(c);
                else if (c == 'B')
                    moveCar(c);
                else if (c == 'R')
                    moveCar(c);
                else if (c == 'L')
                    moveCar(c);
            }
        }
        engineState = CAR_ENGINE_OFF;
        wifiState = CAR_WAITING_INSTRUCTION;
        PCM_gotoLPM0();
    }
}

// -------------------------------------------------------------------------------------------------------------------

void moveCar(char dir)
{
    if(dir == 'F')
    {
        desiredNotches = 40;
        setWheelDirection(CAR_WHEEL_FORWARD);
    }
    else if (dir == 'B')
    {
        desiredNotches = 40;
        setWheelDirection(CAR_WHEEL_BACKWARD);
    }
    else if (dir == 'R')
    {
        desiredNotches = 10;
        //setWheelDirection(CAR_WHEEL_RIGHT);

        GPIO_setOutputLowOnPin(GPIO_PORT_P4, GPIO_PIN0);
        GPIO_setOutputHighOnPin(GPIO_PORT_P4, GPIO_PIN2);
        GPIO_setOutputLowOnPin(GPIO_PORT_P4, GPIO_PIN4);
        GPIO_setOutputHighOnPin(GPIO_PORT_P4, GPIO_PIN5);
    }
    else if (dir == 'L')
    {
        desiredNotches = 10;
        //setWheelDirection(CAR_WHEEL_LEFT);

        GPIO_setOutputLowOnPin(GPIO_PORT_P4, GPIO_PIN2);
        GPIO_setOutputHighOnPin(GPIO_PORT_P4, GPIO_PIN0);
        GPIO_setOutputHighOnPin(GPIO_PORT_P4, GPIO_PIN4);
        GPIO_setOutputLowOnPin(GPIO_PORT_P4, GPIO_PIN5);
    }

    /* Off Green LED when car is moving or not ready*/
    GPIO_toggleOutputOnPin(GPIO_PORT_P2, GPIO_PIN1);
    if (engineState == CAR_ENGINE_ON)
    {
        /* Start timer for RPM/ speed calculations, then call PID contoller to generate PWM*/
        Timer_A_startCounter(TIMER_A1_BASE,TIMER_A_UP_MODE);
/*
        while(rightNotchesDetected < desiredNotches && leftNotchesDetected < desiredNotches)
        {
            getPIDOutput(desiredNotches, dir);
        }
*/
        getPIDOutput(desiredNotches, dir);
        /* Make sure wheel stops moving after reaching desired*/
        //pwmConfig.dutyCycle = 0;
        //pwmConfig2.dutyCycle = 0;
        //Timer_A_generatePWM(TIMER_A0_BASE, &pwmConfig);
        //Timer_A_generatePWM(TIMER_A0_BASE, &pwmConfig2);
        //Timer_A_stopTimer(TIMER_A1_BASE);
        //clearCounters();
    }
    GPIO_toggleOutputOnPin(GPIO_PORT_P2, GPIO_PIN1);
}

// -------------------------------------------------------------------------------------------------------------------

void getPIDOutput(uint32_t targetNotch, char dir)
{
    float kp = 0.28;
    float ki = 0.09;
    float kd = 0.15;

    float currentL = 0;
    float currentR = 0;

    float minDiff = 0;

    do
    {
        __delay_cycles(5000);
        float errorL = targetNotch - leftNotchesDetected;
        float errorR = targetNotch - rightNotchesDetected;

        if (errorL < minDiff && errorL != 0)
        {
            errorTl += errorL;
        }
        else
        {
            errorTl = 0;
        }
        if (errorR < minDiff && errorR != 0)
        {
            errorTr += errorR;
        }
        else
        {
            errorTr = 0;
        }
        if (errorTl > 50/ki)
        {
            errorTl = 50/ki;
        }
        if (errorTr > 50/ki)
        {
            errorTr = 50/ki;
        }
        if (errorL == 0)
        {
            derivativeL = 0;
        }
        if (errorR == 0)
        {
            derivativeR = 0;
        }

        /* PID CALCULATIONS*/
        proportionL = errorL * kp;
        proportionR = errorR * kp;

        integralL = errorTl * ki;
        integralR = errorTr * ki;

        derivativeL = (errorL - lastErrorL) * kd;
        derivativeR = (errorR - lastErrorR) * kd;

        /* Update last errors*/
        lastErrorL = errorL;
        lastErrorR = errorR;

        /* Value to be sent to the motors*/
        currentR = (proportionR + integralR + derivativeR) * 5000;
        currentL = (proportionL + integralL + derivativeL) * 5000;

        /* Adjust PWM*/
        pwmConfig.dutyCycle = currentR;
        pwmConfig2.dutyCycle = currentL;
        Timer_A_generatePWM(TIMER_A0_BASE, &pwmConfig);
        Timer_A_generatePWM(TIMER_A0_BASE, &pwmConfig2);
    } while (leftNotchesDetected < targetNotch || rightNotchesDetected < targetNotch);

    __delay_cycles(5000);
    pwmConfig.dutyCycle = 0;
    pwmConfig2.dutyCycle = 0;
    Timer_A_generatePWM(TIMER_A0_BASE, &pwmConfig2);
    Timer_A_generatePWM(TIMER_A0_BASE, &pwmConfig);
    Timer_A_stopTimer(TIMER_A1_BASE);
    clearCounters();
}

// -------------------------------------------------------------------------------------------------------------------

void clearCounters(void)
{
    Timer_A_clearTimer(TIMER_A1_BASE);
    rightNotchesDetected = 0;
    timeSeconds = 0;
    leftNotchesDetected = 0;
    leftSpeed = 0;
    rightSpeed = 0;
    leftRPM = 0;
    rightRPM = 0;
    leftError = 0;
    rightError = 0;
    errorTl = 0;
    errorTr = 0;
    lastErrorL = 0;
    lastErrorR = 0;
    proportionL = 0;
    proportionR = 0;
    integralL = 0;
    integralR = 0;
    derivativeL = 0;
    derivativeR = 0;
}

// -------------------------------------------------------------------------------------------------------------------

/* Interrupt Handlers */
void PORT1_IRQHandler(void)
{
    uint32_t status_for_switch1;

    status_for_switch1 = MAP_GPIO_getInterruptStatus(GPIO_PORT_P1, GPIO_PIN1); //get status of switch 1's interrupt flag

    if (status_for_switch1 & GPIO_PIN1) //Switch 1 (P1.1) On/Off Car Engine
    {
        // Placeholder interrupt to handle and send data, can be a function for when esp wants to send data, but
        // if msp is in low pwr mode then need interrupt first, probs need to call the func in the interrupt
        // or just apply send logic to that interrupt
        char dataSend[] = "debo\r\n\r\n";
        uint32_t t = sizeof(dataSend) - 1;
        ESP8266_SendData(dataSend, t);

        MAP_GPIO_toggleOutputOnPin(GPIO_PORT_P1, GPIO_PIN0);

        MAP_GPIO_clearInterruptFlag(GPIO_PORT_P1, status_for_switch1);

    }
}

// -------------------------------------------------------------------------------------------------------------------

void PORT2_IRQHandler(void)
{
    uint32_t status;
    status = GPIO_getEnabledInterruptStatus(GPIO_PORT_P2);

    if (status & GPIO_PIN7)
    {
        rightNotchesDetected += 1;
        GPIO_clearInterruptFlag(GPIO_PORT_P2, GPIO_PIN7);
    }
    if (status & GPIO_PIN6)
    {
        leftNotchesDetected += 1;
        GPIO_clearInterruptFlag(GPIO_PORT_P2, GPIO_PIN6);
    }
}

// -------------------------------------------------------------------------------------------------------------------

void TA1_0_IRQHandler(void)
{
    /* Increment global variable (count number of interrupt occurred)*/
    timeSeconds += 1;

    /* RPM*/
    leftRPM = ((leftNotchesDetected/timeSeconds)/20.0)*60;
    rightRPM = ((rightNotchesDetected/timeSeconds)/20.0)*60;

    /* Clear interrupt flag*/
    Timer_A_clearCaptureCompareInterrupt(TIMER_A1_BASE, TIMER_A_CAPTURECOMPARE_REGISTER_0);
}
