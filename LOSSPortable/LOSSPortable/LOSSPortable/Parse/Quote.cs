using System;
using Newtonsoft.Json;

namespace LOSSPortable
{
    class Quote
    {
        [JsonProperty(PropertyName = "id")]
        public string quoteID { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string inspirationalQuote { get; set; }

        [JsonProperty(PropertyName = "image")]
        public string picture { get; set; }
    }
}
