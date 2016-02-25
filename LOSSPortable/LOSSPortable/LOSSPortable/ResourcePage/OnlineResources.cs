using System;
using Xamarin.Forms;
using System.Collections.Generic;

namespace LOSSPortable{
	public class OnlineResources : ContentPage{
		public OnlineResources(){
            if (Helpers.Settings.ContrastSetting == true)
            {
                BackgroundColor = Colors.contrastBg;
            }
            else
            {
                BackgroundColor = Colors.background;

            }
            Title = "Online Resources";
		}// End of OnlineResources() constructor.
	}// End of OnlineResources class.
}// End of namespace LOSS.