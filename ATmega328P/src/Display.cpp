#include "main.h"
#include "Display.h"
#include <LiquidCrystal.h>

LiquidCrystal lcd(
    PIN_LCD_RS, 
    PIN_LCD_ENABLE, 
    PIN_LCD_D4, 
    PIN_LCD_D5, 
    PIN_LCD_D6, 
    PIN_LCD_D7);

void Display::setup() {
    pinMode(PIN_LCD_RS, OUTPUT);
    pinMode(PIN_LCD_ENABLE, OUTPUT);
    pinMode(PIN_LCD_D4, OUTPUT);
    pinMode(PIN_LCD_D5, OUTPUT);
    pinMode(PIN_LCD_D6, OUTPUT);
    pinMode(PIN_LCD_D7, OUTPUT);

    lcd.begin(LCD_NUM_COLUMNS, LCD_NUM_ROWS);
    lcd.noCursor();
}

void Display::count(int counter, String device) {
    lcd.clear();
    lcd.setCursor(0, 0);
    lcd.print(counter);
    lcd.setCursor(0, 1);
    lcd.print(device);
}

void Display::set(int counter) {
    lcd.clear();
    lcd.print(counter);
}