using System;
using Xamarin.Forms;
using System.Collections.Generic;


//
namespace LOSSPortable{
	//
	public class VideoPage : ContentPage{

		// Holds the layout for this page.
		public WebView vid		= new WebView();
		// Holds the url of the video for this page.
		public string url;
       
		// 
		public VideoPage(string pageTitle, string vidURL){
			// Title of page.

            
			Title					= pageTitle;
			// Set url for this page.
			url						= vidURL;
			// Create variable for holding page as a html string.
			var htmlSource			= new HtmlWebViewSource();
			// Compile html string for page.
			htmlSource.Html			= @create_html(url);
			// Set the source for this page (the page as a html stirng).
			vid.Source				= htmlSource;
			// Set the created html string to be the content of the page.
			Content					= vid;
		}// End of VideoPage() constructor.

		// Creates HTML page.
		private string create_html(string url){
			// String for holding each element of html page.
			string[] html_string	= new string[8];

			html_string[0]			= "<html><body bgcolor=\"#";
			html_string[1]			= "000000";
			html_string[2]			= "\">";
			html_string[3]			= "<div style='position:relative; width:100%; height:100%;'>";
			html_string[4]			= "<iframe style='width:100%; height:100%;' frameborder=0 src=";
			html_string[5]			= url;
			html_string[6]			= "></iframe>";
			html_string[7]			= "</div></body></html>";

			// Cat each element of html page into one string and return.
			return String.Join("", html_string);
		}// End of create_html() method.

		// 
		protected override void OnDisappearing(){
			vid.Source				= "";
		}// End of OnDisappearing() method.
	}// End of VideoPage class.
}// End of LOSSPortable namespace.
