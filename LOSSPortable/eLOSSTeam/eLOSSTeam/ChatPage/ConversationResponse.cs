using Amazon.DynamoDBv2.DataModel;
using System;
using Amazon.Lambda.Model;

namespace eLOSSTeam
{


    public class ConversationResponse
    { //response from server

        public string Nickname { get; set; }

        public string ID { get; set; }

        public string Distance { get; set; }

        public string Success { get; set; }

        public string Message { get; set; }

        public ConversationList[] Conversations { get; set; }

        public MessageLst[] MessageList { get; set; }
    }

    public class ConversationList
    {
        public string Nickname { get; set; }

        public string ID { get; set; }
    }

    public class MessageLst
    {
        public string Text{ get; set; }

        public string ToFrom{ get; set; }

        public string Time{ get; set; }
    }
}
