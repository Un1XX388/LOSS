using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LOSSPortable
{
    /*
    {
        "Subject": "Message / Announcement",
        "Title": "New announcement",
        "Text": "Hello",
        "Time": "2016-04-06 16:38:08.618757",
        "ToFrom": "U-79854921478565921900#R-6031175061964857259",
        "Sender": "Gerald"
    }
    */
    public class SNSMessage
    {
        public string Subject { set; get; }

        public string Title { set; get; }

        public string Text { set; get; }

        public string Time { set; get; }

        public string ToFrom { set; get; }

        public string Sender { set; get; }
    }
}