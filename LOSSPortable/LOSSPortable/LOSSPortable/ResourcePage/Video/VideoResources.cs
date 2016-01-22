using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace LOSSPortable{
	// This page displays all the video playlist(s) to the user.
	public class VideoResources : ContentPage{
		// Holds all info for each playlist.
		public ObservableCollection<VideoViewModel> playlist{ get; set; }

		// Gets info on each playlist and determines how it is displayed.
		public VideoResources(){
			playlist				= new ObservableCollection<VideoViewModel> ();
			ListView lstView		= new ListView();
			lstView.RowHeight		= 100;
			this.Title				= "Playlist(s)";
			lstView.ItemsSource		= playlist;
			lstView.ItemTemplate	= new DataTemplate(typeof(VideoCell));
			lstView.ItemSelected	+= Onselected;
			Content					= lstView;
			// The item being added below is a test. In final release, this information should be retrieved from the server.
			playlist.Add(new VideoViewModel{
				Image					= "vid1.jpg",
				Title					= "Uptown Special",
				Description				= "No description.",
				Link					= "https://www.youtube.com/playlist?list=PLpz-Cm0bpQJ4beQi28bBpwEq8nRauirQT"
			});
            
		}// End VideoResources() method.

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