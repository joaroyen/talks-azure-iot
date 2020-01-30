using System.Collections.Generic;

namespace GoogleSmartHome.Models
{
    public class SmartHomeV1ExecutePayload : SmartHomeV1Payload
    {
        public List<SmartHomeV1ExecuteResponseCommands> Commands { get; set; } 
            = new List<SmartHomeV1ExecuteResponseCommands>();
        public string DebugString { get; set; }
        public string ErrorCode { get; set; }
    }
}