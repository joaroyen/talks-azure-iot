using Newtonsoft.Json;

namespace GoogleSmartHome.Models
{
    public class AvailableToggle
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("name_values")]
        public NameValue[] NameValues { get; set; }
    }
}