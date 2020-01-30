namespace GoogleSmartHome.Models
{
    public class SmartHomeV1QueryDeviceBase
    {
        public SmartHomeV1QueryDeviceBase(bool onLine, string status)
        {
            OnLine = onLine;
            Status = status;
        }

        public bool OnLine { get; }
        public string Status { get; }
    }
}