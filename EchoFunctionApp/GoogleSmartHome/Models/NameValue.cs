using Newtonsoft.Json;

namespace GoogleSmartHome.Models
{
    public class NameValue
    {
        [JsonProperty("name_synonym")]
        public string[] NameSynonym { get; set; }
        public string Lang { get; set; }
    }
}