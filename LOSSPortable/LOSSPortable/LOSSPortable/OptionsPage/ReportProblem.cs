using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOSSPortable
{
    [DynamoDBTable("Reports")]
    class ReportProblem
    {
        [DynamoDBHashKey]
        public string id { get; set; }

        [DynamoDBProperty]
        public string Date { get; set; }

        [DynamoDBProperty]
        public String Message { get; set; }

        [DynamoDBProperty]
        public string Type { get; set; }

    }
}
