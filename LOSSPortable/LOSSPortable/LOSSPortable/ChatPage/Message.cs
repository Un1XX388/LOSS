using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LOSSPortable
{
    public class Message : ContentPage
    {
        String sender;
        String message;
        String icon;
        String time;
        //String side;

        public Message ()
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
        
        public String getSender()   { return sender;  }
        public String getMessage()  { return message; }
        public String getIcon()     { return icon;    }
        public String getTime()     { return time;    }
        //public String getSide()     { return side;    }

        public void setSender(String inputSender)   { this.sender = inputSender;   }
        public void setMessage(String inputMessage) { this.message = inputMessage; }
        public void setIcon(String inputIcon)       { this.icon = inputIcon;       }
        public void setTime(String inputTime)       { this.time = inputTime;       }
        //public void setSide(String inputSide)       { this.side = inputSide;       }


    }
}