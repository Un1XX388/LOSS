using System;


namespace LOSSPortable
{
    public class Message
    {   //id, sender, reciever, text, icon, time
        public string id { get; set; }

        public string sender { get; set; }

        public string reciever { get; set; }

        public string text { get; set; }

        public string icon { get; set; }

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

        public String getId() { return id; }
        public String getSender() { return sender; }
        public String getMessage() { return text; }
        public String getIcon() { return icon; }
        public String getTime() { return time; }
        //public String getSide()     { return side;    }

        public void setId(String inputId) { this.id = inputId; }
        public void setSender(String inputSender) { this.sender = inputSender; }
        public void setMessage(String inputMessage) { this.text = inputMessage; }
        public void setIcon(String inputIcon) { this.icon = inputIcon; }
        public void setTime(String inputTime) { this.time = inputTime; }
        //public void setSide(String inputSide)       { this.side = inputSide;       }


    }
}