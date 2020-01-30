using Newtonsoft.Json;

namespace GoogleSmartHome.Models
{
    public class SmartHomeV1ExecuteRequestContext
    {
        [JsonProperty("locale_country")]
        public string LocaleCountry { get; set; }
        [JsonProperty("locale_language")]
        public string LocaleLanguage { get; set; }
    }
}