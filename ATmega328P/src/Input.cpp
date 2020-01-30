#include "main.h"
#include "Input.h"

void Input::setup() {
    pinMode(PIN_BUTTON_UP, INPUT_PULLUP);
    pinMode(PIN_BUTTON_DOWN, INPUT_PULLUP);
}

int Input::read() {
    if (digitalRead(PIN_BUTTON_UP) == 0) {
        return COMMAND_UP;
    }

    if (digitalRead(PIN_BUTTON_DOWN) == 0) {
        return COMMAND_DOWN;
    }

    return COMMAND_NONE;
}