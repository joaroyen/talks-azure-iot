namespace GoogleSmartHome.Models
{
    public class SmartHomeV1ExecuteRequestCommands
    {
        public SmartHomeV1QueryRequestDevices[] Devices { get; set; }
        public SmartHomeV1ExecuteRequestExecution[] Execution { get; set; }
    }
}