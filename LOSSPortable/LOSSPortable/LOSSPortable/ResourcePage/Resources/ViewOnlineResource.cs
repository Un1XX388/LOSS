using System;
using Xamarin.Forms;
using System.Collections.Generic;


namespace LOSSPortable{
	public class ViewOnlineResource : ContentPage{
		public ViewOnlineResource(string page_title, string url){
			// Title of page.
			Title					= page_title;
			// Create web view content page.
			var resource			= new WebView();
			// Set the source for this page (the page as a html stirng).
			resource.Source			= url;
			// Set the created html string to be the content of the page.
			Content					= resource;
		}// End of VideoPage() constructor.
	}// End of VideoPage class.
}// End of LOSSPortable namespace.
