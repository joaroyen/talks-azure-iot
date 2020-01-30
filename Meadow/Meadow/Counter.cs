namespace Meadow
{
    public class Counter
    {
        public int Value { get; private set; }
        public string DeviceName { get; private set; } = "";
        public CounterDirection Direction { get; private set; } = CounterDirection.Unknown;

        public void Up(string deviceName)
        {
            lock (this)
            {
                Direction = CounterDirection.Up;
                Value++;
                DeviceName = deviceName;
            }    
        }

        public void Down(string deviceName)
        {
            lock (this)
            {
                Direction = CounterDirection.Down;
                Value--;
                DeviceName = deviceName;
            }
        }
    }
}