using System;
using Xamarin.Forms;
using System.Collections.Generic;

namespace LOSS{

	public class ResourcesPage : ContentPage{
		// Leads to nation wide recources.
		private Button nationWideRec;
		// Leads to local recources.
		private Button localRec;
		// Leads to video recources.
		private Button videoRec;

		private int clickTotal=0;
		private Label label;

		public ResourcesPage(){
			Title = "Resources";
			Icon = "Leads.png";
			
			// Set up button that leads to nationwide recources.
			Button nationWideRec = new Button{
				Text = "Nation Wide Recources",
				Command = new Command(() => Navigation.PushAsync(new NationalRecources())),
																 Font = Font.SystemFontOfSize(NamedSize.Large),
																 BorderWidth = 1,
																 HorizontalOptions = LayoutOptions.Center,
																 VerticalOptions = LayoutOptions.CenterAndExpand
			};
			// Set up button that leads to local recources.
			Button localRec = new Button{
				Text = "Local Recources",
				Command = new Command(() => Navigation.PushAsync(new LocalRecources())),
																 Font = Font.SystemFontOfSize(NamedSize.Large),
																 BorderWidth = 1,
																 HorizontalOptions = LayoutOptions.Center,
																 VerticalOptions = LayoutOptions.CenterAndExpand
			};
			// Set up button that leads to the video recources.
			Button videoRec = new Button{
				Text = "Video Recources",
				Command = new Command(() => Navigation.PushAsync(new VideoRecources())),
																 Font = Font.SystemFontOfSize(NamedSize.Large),
																 BorderWidth = 1,
																 HorizontalOptions = LayoutOptions.Center,
																 VerticalOptions = LayoutOptions.CenterAndExpand
			};

			// Set what each button does when it is pressed.
			nationWideRec.Clicked += delegate{};
			localRec.Clicked += delegate{};
			videoRec.Clicked += delegate{};
 
			// Accomodate iPhone status bar.
			this.Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5);
 
			// Build the page.
			this.Content = new StackLayout{
			Children =
				{
					nationWideRec,
					localRec,
					videoRec
				}
			};
		}
	}
	
}