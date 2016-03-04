using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LOSSPortable
{
    public class Survey : ContentPage
    {
        public WebView browser;

        public Survey()
        {
            browser = new WebView
            {
                Source = "https://qtrial2016q1az1.qualtrics.com/SE/?SID=SV_ezirKxBzZdk9Kzr"
            };

            this.Content = browser;
        }
        protected override Boolean OnBackButtonPressed() // back button pressed
        {
            ((RootPage)App.Current.MainPage).NavigateTo();

            return true;
        }
    }
}
