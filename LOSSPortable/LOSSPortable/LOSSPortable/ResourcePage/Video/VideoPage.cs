using System;
using Xamarin.Forms;
using System.Collections.Generic;

namespace LOSSPortable{
	public class VideoPage : ContentPage{
		public VideoPage(string pageTitle, string vidURL){
			// Hide the navigation bar.
//			NavigationPage.SetHasNavigationBar(this, false);
//			NavigationController.SetNavigationBarHidden(true, true);
//			SupportActionBar.Hide();
//			ActionBar.Hide();
			// Hide the Nav bar.
//			UIApplication.SharedApplication.SetStatusBarHidden(true, true);

			// Title of page.
			Title					= pageTitle;
			// Create web view content page.
			var vid					= new WebView();
			// Create variable for holding page as a html string.
			var htmlSource			= new HtmlWebViewSource();
			// Compile html string for page.
			htmlSource.Html			= @create_html("5a3c5c", vidURL);
			// Set the source for this page (the page as a html stirng).
			vid.Source				= htmlSource;
			// Set the created html string to be the content of the page.
			Content = vid;
		}// End of VideoPage() constructor.

		// Creates HTML page.
		private string create_html(string color, string url){
			// String for holding each element of html page.
			string[] html_string	= new string[42];

			html_string[0]			= "<html>";
			html_string[1]			= "<body bgcolor=\"#";
			html_string[2]			= color;
			html_string[3]			= "\">";
			html_string[4]			= "<div ";
			html_string[5]			= "style='";
			html_string[6]			= "position: ";
			html_string[7]			= "relative;";
			html_string[8]			= "width: ";
			html_string[9]			= "100";
			html_string[10]			= "%;height: ";
			html_string[11]			= "0";
			html_string[12]			= "px;padding-bottom: ";
			html_string[13]			= "60";
			html_string[14]			= "%;'> ";
			html_string[15]			= "<iframe ";
			html_string[16]			= "style='";
			html_string[17]			= "position: ";
			html_string[18]			= "absolute";
			html_string[19]			= ";left: ";
			html_string[20]			= "0";
			html_string[21]			= "px;";
			html_string[22]			= "top: ";
			html_string[23]			= "0";
			html_string[24]			= "px;";
			html_string[25]			= "width: ";
			html_string[26]			= "100";
			html_string[27]			= "%;";
			html_string[28]			= "height: ";
			html_string[29]			= "100";
			html_string[30]			= "%' ";
			html_string[31]			= "src=\"";
			html_string[32]			= url;
			html_string[33]			= "\" ";
			html_string[34]			= "frameborder=\"";
			html_string[35]			= "0";
			html_string[36]			= "\" ";
			html_string[37]			= "allowfullscreen>";
			html_string[38]			= "</iframe> ";
			html_string[39]			= "</div>";
			html_string[40]			= "</body>";
			html_string[41]			= "</html/>";

			// Cat each element of html page into one string and return.
			return String.Join("", html_string);
		}// End of create_html() method.
	}// End of VideoPage class.
}// End of LOSSPortable namespace.
