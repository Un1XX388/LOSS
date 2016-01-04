using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace LOSS
{
	public partial class WebViewPage : ContentPage
	{
		public WebViewPage (string URL)
		{
			InitializeComponent ();
            Browser.Source = URL;
		}

        private void backClicked(object sender, EventArgs e)
        {
            //check to see if there is anywhere to go back to
            if (Browser.CanGoBack)
            {
                Browser.GoBack();
            }
            else
            { //if not, leave the view
                Navigation.PopAsync();
            }
        }

        private void forwardClicked(object sender, EventArgs e)
        {
            if (Browser.CanGoForward)
            {
                Browser.GoForward();
            }
        }

	}
}
