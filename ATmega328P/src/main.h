#ifndef __Main_h
#define __Main_h

#include <arduino.h>
#include "Logger.h"

enum {
    PIN_BUTTON_DOWN = 2,
    PIN_BUTTON_UP = 7,

    PIN_LCD_RS = A5,
    PIN_LCD_ENABLE = A4,
    PIN_LCD_D4 = A3,
    PIN_LCD_D5 = A2,
    PIN_LCD_D6 = A1,
    PIN_LCD_D7 = A0,

    PIN_BTSERIAL_RX = 8, // Hardcoded in serial library
    PIN_BTSERIAL_TX = 9 // Hardcoded in serial library
};

enum {
    COMMAND_DOWN = -1,
    COMMAND_NONE = 0,
    COMMAND_UP = 1
};

enum {
    LCD_NUM_COLUMNS = 16,
    LCD_NUM_ROWS = 2
};

#endif