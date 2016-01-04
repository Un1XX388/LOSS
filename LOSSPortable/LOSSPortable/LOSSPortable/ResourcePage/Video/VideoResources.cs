using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace LOSSPortable
{
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
			lstView.ItemTemplate	= new DataTemplate(typeof(PlaylistCell));
            lstView.ItemSelected += Onselected;
			Content					= lstView;
			playlist.Add(new VideoViewModel{
				Image					= "accounts.png",
				Name					= "Playlist Title: Kill Kill Kill",
				Description				= "Description: How to kill.",
				TargetType				= typeof(PlaylistPage)
			});
            
		}// End VideoResources() method.

        void Onselected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                return;
            }
            DisplayAlert("Item Selected", e.SelectedItem.ToString(), "OK");
        }


		public class PlaylistCell : ViewCell{
			public PlaylistCell(){
				Grid cellView			= new Grid{
					VerticalOptions			= LayoutOptions.FillAndExpand,
					RowDefinitions			= { new RowDefinition{		Height	= new GridLength(1, GridUnitType.Star)} },
					ColumnDefinitions		= { new ColumnDefinition{	Width	= new GridLength(1, GridUnitType.Star)} }
				};// End of Grid.

				// Create image for cell.
				var cellThum			= new Image();
				cellThum.SetBinding(Image.SourceProperty, new Binding("Image"));
				cellView.Children.Add(cellThum, 0, 2, 0, 8);

				// Create name for cell.
				var cellName			= new Label(){ BackgroundColor = Color.Transparent, TextColor = Color.White };
				cellName.SetBinding(Label.TextProperty, new Binding("Name"));
				cellView.Children.Add(cellName, 2, 5, 0, 3);

				// Create description for cell.
				var cellDesc			= new Label(){ BackgroundColor = Color.Transparent, TextColor = Color.White };
				cellDesc.SetBinding(Label.TextProperty, new Binding("Description"));
				cellView.Children.Add(cellDesc, 2, 5, 3, 8);

				this.View				= cellView;
			}// End of PlaylistCell() method.
		}// End of PlaylistCell class.
	}// End of VideoResources class.
}// End of namespace LOSS.
