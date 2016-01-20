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

			// The bellow items will be taken from the server in the final build (they are temporary).
			// IMPORTANT NOTE:
				// The max length of a title is 50 char + "..."
				// The max length of a description is 90 char + "..."
				// Video links from youtube playlists may not be used.
			vids.Add(new VideoViewModel{
				Image				= "vid1.jpg",
				Title				= "Creationist Cat solves the abortion crisis!",
				Description			= "Hey guys, last month I took part in Rick Perry's 'da Response' gaddering and let me tell you...",
				Link				= "https://youtu.be/lbHbp8Penrg"
			});
			vids.Add(new VideoViewModel{
				Image				= "vid2.jpg",
				Title				= "Creationist Cat sings!!!",
				Description			= "Whaddup sodomites? My debut album is dropping and diss shit is hotter dan da devil's freaking",
				Link				= "https://youtu.be/UjdutAqWE4s"
			});
			vids.Add(new VideoViewModel{
				Image				= "vid3.jpg",
				Title				= "SuperKitty Hugs & Cuddles Fun Time!",
				Description			= "A liddle someting for da fans dat have been itching to pet me...",
				Link				= "https://youtu.be/UjdutAqWE4s"
			});
			vids.Add(new VideoViewModel{
				Image				= "vid4.jpg",
				Title				= "Breaking News: I'm not a Hermaphrodite!",
				Description			= "In diss very special news bulletin, I, Creationist Cat, dispel deez stupid freaking rumors...",
				Link				= "https://youtu.be/u3Sl_KoC79s"
			});
			vids.Add(new VideoViewModel{
				Image				= "vid5.jpg",
				Title				= "Creastionist Cat: Science vs. REAL MIRACLES!",
				Description			= "Hey kids, tawt I'd take a break from pwning hellbound sodomites today and give you da word...",
				Link				= "https://youtu.be/5Jc5-bCf16w"
			});
			vids.Add(new VideoViewModel{
				Image				= "vid6.jpg",
				Title				= "",
				Description			= "",
				Link				= ""
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