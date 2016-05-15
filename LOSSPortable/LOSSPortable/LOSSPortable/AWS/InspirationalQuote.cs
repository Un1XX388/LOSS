using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOSSPortable
{
    //This class is used by HomePage 
    [DynamoDBTable("InspirationalQuote")]
    public class InspirationalQuote
    {
        [DynamoDBHashKey]
        public string Message { get; set; }
    }
}
