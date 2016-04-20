using Acr.UserDialogs;
using Amazon.Lambda;
using Amazon.Lambda.Model;
using Amazon.Util;
using Newtonsoft.Json;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LOSSPortable
{
    public class AccountPage: CarouselPage
    {

        public AccountPage()
        {
            Title = "Account";

            if(Helpers.Settings.ContrastSetting == true)
            {
                BackgroundColor = Colors.contrastBg;
            }
            else
            {
                BackgroundColor = Colors.background;
            }

            if(Helpers.Settings.LoginSetting == true)
            {
                Children.Add(new RegisteredAccountPage()); //displays page similar to profile, an option to change password, general settings, logout option
            }
            else
            {
                Children.Add(new GeneralAccountPage()); //displays general settings, login or register option, report a problem option
            }
        }

        
    }
}
