using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace LOSSPortable
{
        //This class is used by ChatPage and ChatSelection class

    public class UserInfoJson
    {

        public string operation { get; set; }

        public string tableName { get; set; }

        public UserInfoItem  payload { get; set; }


    }

    //Used by ChatPage and ChatSelection class
    public class UserInfoItem
    {
        public object Item { get; set; }
    }


}
