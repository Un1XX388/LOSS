using System;
using Xamarin.Forms;
using System.Collections.Generic;

namespace LOSSPortable{
	public class VideoPage : ContentPage{
		public VideoPage(string pageTitle, string vidURL){
			// Title of page.
			Title					= pageTitle;
			// Create web view content page.
			var vid					= new WebView();
			// Create variable for holding page as a html string.
			var htmlSource			= new HtmlWebViewSource();

			// Get width of device's screen.
			int width				= (App.ScreenWidth-15);
			// Calcualte correct height of video based on video width.
			int height				= (int)(width * 0.5625);
			// Compile html string for page.
			htmlSource.Html			= @create_html("5a3c5c", width, height, vidURL);
			// Set the source for this page (the page as a html stirng).
			vid.Source				= htmlSource;
			// Set the created html string to be the content of the page.
			Content = vid;
		}// End of VideoPage() constructor.

		// Creates HTML page.
		private string create_html(string color, int width, int height, string url){
			// String for holding each element of html page.
			string html_start		= "<html>";
			string body_start		= "<body bgcolor=\"#";
			string bg_color			= color;
			string iframe_start		= "\"><iframe ";
			string width_label		= "width=\"";
			string vid_width		= width.ToString();
			string height_label		= "\" height=\"";
			string vid_height		= height.ToString();
			string url_label		= "\" src=\"";
			string vid_link			= url;
			string border_label		= "\" frameborder=\"";
			string vid_border		= "0\" ";
			string vid_fs			= "allowfullscreen>";
			string iframe_end		= "</iframe>";
			string body_end			= "</body>";
			string html_end			= "</html/>";

			// Cat each element of html page into one string and return.
			return html_start+body_start+bg_color+iframe_start+width_label+vid_width+height_label+vid_height+url_label+vid_link+border_label+vid_border+vid_fs+iframe_end+body_end+html_end;
		}// End of create_html() method.
	}// End of VideoPage class.
}// End of LOSSPortable namespace.
