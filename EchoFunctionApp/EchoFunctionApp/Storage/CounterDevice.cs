using GoogleSmartHome.Models;

namespace EchoFunctionApp.Storage
{
    public class CounterDevice
    {
        public bool On { get; set; }
        public string Device { get; set; }
        public int Counter { get; set; }

        public CounterQueryDevice QueryResponse()
        {
            return new CounterQueryDevice(true, SmartHomeV1ExecuteStatus.Success, On, Counter, Device);
        }

        public SmartHomeV1ExecuteResponseStatesBase ExecuteResponseState()
        {
            return new CounterExecuteResponseStates(Counter);
        }

        public CounterUpdatedEvent CounterUpdated()
        {
            return new CounterUpdatedEvent { Counter = Counter, Device = Device };
        }

        public void SetTemperature(decimal temperature)
        {
            Counter = (int) temperature;
            Device = "GoogleAssistant";
        }

        public void Increase()
        {
            SetTemperature(Counter + 1);
        }

        public void Decrease()
        {
            SetTemperature(Counter - 1);
        }
    }
}