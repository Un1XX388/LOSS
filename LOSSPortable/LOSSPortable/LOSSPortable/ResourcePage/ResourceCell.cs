using System;
using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace LOSSPortable{
		public class ResourceCell : ViewCell{
			public ResourceCell(){
				Grid cellView			= new Grid{
					VerticalOptions			= LayoutOptions.FillAndExpand,
					RowDefinitions			= { new RowDefinition{		Height	= new GridLength(1, GridUnitType.Star)} },
					ColumnDefinitions		= { new ColumnDefinition{	Width	= new GridLength(1, GridUnitType.Star)} }
				};// End of Grid.

				// Create image for cell.
				var cellThum			= new Image();
				cellThum.SetBinding(Image.SourceProperty, new Binding("Image"));
				cellView.Children.Add(cellThum, 0, 2, 1, 17);

				// Create name for cell.
				var cellName			= new Label(){
					BackgroundColor = Color.Transparent,
					TextColor = Color.White,
					FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
					FontAttributes = FontAttributes.Bold
					};
				cellName.SetBinding(Label.TextProperty, new Binding("Title"));
				cellView.Children.Add(cellName, 2, 5, 0, 7);

				// Create description for cell.
				var cellDesc			= new Label(){
					BackgroundColor = Color.Transparent,
					TextColor = Color.White,
					FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
					FontAttributes = FontAttributes.Italic
				};
				cellDesc.SetBinding(Label.TextProperty, new Binding("Description"));
				cellView.Children.Add(cellDesc, 2, 5, 7, 30);

				this.View				= cellView;
			}// End of ResourceCell() method.
		}// End of ResourceCell class.
}// End of LossPortable namespace.
