using Newtonsoft.Json.Linq;

namespace GoogleSmartHome.Models
{
    public class SmartHomeV1RequestInput
    {
        public string Intent { get; set; }

        public JObject Payload { get; set; }
        public JObject Context { get; set; }
    }
}