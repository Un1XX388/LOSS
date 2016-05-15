using Amazon.DynamoDBv2.DataModel;
using System;


namespace eLOSSTeam
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
