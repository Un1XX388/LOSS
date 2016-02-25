using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LOSSPortable
{
    public class AboutPage : ContentPage
    {
        public AboutPage()
        {

            if (Helpers.Settings.ContrastSetting == true)
            {
                BackgroundColor = Colors.contrastBg;
            }
            else
            {
                BackgroundColor = Colors.background;

            }

            Title = "About Us";
            this.Padding = new Thickness(0, Device.OnPlatform(20, 0, 0), 0, 0);

            Label info = new Label
            {
                Text = "This is the page about the monkeys!"
            };

            Content = new StackLayout
            {
                Children = { info }

            };
        }

        //functions would be outside of this
    }
}
