using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LOSSPortable
{
    class ProfilePage: ContentPage
    {
        public ProfilePage()
        {
            Title = "Profile Page";
            this.Padding = new Thickness(0, Device.OnPlatform(20, 0, 0), 0, 0);

            Label report = new Label
            {
                Text = "Profile"
            };

            Content = new StackLayout
            {
                Children = { report }

            };
        }
    }
}
