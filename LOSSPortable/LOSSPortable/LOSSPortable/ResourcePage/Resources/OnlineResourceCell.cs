using System;
using Xamarin.Forms;
using System.Collections.ObjectModel;


namespace LOSSPortable{


		public class OnlineResourceCell : ViewCell{

        public Boolean favClicked = false;

        public OnlineResourceCell(){

				Grid cellView			= new Grid{
					VerticalOptions			= LayoutOptions.FillAndExpand,
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

				// Create favorite option.
				var cellFav				= new Button(){
					Font					= Font.SystemFontOfSize(NamedSize.Small),
					BorderRadius			= 10,
					HorizontalOptions		= LayoutOptions.Center
					};
				cellFav.SetBinding(Button.TextProperty, new Binding("Fav"));
				cellView.Children.Add(cellFav, 4, 5, 0, 13);
                cellFav.Clicked += CellFav_Clicked;

				View					= cellView;
			}// End of OnlineResourceCell() method.

        public void CellFav_Clicked(object sender, EventArgs e)
        {
          //  System.Diagnostics.Debug.WriteLine("Favorite Button Clicked");
            favClicked = true;
            System.Diagnostics.Debug.WriteLine(favClicked);


        }
    }// End of OnlineResourceCell class.
}// End of LossPortable namespace.
