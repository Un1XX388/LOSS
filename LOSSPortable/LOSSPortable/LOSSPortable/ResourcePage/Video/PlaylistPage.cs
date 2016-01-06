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
			vids.Add(new VideoViewModel{
				Image				= "play.png",
				Title				= "Why kill a baby?",
				Description			= "It seems like creatures killing infants of their own species is a very common occurrence. Why is this a thing?",
				Link				= "https://youtu.be/REUzqgRh3GU"
			});
			vids.Add(new VideoViewModel{
				Image				= "play.png",
				Title				= "Top 10 Foods that Can Kill You",
				Description			= "You may or may not be able to stay alive long enough to enjoy these deadly delicacies!",
				Link				= "https://youtu.be/p0srsGgZ1Pc"
			});
			vids.Add(new VideoViewModel{
				Image				= "play.png",
				Title				= "Murder Suicide Cleanup in a Home, OSHA Blood borne Pathogens, PPE, Hazcom",
				Description			= "This Trauma Scene Cleanup Quick Reference Guide is part of an open-source training platform comprised of a 3 mil laminated UV resistant plastic cover (3 2-sided pages, 6 total pages), with OSHA Standards Bloodborne Pathogens 1910.1030, Confined Space 1910.146, Respiratory Protection 1910.134, PPE 1910.132, and Haz Com 1910.1200 explained next to HD photo case studies and the associated training material slides for each of these OSHA standards.",
				Link				= "https://youtu.be/jWjnWiU4egw"
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