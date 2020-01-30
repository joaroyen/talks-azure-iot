#include "main.h"
#include "Logger.h"

void Logger::setup() {
    Serial.begin(115200);
    println("Sketch:", __FILE__);
    println("Uploaded:", __DATE__);
    blankln();
}

void Logger::println(String value) {
    Serial.println(value);
}

void Logger::println(String label, String value)
{
    Serial.print(label);
    for (int i = 0; i < 16 - label.length(); i++) {
        Serial.print(" ");
    }
    Serial.println(value);
}

void Logger::println(String label, int value) {
    println(label, String(value));
}


void Logger::blankln() {
    Serial.println("");
}