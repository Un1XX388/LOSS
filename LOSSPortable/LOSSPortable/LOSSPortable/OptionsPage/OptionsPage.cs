using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LOSSPortable
{
    public class OptionsPage : ContentPage
    {
        Label label;

        public OptionsPage()
        {
            Title = "Options";
            /**/

            Label header = new Label
            {
                Text = "Enable High Contrast Mode",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center
            };

            Switch switcher = new Switch
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            switcher.Toggled += switcher_Toggled;

            label = new Label
            {
                Text = "Switch is now False",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            StackLayout header_stack = new StackLayout
            {
                Children =
                {
                    header
                },
                HorizontalOptions = LayoutOptions.Start
            };

            StackLayout switcher_stack = new StackLayout
            {
                Children =
                {
                    switcher
                },
                HorizontalOptions = LayoutOptions.EndAndExpand
            };

            StackLayout row = new StackLayout
            {
                Children = {
                    header_stack, switcher_stack
                },
                Orientation = StackOrientation.Horizontal,
                Padding = new Thickness(5, 5)
            };


            this.Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5);

            // Build the page.
            this.Content = new StackLayout
            {
                Children =
                {
                    row,
                    label
                }
            };
        }

        void switcher_Toggled(object sender, ToggledEventArgs e)
        {
            label.Text = String.Format("High Contrast Mode Enabled? {0}", e.Value);
        }
    }
}
