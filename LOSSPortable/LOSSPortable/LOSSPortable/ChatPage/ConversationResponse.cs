using Amazon.DynamoDBv2.DataModel;
using System;
using Amazon.Lambda.Model;

namespace LOSSPortable
{


    public class ConversationResponse
    { //response from server

        [DynamoDBProperty("Nickname")]
        public string Nickname { get; set; }

        [DynamoDBProperty("ID")]
        public string ID { get; set; }

        [DynamoDBProperty("Distance")]
        public string Distance { get; set; }

        [DynamoDBProperty("Arns")]
        public string ID { get; set; }
        

    }
}
