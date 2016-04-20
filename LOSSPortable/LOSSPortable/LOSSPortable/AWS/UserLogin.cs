using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LOSSPortable
{
    public class UserLogin 
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Arn { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
        public string Nickname { get; set; }
        public string Email { get; set; }
        public string UserType { get; set; }
        
    }
}
