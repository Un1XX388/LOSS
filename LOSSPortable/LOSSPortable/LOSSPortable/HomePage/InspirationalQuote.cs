using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOSSPortable
{
    [DynamoDBTable("InspirationalQuote")]
    class InspirationalQuote
    {
        [DynamoDBHashKey]
        public string id { get; set; }

        [DynamoDBProperty]
        public string Message { get; set; }
/*
        [DynamoDBProperty]
        public Boolean isField { get; set; }

        [DynamoDBProperty]
        public string testField { get; set; }
*/

    }
}
