using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LOSSPortable
{
    public class FavoritesPage: ContentPage
    {

        public FavoritesPage()
        {
            //sets the background color based on settings
            this.Title = "Favorites";

            if (Helpers.Settings.ContrastSetting == true)
            {
                this.BackgroundColor = Colors.contrastBg;
            }
            else
            {
                this.BackgroundColor = Colors.background;
            }

            this.Content = new StackLayout
            {
                Children = {
                      new Label { Text = "No favorites", FontAttributes = FontAttributes.Italic, FontSize = Device.GetNamedSize( NamedSize.Default, typeof(Label))}
                 }
            };

        }
    }
}
