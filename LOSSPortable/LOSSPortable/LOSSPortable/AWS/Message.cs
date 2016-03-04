using Amazon.DynamoDBv2.DataModel;
using System;
using Amazon.Lambda.Model;

namespace LOSSPortable
{

    [DynamoDBTable("Message")]
    public class ChatMessage
    {
        [DynamoDBHashKey("To#From")]
        public string ToFrom { get; set; }

        [DynamoDBRangeKey("Time")]
        public string Time { get; set; }

        [DynamoDBProperty("Text")]
        public string Text { get; set; }

        public override string ToString()
        {
            return "To#From:" + ToFrom + ",\nTime:" + Time + ",\nText:" + Text;
        }
    }
}
