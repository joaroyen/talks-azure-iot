#ifndef __Logger_h
#define __Logger_h

class Logger {
    public:
        void setup();
        void println(String value);
        void println(String label, String value);
        void println(String label, int value);
        void blankln();
};

#endif