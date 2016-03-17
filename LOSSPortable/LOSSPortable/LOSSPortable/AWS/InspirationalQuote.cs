using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOSSPortable
{
    [DynamoDBTable("InspirationalQuote")]
    public class InspirationalQuote
    {
        [DynamoDBHashKey]
        public string id { get; set; }

        [DynamoDBProperty]
        public string Message { get; set; }
    }
}
