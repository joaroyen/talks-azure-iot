using GoogleSmartHome.Models;

namespace EchoFunctionApp.Storage
{
    public class CounterExecuteResponseStates : SmartHomeV1ExecuteResponseStatesBase
    {
        public CounterExecuteResponseStates(int counter)
        {
            TemperatureSetpointCelsius = counter;
            TemperatureAmbientCelsius = counter;
        }

        public int TemperatureSetpointCelsius { get; set; }
        public int TemperatureAmbientCelsius { get; set; }
    }
}