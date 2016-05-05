using System.Collections.Generic;
using System;
using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace LOSSPortable{
	class VideoCell : ViewCell{

			public VideoCell(){

                 Color bg;

                 if (Helpers.Settings.ContrastSetting) { bg = Colors.contrastBg; }
                 else { bg = Colors.background; }

                Grid cellView = new Grid {
                    BackgroundColor         = bg,
					VerticalOptions			= LayoutOptions.FillAndExpand,
                    HorizontalOptions       = LayoutOptions.StartAndExpand,
					RowDefinitions			= { new RowDefinition{		Height	= new GridLength(1, GridUnitType.Star)} },
					ColumnDefinitions		= { new ColumnDefinition{	Width	= new GridLength(1, GridUnitType.Star)} }
				};// End of Grid.

				// Create image for cell.
				var cellThum			= new Image();
				cellThum.SetBinding(Image.SourceProperty, new Binding("Image"));
				cellView.Children.Add(cellThum, 0, 2, 1, 17);

				// Create name for cell.
				var cellName			= new Label(){
					BackgroundColor			= Color.Transparent,
					TextColor				= Color.White,
					FontSize				= Device.GetNamedSize(NamedSize.Small, typeof(Label)),
					FontAttributes			= FontAttributes.Bold
				};
				cellName.SetBinding(Label.TextProperty, new Binding("Title"));
				cellView.Children.Add(cellName, 2, 5, 0, 7);

				// Create description for cell.
				var cellDesc			= new Label(){
					BackgroundColor			= Color.Transparent,
					TextColor				= Color.White,
					FontSize				= Device.GetNamedSize(NamedSize.Small, typeof(Label)),
					FontAttributes			= FontAttributes.Italic
				};
				cellDesc.SetBinding(Label.TextProperty, new Binding("Description"));
				cellView.Children.Add(cellDesc, 2, 5, 7, 30);

				this.View				= cellView;
			}// End of ResourceCell() method.
		}// End of ResourceCell class.
}// End of LossPortable namespace.