using System;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using Plugin.TextToSpeech;


namespace LOSSPortable{
	// This page displays 3 online resources, a link to the online resources page, 3 video resources, and a link to the video resource page.
	public class ResourcesPage : ContentPage{
		// Holds all info for each item on resources page.
		public ObservableCollection<ResourceViewModel> resources{ get; set; }

		// Gets relavent info for each element in page and displays them.
		public ResourcesPage(){

			//sets the background color based on settings
			if (Helpers.Settings.ContrastSetting == true){
				BackgroundColor				= Colors.contrastBg;
			}
			else{
				BackgroundColor				= Colors.background;
			}


			// Holds data to be displayed on this content page.
			resources					= new ObservableCollection<ResourceViewModel>();
			// View type for this content page.
			ListView lstView			= new ListView();
			// Set size (height) of each element displayed on this page.
			lstView.RowHeight			= 70;
			// Set title of this page.
			this.Title					= "Resource(s)";
			// Set source of data for the list view used on this page.
			lstView.ItemsSource			= resources;
			// Set layout for each element in this list view.
			lstView.ItemTemplate		= new DataTemplate(typeof(ResourceCell));
			// Set behavior of element when selected by user.
			lstView.ItemSelected		+= Onselected;
			// Assign the list view created above to this content page.
			Content						= lstView;

			// The bellow items will be taken from the server in the final build (they are temporary).
			// IMPORTANT NOTE:
				// The max length of a title is 36 char + "..."
				// The max length of a description is 82 char + "..."
				// Video links from youtube playlists may not be used.
			resources.Add(new ResourceViewModel{
				Image						= "quote64.png",
				Title						= "Feel Good Quotes",
				Description					= "The most popular user submitted inspirational quotes.",
				Link						= "http://www.brainyquote.com/quotes/keywords/feel_good.html"
			});
			resources.Add(new ResourceViewModel{
				Image						= "quote64.png",
				Title						= "FEEL GOOD QUOTES: 25 quotations that...",
				Description					= "Each morning when I open my eyes I say to myself: I, not events, have the power to...",
				Link						= "http://www.themindfulword.org/2013/feel-good-quotes/"
			});
			resources.Add(new ResourceViewModel{
				Image						= "arrow5364.png",
				Title						= "Online Resources",
				Description					= "Choose this to see even more online resources.",
				Link						= "online"
			});
			resources.Add(new ResourceViewModel{
				Image						= "videoplay64.png",
				Title						= "Mark Ronson - Uptown Funk ft. Bruno...",
				Description					= "Mark Ronson's official music video for 'Uptown Funk' ft. Bruno Mars.",
				Link						= "https://youtu.be/OPf0YbXqDm0"
			});
			resources.Add(new ResourceViewModel{
				Image						= "videoplay64.png",
				Title						= "Mark Ronson - Feel Right ft. Mystikal",
				Description					= "Mark Ronson – Feel right Ft. Mystikal",
				Link						= "https://youtu.be/ognnZ3r2qyQ"
			});
			resources.Add(new ResourceViewModel{
				Image						= "arrow5364.png",
				Title						= "Video Resources",
				Description					= "Choose this to see even more videos.",
				Link						= "video"
			});
 
			// Accomodate iPhone status bar.
			this.Padding				= new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5);
		}// End of ResourcesPage() constructor.

		// Determines what happens when an element from the list is chosen by the user.
		void Onselected(object sender, SelectedItemChangedEventArgs e){
			if (e.SelectedItem == null){
				return;
			}
			// This deselects the item after it is selected.
			((ListView)sender).SelectedItem	= null;

			if (Helpers.Settings.SpeechSetting == true){
				string text						= ((e.SelectedItem.ToString()).Split(','))[1];
				CrossTextToSpeech.Current.Speak(text);
			}


			// Determine what next page should be based on what element was chosen.
			if ( ((e.SelectedItem.ToString()).Split(','))[3].Equals("online") )
				Navigation.PushAsync(new OnlineResources());
			else if( ((e.SelectedItem.ToString()).Split(','))[3].Equals("video") )
				Navigation.PushAsync(new VideoResources());
			else
				Navigation.PushAsync(new VideoPage( ((e.SelectedItem.ToString()).Split(','))[1], ((e.SelectedItem.ToString()).Split(','))[3] ));
		}// End of Onselected() method.

		protected override Boolean OnBackButtonPressed(){
			((RootPage)App.Current.MainPage).NavigateTo();
			return true;
		}// End of OnBackButtonPressed() method.
	}// End of ResourcesPage class.
}// End of namespace LOSS.