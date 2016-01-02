using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq;

namespace LOSS{

	public class MenuTableView : TableView{
		public MenuTableView (){
//			List<MenuItem> data = new MenuTableData ();
			// Construct the table.
			Intent = TableIntent.Form;
			Root = new TableRoot("Table Title"){
				new TableSection("Table Section"){
					new TextCell{
						Text = "Resources",
						Command = new Command(() => new NavigationPage(new ResourcesPage()))
					},
					new TextCell{
						Text = "Feedback",
						Command = new Command(() => new NavigationPage(new FeedbackPage()))
					}
				}
			};

//			ItemsSource = data;
			VerticalOptions = LayoutOptions.FillAndExpand;
			BackgroundColor = Color.Transparent;
//			SeparatorVisibility = SeparatorVisibility.None;

			var cell = new DataTemplate (typeof(MenuCell));
			cell.SetBinding (MenuCell.TextProperty, "Title");
			cell.SetBinding (MenuCell.ImageSourceProperty, "IconSource");

//			ItemTemplate = cell;
		}
	}
}