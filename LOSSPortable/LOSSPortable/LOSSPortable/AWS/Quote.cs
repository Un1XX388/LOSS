using Amazon.DynamoDBv2.DataModel;
using System;

namespace LOSSPortable
{
    /*
     * Create a unique one of these for each parseobject
     * that needs to be created.
     */
    public class Quote
    {
        public string ID { get; set; }

        public string inspirationalQuote { get; set; }

        public string picture { get; set; }
    }
}
