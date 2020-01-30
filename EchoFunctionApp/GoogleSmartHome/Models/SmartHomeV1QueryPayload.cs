using System.Collections.Generic;

namespace GoogleSmartHome.Models
{
    public class SmartHomeV1QueryPayload : SmartHomeV1Payload
    {
        public Dictionary<string, SmartHomeV1QueryDeviceBase> Devices { get; set; }
    }
}