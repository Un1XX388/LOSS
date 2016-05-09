using Amazon.DynamoDBv2.DataModel;
using System;
using Amazon.Lambda.Model;

namespace LOSSPortable
{
    //This class is used by ChatPage 
    [DynamoDBTable("Message")]
    public class ChatMessage
    {
        [DynamoDBHashKey("ToFrom")]
        public string ToFrom { get; set; }

        [DynamoDBRangeKey("Time")]
        public string Time { get; set; }

        [DynamoDBProperty("Text")]
        public string Text { get; set; }

        [DynamoDBProperty("Id")]
        public string Id { get; set; }

        [DynamoDBProperty("Sender")]
        public string Sender { get; set; }
    }
}
