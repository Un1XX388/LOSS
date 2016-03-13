using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOSSPortable
{
    [DynamoDBTable("MessageReport")]
    class ReportM
    {
        [DynamoDBHashKey]
        public String id { get; set; }

        [DynamoDBProperty]
        public String Date { get; set; }

        [DynamoDBProperty]
        public String Type { get; set; }

        [DynamoDBProperty]
        public String Message { get; set; }

        [DynamoDBProperty]
        public String Comment { get; set; }

        [DynamoDBProperty]
        public String From { get; set; }

        [DynamoDBProperty]
        public String To { get; set; }
    }
}
