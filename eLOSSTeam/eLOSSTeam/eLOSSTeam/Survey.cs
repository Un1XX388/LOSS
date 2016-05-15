using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace eLOSSTeam
{

    /**
     * Creates webview that is directed to the survey intended to be displayed to the user
     */
    public class Survey : ContentPage
    {
        public WebView browser;
        public RangeObservableCollection<Miscellaneous> misc_items { get; set; }

        public Survey()
        {
            Title = "Survey";
            misc_items = AmazonUtils.getMiscList;            // Holds data to be displayed on this content page.


            browser = new WebView();
            for (int i = 0; i < misc_items.Count; i++)
            {
                if (misc_items[i].Type == "Survey Link")
                {
                    browser.Source = misc_items[i].Description;
                }
            }

            this.Content = browser;
        }
        protected override Boolean OnBackButtonPressed() // back button pressed
        {
            ((RootPage)App.Current.MainPage).NavigateTo();

            return true;
        }
    }
}
