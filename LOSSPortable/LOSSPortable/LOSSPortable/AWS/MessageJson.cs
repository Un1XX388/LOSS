using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace LOSSPortable
{
    public class MessageJson
    {

        public string operation { get; set; }

        public string tableName { get; set; }

        public object payload { get; set; }
        

    }

    public class MessageItem
    {
        public ChatMessage Item { get; set; }
    }

    public class UserItem
    {
        public UserLogin Item { get; set; }
    }
}
