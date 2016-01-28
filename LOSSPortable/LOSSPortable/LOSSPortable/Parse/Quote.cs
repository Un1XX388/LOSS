using System;
using Newtonsoft.Json;

namespace LOSSPortable
{
    /*
     * Create a unique one of these for each parseobject
     * that needs to be created.
     */
    public class Quote
    {
        [JsonProperty(PropertyName = "id")]
        public string ID { get; set; }

        [JsonProperty(PropertyName = "Message")]
        public string inspirationalQuote { get; set; }

        [JsonProperty(PropertyName = "image")]
        public string picture { get; set; }
    }
}
