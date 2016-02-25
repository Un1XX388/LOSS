using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LOSSPortable
{
    public class ReportPage: ContentPage
    {
        public ReportPage()
        {
            //sets the background color based on settings
            if (Helpers.Settings.ContrastSetting == true)
            {
                BackgroundColor = Colors.contrastBg;
            }
            else
            {
                BackgroundColor = Colors.background;
            }


            Title = "Report A Problem";
            this.Padding = new Thickness(0, Device.OnPlatform(20, 0, 0), 0, 0);

            Label report = new Label
            {
                Text = "Bug Reports or any other problems"
            };

            Content = new StackLayout
            {
                Children = { report }

            };
        }


    }
}
