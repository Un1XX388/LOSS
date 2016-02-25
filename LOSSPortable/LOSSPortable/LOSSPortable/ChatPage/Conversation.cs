using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace LOSSPortable
{
    public class Conversation
    {  //List of message objects; essentially a chat history with a person.
        [JsonProperty(PropertyName = "msgs")]
        public List<Message> msgs { get; set; }
        [JsonProperty(PropertyName = "id")]
        public String id { get; set; }
        [JsonProperty(PropertyName = "username")]
        public String name { get; set; }

        public Conversation()
        {
            msgs = new List<Message>();
        }


    }
}