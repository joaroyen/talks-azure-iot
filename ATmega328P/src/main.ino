#include "main.h"
#include "Input.h"
#include "Display.h"
#include "Messaging.h"

int counter = 0;

Logger logger = Logger();
Input input = Input();
Display display = Display();
Messaging messaging = Messaging(&logger);

void setup() {
    logger.setup();
    display.setup();
    input.setup();
    messaging.setup();

    display.count(counter, "ATmega328P");
}

void loop() {
    inputCommand();
    messageCommand();
}

void inputCommand() {
    int command = input.read();

    if (command == COMMAND_NONE) return;

    counter += command;
    logger.println("Input:", counter);
    display.count(counter, "ATmega328P");
    messaging.send(command, counter);

    delay(250); // Prevent multiple registrations
}

void messageCommand() {
    String msgDevice;
    int msgCounter;
    int command = messaging.receive(msgDevice, msgCounter); 
    
    if (command == COMMAND_NONE) return;

    counter = msgCounter;

    logger.println("Message:", counter);
    display.count(counter, msgDevice);
}