using System;

using Newtonsoft.Json;

namespace LOSSPortable
{
    public class Message
    {   //id, sender, reciever, text, icon, time
        [JsonProperty(PropertyName = "id")]
        public string id { get; set; }

        [JsonProperty(PropertyName = "sender")]
        public string sender { get; set; }

        [JsonProperty(PropertyName = "reciever")]
        public string reciever { get; set; }

        [JsonProperty(PropertyName = "text")]
        public string text { get; set; }

        [JsonProperty(PropertyName = "icon")]
        public string icon { get; set; }

        [JsonProperty(PropertyName = "time")]
        public string time { get; set; }

        /*public Message ()
        {
            this.sender = "";
            this.message = "";
            this.icon = "";
            this.time = "";
            //this.side = "";
        }

        public Message(String sender, String message, String icon, String time )
        { 
            this.sender = sender;
            this.message = message;
            this.icon = icon;
            this.time = time;
            //this.side = side;
        } 
        */


        /*public String getSender()   { return sender;  }
        public String getMessage()  { return message; }
        public String getIcon()     { return icon;    }
        public String getTime()     { return time;    }
        //public String getSide()     { return side;    }

        public void setSender(String inputSender)   { this.sender = inputSender;   }
        public void setMessage(String inputMessage) { this.message = inputMessage; }
        public void setIcon(String inputIcon)       { this.icon = inputIcon;       }
        public void setTime(String inputTime)       { this.time = inputTime;       }
        //public void setSide(String inputSide)       { this.side = inputSide;       }
        */

    }
}