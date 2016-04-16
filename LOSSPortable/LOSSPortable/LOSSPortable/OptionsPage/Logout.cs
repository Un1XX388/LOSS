using Acr.UserDialogs;
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
                Text = "You have logged out.",
                TextColor = Color.Black,
                FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label)),
            };
          //  Content = label;

            UserDialogs.Instance.ShowSuccess("You have successfully logged out.");
            Helpers.Settings.LoginSetting = false;
            redirect();

        }
        
        void redirect()
        {
           ((RootPage)App.Current.MainPage).NavigateTo();
        }

    }
}
