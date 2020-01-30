#ifndef __Messaging_h
#define __Messaging_h

class Messaging {
    public:
        Messaging(Logger* logger);
        void setup();
        int receive(String &device, int &counter);
        void send(int command, int counter);
    private:
        Logger* _logger;
};

#endif