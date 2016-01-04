using System;
using Xamarin.Forms;
using System.Collections.Generic;

namespace LOSSPortable{

	public class ResourcesPage : ContentPage{

		public ResourcesPage(){
			Title			= "Resources";
			Icon			= "Leads.png";

			var buttonStyle = new Style(typeof(Button)){
				Setters = {
					//Sets background color of Button
					new Setter{ Property = Button.BackgroundColorProperty, Value = Color.Blue },
					//Larger value increases 'roundness' of corner of button, 0 is sharp, square edges.
					new Setter{ Property = Button.BorderRadiusProperty, Value = 1 },
					new Setter{ Property = Button.BorderWidthProperty, Value = 2 },
					new Setter{ Property = Button.BorderColorProperty, Value = Color.Aqua },
					new Setter{ Property = Button.HeightRequestProperty, Value = 42 },
					new Setter{ Property = Button.FontFamilyProperty, Value = Device.OnPlatform("MarkerFelt-Thin","Droid Sans Mono","Comic Sans MS") }
				}
			};

			// Set up button that leads to all online recources.
			Button onlineRec = new Button{
				Text					= "Online Resources",
                TextColor				= Color.White,
                Style					= buttonStyle,
				Command					= new Command(() => Navigation.PushAsync(new OnlineResources())),
				Font					= Font.SystemFontOfSize(NamedSize.Large),
				BorderWidth				= 1,
				HorizontalOptions		= LayoutOptions.Center,
				VerticalOptions			= LayoutOptions.CenterAndExpand
			};
			// Set up button that leads to the video recources.
			Button videoRec = new Button{
				Text					= "Video Resources",
                TextColor				= Color.White,
                Style					= buttonStyle,
				Command					= new Command(() => Navigation.PushAsync(new VideoResources())),
				Font					= Font.SystemFontOfSize(NamedSize.Large),
				BorderWidth				= 1,
				HorizontalOptions		= LayoutOptions.Center,
				VerticalOptions			= LayoutOptions.CenterAndExpand
			};

			// Set what each button does when it is pressed.
			onlineRec.Clicked		+= delegate{};
			videoRec.Clicked		+= delegate{};
 
			// Accomodate iPhone status bar.
			this.Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5);
 
			// Build the page.
			this.Content = new StackLayout{
				Children = {
					onlineRec,
					videoRec
				}
			};
		}// End of ResourcesPage() method.
	}// End of ResourcesPage class.
}// End of namespace LOSS.