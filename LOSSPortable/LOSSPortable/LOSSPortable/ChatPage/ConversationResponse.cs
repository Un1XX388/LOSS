using Amazon.DynamoDBv2.DataModel;
using System;
using Amazon.Lambda.Model;

namespace LOSSPortable
{


    public class ConversationResponse
    { //response from server

        public string Nickname { get; set; }

        public string ID { get; set; }

        public string Distance { get; set; }

        public string Success { get; set; }

        public string Message { get; set; }
    }
}
