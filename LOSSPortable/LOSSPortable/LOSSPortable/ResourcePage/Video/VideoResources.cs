using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace LOSSPortable{
	// This page displays all the video playlist(s) to the user.
	public class VideoResources : ContentPage{
		// Holds all info for each playlist.
		public ObservableCollection<ResourceViewModel> playlist{ get; set; }

		// Gets info on each playlist and determines how it is displayed.
		public VideoResources(){
			// Holds data to be displayed on this content page.
			playlist				= new ObservableCollection<ResourceViewModel>();
			// View type for this content page.
			ListView lstView		= new ListView();
			// Set size (height) of each element displayed on this page.
			lstView.RowHeight		= 100;
			// Set the title of this page.
			this.Title				= "Playlist(s)";
			// Set the source of data for page's list view.
			lstView.ItemsSource		= playlist;
			// Set layout for each element in this list view.
			lstView.ItemTemplate	= new DataTemplate(typeof(VideoCell));
			// Set behavior of element when selected by user.
			lstView.ItemSelected	+= Onselected;
			// Assign the list view created above to this content page.
			Content					= lstView;

			// The bellow items will be taken from the server in the final build (they are temporary).
			// IMPORTANT NOTE:
				// The max length of a title is 50 char + "..."
				// The max length of a description is 90 char + "..."
				// Video links from youtube playlists may not be used.
			playlist.Add(new ResourceViewModel{
				Image					= "vid1.jpg",
				Title					= "Uptown Special",
				Description				= "No description.",
				Link					= "https://www.youtube.com/playlist?list=PLpz-Cm0bpQJ4beQi28bBpwEq8nRauirQT"
			});

			// Accomodate iPhone status bar.
			this.Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5);
		}// End VideoResources() constructor.

		// Determines what happens when an element from the list is chosen by the user.
		void Onselected(object sender, SelectedItemChangedEventArgs e){
			if (e.SelectedItem == null){
				return;
			}
			// This deselects the item after it is selected.
			((ListView)sender).SelectedItem = null;
			// When given playlist is selected user is brought to the page listed below.
			Navigation.PushAsync(new PlaylistPage());
		}// End of Onselected() method.
	}// End of VideoResources class.
}// End of namespace LOSS.