using System;
using Xamarin.Forms;
using System.Collections.ObjectModel;


namespace LOSSPortable{


		public class OnlineResourceCell : ViewCell{

        public Boolean favClicked = false;

        public OnlineResourceCell(){
            Color bg;

            if (Helpers.Settings.ContrastSetting == true) { bg = Colors.contrastBg; }
            else { bg = Colors.background; }

                Grid cellView			    = new Grid{
                    BackgroundColor         = bg,
					VerticalOptions			= LayoutOptions.FillAndExpand,
                    HorizontalOptions       = LayoutOptions.StartAndExpand,
					RowDefinitions			= { new RowDefinition{		Height	= new GridLength(1, GridUnitType.Star)} },
					ColumnDefinitions		= { new ColumnDefinition{	Width	= new GridLength(1, GridUnitType.Star)} }
				};// End of Grid.

				// Create image for cell.
				var cellThum			= new Image();
				cellThum.SetBinding(Image.SourceProperty, new Binding("Image"));
				cellView.Children.Add(cellThum, 0, 1, 1, 12);

				// Create name for cell.
				var cellName			= new Label(){
					BackgroundColor			= Color.Transparent,
					TextColor				= Color.White,
					FontSize				= Device.GetNamedSize(NamedSize.Small, typeof(Label)),
					FontAttributes			= FontAttributes.Bold
					};
				cellName.SetBinding(Label.TextProperty, new Binding("Title"));
				cellView.Children.Add(cellName, 1, 4, 0, 4);

				// Create description for cell.
				var cellDesc			= new Label(){
					BackgroundColor			= Color.Transparent,
					TextColor				= Color.White,
					FontSize				= Device.GetNamedSize(NamedSize.Small, typeof(Label)),
					FontAttributes			= FontAttributes.Italic
					};
				cellDesc.SetBinding(Label.TextProperty, new Binding("Description"));
				cellView.Children.Add(cellDesc, 1, 4, 4, 30);

            //var cellFav = new Image();
            //cellFav.VerticalOptions = LayoutOptions.Center;
            //cellFav.HorizontalOptions = LayoutOptions.Center;
            //cellFav.SetBinding(Image.SourceProperty, new Binding("Fav"));
            //cellView.Children.Add(cellFav, 4, 5, 0, 13);

            //cellFav.GestureRecognizers.Add(new TapGestureRecognizer
            //{
            //    Command = new Command(
            //               () => cellFav.Source = isFavorited()),
            //});

            View = cellView;
		}// End of OnlineResourceCell() method.
    }// End of OnlineResourceCell class.
}// End of LossPortable namespace.
