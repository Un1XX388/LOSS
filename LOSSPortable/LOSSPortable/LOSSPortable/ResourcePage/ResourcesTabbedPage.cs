using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LOSSPortable
{
    class ResourcesTabbedPage: TabbedPage
    {
        public ResourcesTabbedPage()
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

            this.Title = "Resources";
            this.Children.Add(new OnlineResources()
            );
            this.Children.Add(new VideoResources()
            );
            this.Children.Add(new ContentPage
            {
                Title = "Favorites",
                Content = new StackLayout
                {
                    Children = {
                        new Label { Text = "No favorites", FontAttributes = FontAttributes.Italic }
                    }
                }
            });
        }

    }
}
