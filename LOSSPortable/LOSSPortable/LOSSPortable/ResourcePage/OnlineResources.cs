using System;
using Xamarin.Forms;
using System.Collections.Generic;
using Plugin.TextToSpeech;


namespace LOSSPortable{
	public class OnlineResources : ContentPage{
		public OnlineResources(){
			//sets the background color based on settings
			if (Helpers.Settings.ContrastSetting == true){
				BackgroundColor				= Colors.contrastBg;
			}
			else{
				BackgroundColor				= Colors.background;
			}
			// Set title of page.
			Title						= "Online Resources";

			// 
		}// End of OnlineResources() constructor.
	}// End of OnlineResources class.
}// End of namespace LOSS.