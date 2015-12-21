using System;
using Newtonsoft.Json;

namespace LOSS
{
    public class ResourceItem
    {
        [JsonProperty(PropertyName = "objectID")]
        public string objectID { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string description { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string type { get; set; }

        [JsonProperty(PropertyName = "url")]
        public string url { get; set; }
    }
}
