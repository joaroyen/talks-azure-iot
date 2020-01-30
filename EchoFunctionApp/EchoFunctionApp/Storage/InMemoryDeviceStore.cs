namespace EchoFunctionApp.Storage
{
    // Fake in-memory singleton store for a single device
    public class InMemoryDeviceStore
    {
        public CounterDevice Counter { get; set; } = new CounterDevice();
    }
}