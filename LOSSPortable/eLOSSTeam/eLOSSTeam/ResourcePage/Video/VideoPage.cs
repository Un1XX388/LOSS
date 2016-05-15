using System;
using Xamarin.Forms;
namespace eLOSSTeam{

	public class VideoPage : ContentPage{

		public WebView vid		= new WebView();             // Holds the layout for this page.
        public string url;                                  // Holds the url of the video for this page.


        // 
        public VideoPage(string pageTitle, string vidURL){

			//sets the background color based on settings
			if (Helpers.Settings.ContrastSetting == true) 
			{
				BackgroundColor = Colors.contrastBg;
			} 
			else 
			{
				BackgroundColor = Colors.background;
			}
            
			Title					= pageTitle;
			url						= vidURL;
			var htmlSource			= new HtmlWebViewSource();                      // Create variable for holding page as a html string.
            htmlSource.Html			= @create_html(url);                            // Compile html string for page.
            vid.Source				= htmlSource;                                   // Set the source for this page (the page as a html stirng).

            Content = vid;                                      // Set the created html string to be the content of the page.

        }// End of VideoPage() constructor.

        // Creates HTML page.
        private string create_html(string url){
			string[] html_string	= new string[8];                        // String for holding each element of html page.

            html_string[0]			= "<html><body bgcolor=\"#";
			html_string[1]			= "000000";
			html_string[2]			= "\">";
			html_string[3]			= "<div style='position:relative; width:100%; height:100%;'>";
			html_string[4]			= "<iframe style='width:100%; height:100%;' frameborder=0 src=";
			html_string[5]			= url;
			html_string[6]			= "></iframe>";
			html_string[7]			= "</div></body></html>";

			return String.Join("", html_string);                                // Cat each element of html page into one string and return.
        }// End of create_html() method.

        
        protected override void OnDisappearing(){
			vid.Source				= "";
		}// End of OnDisappearing() method.

	}// End of VideoPage class.
}// End of LOSSPortable namespace.
