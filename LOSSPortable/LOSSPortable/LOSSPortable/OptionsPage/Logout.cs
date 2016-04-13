using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LOSSPortable
{
    public class Logout: ContentPage
    {
        public Logout()
        {
            BackgroundColor = Color.White;
            var label = new Label()
            {
                Text = "Logging out..."
            };
            Content = label;
            Helpers.Settings.LoginSetting = false;
            ((RootPage)App.Current.MainPage).NavigateTo();

        }


    }
}
