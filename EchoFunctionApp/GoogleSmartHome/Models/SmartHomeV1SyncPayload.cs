using System.Collections.Generic;

namespace GoogleSmartHome.Models
{
    public class SmartHomeV1SyncPayload : SmartHomeV1Payload
    {
        public List<SmartHomeV1SyncDevices> Devices { get; set; }
        public string AgentUserId { get; set; }
        public string DebugString { get; set; }
        public string ErrorCode { get; set; }
    }
}