using System;
using Newtonsoft.Json;

namespace LOSSPortable
{
    /*
     * Create a unique one of these for each parseobject
     * that needs to be created.
     */
    public class SampleItem
    {
        [JsonProperty(PropertyName = "id")]
        public string ID { get; set; }
        
        [JsonProperty(PropertyName = "sample1")]
        public string Sample1 { get; set; }

        [JsonProperty(PropertyName = "sample2")]
        public string Sample2 { get; set; }
    }
}
