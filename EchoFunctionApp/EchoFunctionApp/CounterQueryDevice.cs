using GoogleSmartHome.Models;

namespace EchoFunctionApp
{
    public class CounterQueryDevice : SmartHomeV1QueryDeviceBase
    {
        public CounterQueryDevice(bool onLine, string status, bool on, int counter, string device) : base(onLine, status)
        {
            On = on;
            Counter = counter;
            Device = device;

            TemperatureSetpointCelsius = counter;
            TemperatureAmbientCelsius = counter;
        }

        public bool On { get; }
        public int TemperatureSetpointCelsius { get; }
        public int TemperatureAmbientCelsius { get; }
        
        public int Counter { get; }
        public string Device { get; }
    }
}