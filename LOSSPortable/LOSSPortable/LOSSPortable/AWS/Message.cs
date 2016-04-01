using Amazon.DynamoDBv2.DataModel;
using System;
using Amazon.Lambda.Model;

namespace LOSSPortable
{

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

        [DynamoDBProperty("Reciever")]
        public string Reciever { get; set; }

        [DynamoDBProperty("Icon")]
        public string Icon { get; set; }

        [DynamoDBProperty("Date")]
        public string Date { get; set; }


    }
}
