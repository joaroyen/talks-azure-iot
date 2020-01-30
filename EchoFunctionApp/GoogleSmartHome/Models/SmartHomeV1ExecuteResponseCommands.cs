using System.Collections.Generic;

namespace GoogleSmartHome.Models
{
    public class SmartHomeV1ExecuteResponseCommands
    {
        public List<string> Ids { get; set; }
        public string Status { get; set; }
        public SmartHomeV1ExecuteResponseStatesBase States { get; set; }
        public string DebugString { get; set; }
        public string ErrorCode { get; set; }
    }
}