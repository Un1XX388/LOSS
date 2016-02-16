using System;
using Xamarin.Forms;
using System.Collections.Generic;

namespace LOSSPortable{
	public class VideoPage : ContentPage{
		public VideoPage(string pageTitle, string vidURL){
			// Title of page.
			Title			= pageTitle;
			var vid			= new WebView();
			vid.Source		= vidURL;
			Content			= vid;
		}// End of VideoPage() constructor.
	}// End of VideoPage class.
}// End of LOSSPortable namespace.
