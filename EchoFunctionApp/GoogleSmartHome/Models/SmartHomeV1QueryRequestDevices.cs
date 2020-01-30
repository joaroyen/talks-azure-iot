using Newtonsoft.Json.Linq;

namespace GoogleSmartHome.Models
{
    public class SmartHomeV1QueryRequestDevices
    {
        public string Id { get; set; }
        public JObject CustomData { get; set; }
    }
}