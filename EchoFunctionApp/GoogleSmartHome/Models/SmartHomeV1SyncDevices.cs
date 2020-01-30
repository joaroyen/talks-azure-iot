using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace GoogleSmartHome.Models
{
    public class SmartHomeV1SyncDevices
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public List<string> Traits { get; set; }
        public SmartHomeV1SyncName Name { get; set; }
        public SmartHomeV1SyncDeviceInfo DeviceInfo { get; set; }
        public bool WillReportState { get; set; }
        public JObject Attributes { get; set; }
        public JObject CustomData { get; set; }
    }
}