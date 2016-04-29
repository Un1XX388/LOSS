using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace LOSSPortable
{
    public class Conversation
    {  //List of message objects; essentially a chat history with a person.
        
        public List<ChatMessage> msgs { get; set; }
        
        public String id { get; set; }
        
        public String name { get; set; }

        public Conversation()
        {
            msgs = new List<ChatMessage>();
        }
    }
}