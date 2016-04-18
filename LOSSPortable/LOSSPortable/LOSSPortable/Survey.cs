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
            Title = "Survey";
            browser = new WebView
            {
                Source = "https://uta.qualtrics.com/SE/?SID=SV_3DhjZ0kSqV5thCB"
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
