#include <ti/devices/msp432p4xx/driverlib/driverlib.h>
#include <stdint.h>

 /* Define Macros for Wheel Direction*/
#define CAR_WHEEL_STOP              0x0
#define CAR_WHEEL_FORWARD           0x1
#define CAR_WHEEL_BACKWARD          0x2
#define CAR_WHEEL_LEFT              0x5
#define CAR_WHEEL_RIGHT             0x9

/* Function Prototypes*/
void setWheelDirection(uint32_t dir);
