using System;
using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace LOSSPortable{
    public class PlaylistPage : ContentPage{
		// Holds all info for each playlist.
		public ObservableCollection<ResourceViewModel> vids{ get; set; }

		public PlaylistPage(){
            if (Helpers.Settings.ContrastSetting == true)
            {
                BackgroundColor = Colors.contrastBg;
            }
            else
            {
                BackgroundColor = Colors.background;
            }

            Title					= "Uptown Special";
			vids					= new ObservableCollection<ResourceViewModel> ();
			ListView VidLstView		= new ListView();
			VidLstView.RowHeight	= 100;
			VidLstView.ItemsSource	= vids;
			VidLstView.ItemTemplate	= new DataTemplate(typeof(ResourceCell));
			VidLstView.ItemSelected	+= Onselected;

			// The bellow items will be taken from the server in the final build (they are temporary).
			// IMPORTANT NOTE:
				// The max length of a title is 50 char + "..."
				// The max length of a description is 90 char + "..."
				// Video links from youtube playlists may not be used.
			vids.Add(new ResourceViewModel{
				Image				= "vid1.jpg",
				Title				= "Mark Ronson - Uptown Funk ft. Bruno Mars",
				Description			= "Mark Ronson's official music video for 'Uptown Funk' ft. Bruno Mars.",
				Link				= "https://youtu.be/OPf0YbXqDm0"
			});
			vids.Add(new ResourceViewModel{
				Image				= "vid2.jpg",
				Title				= "Mark Ronson - Feel Right ft. Mystikal",
				Description			= "Mark Ronson – Feel right Ft. Mystikal",
				Link				= "https://youtu.be/ognnZ3r2qyQ"
			});
			vids.Add(new ResourceViewModel{
				Image				= "vid3.jpg",
				Title				= "Mark Ronson - Daffodils (Audio) ft. Kevin Parker",
				Description			= "You can vote Uptown Funk as British Artist Video Of The Year by tweeting using #BRITMarkRonson",
				Link				= "https://youtu.be/-OWkLF2HLp0"
			});
			vids.Add(new ResourceViewModel{
				Image				= "vid4.jpg",
				Title				= "Mark Ronson - Feel Right (Lyric Video) ft. Mystikal",
				Description			= "You can vote Uptown Funk as British Artist Video Of The Year by tweeting using #BRITMarkRonson",
				Link				= "https://youtu.be/gb73FC6I_0U"
			});
			vids.Add(new ResourceViewModel{
				Image				= "vid5.jpg",
				Title				= "Mark Ronson - Uptown Funk (Live on SNL) ft. Bruno Mars",
				Description			= "You can vote Uptown Funk as British Artist Video Of The Year by tweeting using #BRITMarkRonson",
				Link				= "https://youtu.be/GbGX1Sx0gvo"
			});
			vids.Add(new ResourceViewModel{
				Image				= "vid6.jpg",
				Title				= "Mark Ronson - Feel Right (Live on SNL) ft. Mystikal",
				Description			= "You can vote Uptown Funk as British Artist Video Of The Year by tweeting using #BRITMarkRonson",
				Link				= "https://youtu.be/b0kl0pr0k24"
			});

			Content = VidLstView;
		}// End of PlaylistPage() constructor.


		void Onselected(object sender, SelectedItemChangedEventArgs e){
			if (e.SelectedItem == null){
				return;
			}
			// This deselects the item after it is selected.
			((ListView)sender).SelectedItem = null;
			// Show the selected video to the user.
			Navigation.PushAsync(new VideoPage( ((e.SelectedItem.ToString()).Split(','))[1], ((e.SelectedItem.ToString()).Split(','))[3] ));
		}// End of Onselected() method.
	}// End of PlaylistPage class.
}// End of namespace LOSS.