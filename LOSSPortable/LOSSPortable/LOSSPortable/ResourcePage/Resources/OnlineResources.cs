using System;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using Plugin.TextToSpeech;


namespace LOSSPortable{
	// Page displays online resources to end user.
	public class OnlineResources : ContentPage{
		// Holds all info for each item on resources page.
		public ObservableCollection<OnlineResourceViewModel> online_resources{ get; set; }

		// 
		public OnlineResources(){
			//sets the background color based on settings
			if (Helpers.Settings.ContrastSetting == true){
				BackgroundColor				= Colors.contrastBg;
			}
			else{
				BackgroundColor				= Colors.background;
			}

			// Holds data to be displayed on this content page.
			online_resources			= new ObservableCollection<OnlineResourceViewModel>();
			// View type for this content page.
			ListView lstView			= new ListView();
			// Set size (height) of each element displayed on this page.
			lstView.RowHeight			= 70;
			// Set title of this page.
			Title						= "Online Resources";
			// Set source of data for the list view used on this page.
			lstView.ItemsSource			= online_resources;
			// Set layout for each element in this list view.
			lstView.ItemTemplate		= new DataTemplate(typeof(OnlineResourceCell));
			// Set behavior of element when selected by user.
			lstView.ItemSelected		+= Onselected;
			// Assign the list view created above to this content page.
			Content						= lstView;

			// 
			online_resources.Add(new OnlineResourceViewModel{
				Image						= "quote64.png",
				Title						= "Feel Good Quotes",
				Description					= "The most popular user submitted inspirational quotes.",
				Link						= "http://www.brainyquote.com/quotes/keywords/feel_good.html",
				Fav							= "Fav"
			});
			online_resources.Add(new OnlineResourceViewModel{
				Image						= "quote64.png",
				Title						= "FEEL GOOD QUOTES: 25 quotations that...",
				Description					= "Each morning when I open my eyes I say to myself: I, not...",
				Link						= "http://www.themindfulword.org/2013/feel-good-quotes/",
				Fav							= "Fav"
			});

			// Accomodate iPhone status bar.
			Padding						= new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5);
		}// End of OnlineResources() constructor.


		// Determines what happens when an element from the list is chosen by the user.
		void Onselected(object sender, SelectedItemChangedEventArgs e){
			if (e.SelectedItem == null){
				return;
			}
			// This deselects the item after it is selected.
			((ListView)sender).SelectedItem	= null;

			if (Helpers.Settings.SpeechSetting == true){
				string text					= ((e.SelectedItem.ToString()).Split(','))[1];
				CrossTextToSpeech.Current.Speak(text);
			}

			// Determine what next page should be based on what element was chosen.
			if ( ((e.SelectedItem.ToString()).Split(','))[3].Equals("online") )
				Navigation.PushAsync(new OnlineResources());
			else if( ((e.SelectedItem.ToString()).Split(','))[3].Equals("video") )
				Navigation.PushAsync(new VideoResources());
			else if ( ((e.SelectedItem.ToString()).Split(','))[3].Substring(0, 12).Equals("https://youtu") )
				Navigation.PushAsync(new VideoPage( ((e.SelectedItem.ToString()).Split(','))[1], ((e.SelectedItem.ToString()).Split(','))[3] ));
			else
				Navigation.PushAsync(new ViewOnlineResource( ((e.SelectedItem.ToString()).Split(','))[1], ((e.SelectedItem.ToString()).Split(','))[3] ));
		}// End of Onselected() method.

		// 
		protected override Boolean OnBackButtonPressed(){
			((RootPage)App.Current.MainPage).NavigateTo();
			return true;
		}// End of OnBackButtonPressed() method.
	}// End of OnlineResources class.
}// End of namespace LOSS.