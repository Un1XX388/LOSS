using System;
using Xamarin.Forms;
using System.Collections.Generic;

namespace LOSSPortable{
	public class VideoPage : ContentPage{
		public VideoPage(string pageTitle, string vidURL){

            if (Helpers.Settings.ContrastSetting == true)
            {
                BackgroundColor = Colors.contrastBg;
            }
            else
            {
                BackgroundColor = Colors.background;
            }

            // Title of page.
            Title			= pageTitle;
			var vid1		= new WebView();
			vid1.Source		= vidURL;
			Content			= vid1;
		}// End of Video1Page() constructor.
	}// End of Video1Page class.
}// End of LOSSPortable namespace.
