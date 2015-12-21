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

            var buttonStyle = new Style(typeof(Button))
            {
                Setters = {
                    //Sets background color of Button
                    new Setter {Property = Button.BackgroundColorProperty, Value = Colors.darkblue},
                    //Larger value increases 'roundness' of corner of button, 0 is sharp, square edges.
                    new Setter {Property = Button.BorderRadiusProperty, Value = 1},
                    new Setter {Property = Button.BorderWidthProperty, Value = 2},
                    new Setter {Property = Button.BorderColorProperty, Value = Colors.lightblue},
                    new Setter {Property = Button.HeightRequestProperty, Value = 42},
                    new Setter {Property = Button.FontFamilyProperty, Value = Device.OnPlatform("MarkerFelt-Thin","Droid Sans Mono","Comic Sans MS")}
                    /*new Setter {Property = Button.FontSizeProperty, Value = Device.OnPlatform (24, Device.GetNamedSize (NamedSize.Medium, label), Device.GetNamedSize (NamedSize.Large, label))}*/
                }
            };

			// Set up button that leads to nationwide recources.
			Button nationWideRec = new Button{
				Text = "Nation Wide Resources",
                TextColor = Colors.white,
                Style = buttonStyle,
				Command = new Command(() => Navigation.PushAsync(new NationalResources())),
																 Font = Font.SystemFontOfSize(NamedSize.Large),
																 BorderWidth = 1,
																 HorizontalOptions = LayoutOptions.Center,
																 VerticalOptions = LayoutOptions.CenterAndExpand
			};
			// Set up button that leads to local recources.
			Button localRec = new Button{
				Text = "Local Resources",
				Command = new Command(() => Navigation.PushAsync(new LocalResources())),
																 Font = Font.SystemFontOfSize(NamedSize.Large),
																 BorderWidth = 1,
																 HorizontalOptions = LayoutOptions.Center,
																 VerticalOptions = LayoutOptions.CenterAndExpand
			};
			// Set up button that leads to the video recources.
			Button videoRec = new Button{
				Text = "Video Resources",
				Command = new Command(() => Navigation.PushAsync(new VideoResources())),
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