using System;
using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace LOSSPortable{
    public class PlaylistPage : ContentPage{
		// Holds all info for each playlist.
		public ObservableCollection<VideoViewModel> vids{ get; set; }

		public PlaylistPage(){
			Title					= "Kill Kill Kill";
			vids					= new ObservableCollection<VideoViewModel> ();
			ListView VidLstView		= new ListView();
			VidLstView.RowHeight	= 100;
			VidLstView.ItemsSource	= vids;
			VidLstView.ItemTemplate	= new DataTemplate(typeof(VideoCell));
			VidLstView.ItemSelected	+= Onselected;

			vids.Add(new VideoViewModel{
				Image				= "home.png",
				Title				= "Title: Why Kill?",
				Description			= "Description: Reasonse to kill.",
				Link				= "1"
			});
			vids.Add(new VideoViewModel{
				Image				= "home.png",
				Title				= "Title: How to Kill.",
				Description			= "Description: Best methods of killing.",
				Link				= "https://youtu.be/p0srsGgZ1Pc"
			});
			vids.Add(new VideoViewModel{
				Image				= "home.png",
				Title				= "Title: How to cleen up your work.",
				Description			= "Description: Basic steps to not getting caught.",
				Link				= "NULL"
			});

			Content = VidLstView;
		}// End of PlaylistPage() constructor.


		void Onselected(object sender, SelectedItemChangedEventArgs e){
			if (e.SelectedItem == null){
				return;
			}
				Navigation.PushAsync(new Video1Page());
		}// End of Onselected() method.
	}// End of PlaylistPage class.
}// End of namespace LOSS.