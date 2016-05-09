using System;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using Plugin.TextToSpeech;

namespace LOSSPortable{
	// This displays all the videos in the selected palylist.
    public class PlaylistPage : ContentPage{
		// Holds all info for each playlist.
        public RangeObservableCollection<OnlineVViewModel> vids { get; set; }
        
		// Gets all relavent info for each video and determines how it should be displayed.
		public PlaylistPage(string playList, string title){

            System.Diagnostics.Debug.WriteLine("PlayList Selected : " + playList);

            //sets the background color based on settings
            if (Helpers.Settings.ContrastSetting == true)
            {
                BackgroundColor = Colors.contrastBg;
            }
            else
            {
                BackgroundColor = Colors.background;
            }

            Title					= playList;

			// Holds data to be displayed on this content page.
            RangeObservableCollection<OnlineVViewModel> tmp = AmazonUtils.getOnlineVList;
			vids					= new RangeObservableCollection<OnlineVViewModel>();
            for (int i = 0; i < tmp.Count; i++)
            {
				if (Device.OS == TargetPlatform.iOS) {
					if (tmp [i].Description.Length > 110) {
						tmp [i].Description = tmp [i].Description.Substring (0, 110) + "...";
					}
				}
                if (tmp[i].Playlist.Equals(playList))
                {
                    vids.Add(tmp[i]);
                }
            }

			ListView VidLstView		= new ListView();            // View type for this content page.
            VidLstView.RowHeight	= 100;                      // Set size (height) of each element displayed on this page.
            this.Title				= title;                    // Set the title of this page.
            VidLstView.ItemsSource	= vids;                     // Set the source of data for page's list view.
            VidLstView.BackgroundColor = BackgroundColor;
			VidLstView.ItemTemplate	= new DataTemplate(typeof(VideoCell));                  // Set layout for each element in this list view.

            VidLstView.ItemSelected	+= Onselected;                  // Set behavior of element when selected by user.
            Content = VidLstView;                                   // Assign the list view created above to this content page.


            // Accomodate iPhone status bar.
            this.Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5);

		}// End of PlaylistPage() constructor.

		// Determines what happens when an element from the list is chosen by the user.
		void Onselected(object sender, SelectedItemChangedEventArgs e){
			if (e.SelectedItem == null){
				return;
			}

			// This deselects the item after it is selected.
			((ListView)sender).SelectedItem = null;
            var select = e.SelectedItem as OnlineVViewModel;

            if (Helpers.Settings.SpeechSetting)
            {
                CrossTextToSpeech.Current.Speak(select.Title);
            }

            VideoPage temp = new VideoPage(select.Title, select.URL);
            if (Device.OS == TargetPlatform.iOS)
            {
                NavigationPage.SetHasNavigationBar(temp, true);
            }
            else
            {
                NavigationPage.SetHasNavigationBar(temp, false);
            }
			
			// Show the selected video to the user.
			Navigation.PushAsync(temp);
		}// End of Onselected() method.

	}// End of PlaylistPage class.
}// End of namespace LOSS.