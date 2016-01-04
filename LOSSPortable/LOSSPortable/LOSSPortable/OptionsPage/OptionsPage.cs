using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace LOSSPortable
{
    public class OptionsPage : ContentPage
    {
        public OptionsPage()
        {
            Title = "Options";
            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "Options Page goes here!" }
                }
            };
        }
    }
}
