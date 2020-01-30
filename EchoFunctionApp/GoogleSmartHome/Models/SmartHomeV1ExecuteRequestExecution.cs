using Newtonsoft.Json.Linq;

namespace GoogleSmartHome.Models
{
    public class SmartHomeV1ExecuteRequestExecution
    {
        public string Command { get; set; }
        public JObject Params { get; set; }
    }
}