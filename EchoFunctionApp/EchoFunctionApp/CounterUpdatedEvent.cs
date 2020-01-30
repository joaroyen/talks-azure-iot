namespace EchoFunctionApp
{
    public class CounterUpdatedEvent
    {
        public string Device { get; set; }
        public int Counter { get; set; }

        public bool IsValid() => !string.IsNullOrWhiteSpace(Device);
    }
}