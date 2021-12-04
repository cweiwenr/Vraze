#include <CAR_Controls.h>

uint32_t wheelDirection = CAR_WHEEL_STOP;

/* Set wheel direction function*/
void setWheelDirection(uint32_t dir)
{
    wheelDirection = dir;

    if (dir & CAR_WHEEL_FORWARD)
    {
        /* Set direction Forward*/
        __delay_cycles(3000);

        /* Left Wheel Configuration*/
        GPIO_setOutputLowOnPin(GPIO_PORT_P4, GPIO_PIN2);
        GPIO_setOutputHighOnPin(GPIO_PORT_P4, GPIO_PIN0);

        /* Right Wheel Configuration*/
        GPIO_setOutputLowOnPin(GPIO_PORT_P4, GPIO_PIN4);
        GPIO_setOutputHighOnPin(GPIO_PORT_P4, GPIO_PIN5);
    }
    else if (dir & CAR_WHEEL_BACKWARD)
    {
        /* Set direction Backward*/

        __delay_cycles(3000);
        /* Left Wheel Configuration*/
        GPIO_setOutputLowOnPin(GPIO_PORT_P4, GPIO_PIN0);
        GPIO_setOutputHighOnPin(GPIO_PORT_P4, GPIO_PIN2);

        /* Right Wheel Configuration*/
        GPIO_setOutputLowOnPin(GPIO_PORT_P4, GPIO_PIN5);
        GPIO_setOutputHighOnPin(GPIO_PORT_P4, GPIO_PIN4);
    }
    else if (dir & CAR_WHEEL_LEFT)
    {
        /* Set direction Left*/
        __delay_cycles(3000);
        /* Left Wheel Configuration*/
        GPIO_setOutputLowOnPin(GPIO_PORT_P4, GPIO_PIN2);
        GPIO_setOutputHighOnPin(GPIO_PORT_P4, GPIO_PIN0);

        /* Right Wheel Configuration*/
        GPIO_setOutputLowOnPin(GPIO_PORT_P4, GPIO_PIN5);
        GPIO_setOutputHighOnPin(GPIO_PORT_P4, GPIO_PIN4);
    }
    else if (dir & CAR_WHEEL_RIGHT)
    {
        /* Set direction Right*/
        __delay_cycles(3000);

        /* Left Wheel Configuration*/
        GPIO_setOutputLowOnPin(GPIO_PORT_P4, GPIO_PIN0);
        GPIO_setOutputHighOnPin(GPIO_PORT_P4, GPIO_PIN2);

        /* Right Wheel Configuration*/
        GPIO_setOutputLowOnPin(GPIO_PORT_P4, GPIO_PIN4);
        GPIO_setOutputHighOnPin(GPIO_PORT_P4, GPIO_PIN5);
    }

}
