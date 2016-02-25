using System;
using Newtonsoft.Json;
using Xamarin.Forms;
using System.Collections.Generic;



namespace LOSSPortable
{
    public class Contacts
    {  //List of message objects; essentially a chat history with a person.
        //[JsonProperty(PropertyName = "msgs")]
        //public List<ChatPage> contacts { get; set; }
        /*[JsonProperty(PropertyName = "id")]
        public String id { get; set; }
        [JsonProperty(PropertyName = "conv")]
        public List<String> conv { get; set; }
        */
        [JsonProperty(PropertyName = "conv")]
        public List<String> conv { get; set; }

        //[JsonProperty(PropertyName = "layout")]
        //public StackLayout layout { get; set; }


        //[JsonProperty(PropertyName = "name")]
        //public List <String> name { get; set; }


        public Contacts()
        {
            
            this.conv = new List<String>();
            //this.name = new List<String>();
            //this.id = "123456";

        }


    }
}

