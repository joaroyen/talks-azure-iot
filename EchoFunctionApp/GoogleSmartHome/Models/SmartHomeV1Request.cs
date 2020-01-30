namespace GoogleSmartHome.Models
{
    public class SmartHomeV1Request
    {
        public string RequestId { get; set; }
        public SmartHomeV1RequestInput[] Inputs { get; set; }

    }
}