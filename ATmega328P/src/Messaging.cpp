#include "main.h"
#include "Messaging.h"

#include <AltSoftSerial.h>
AltSoftSerial btSerial; 


Messaging::Messaging(Logger* logger) {
    _logger = logger;
}

void Messaging::setup() {
    btSerial.begin(9600);
    _logger->println("BTserial started at 9600");
}

int Messaging::receive(String &device, int &counter) {
    if (btSerial.available() > 0) {
        device = btSerial.readStringUntil(';');
        if (device.length() == 0) return COMMAND_NONE;

        _logger->println("MsgDevice:", device);

        int command = btSerial.parseInt();
        _logger->println("MsgCommand:", command);
        btSerial.read(); // ';'
        counter = btSerial.parseInt();
        _logger->println("MsgCounter:", counter);
        btSerial.readString(); // '\r\n'
        
        return command;
    }

    return COMMAND_NONE;
}

void Messaging::send(int command, int counter) {
    char buffer[16];
    int bufferLength = sprintf(buffer, "ATmega328P;%i;%i\r\n", command, counter);
    btSerial.write(buffer, bufferLength);
    
    _logger->println("BT Send;", String(buffer));
}