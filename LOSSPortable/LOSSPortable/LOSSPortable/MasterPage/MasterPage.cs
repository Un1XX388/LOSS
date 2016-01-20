using System.Collections.Generic;
using Xamarin.Forms;
using System;

namespace LOSSPortable{
	public class MasterPage : ContentPage{
		public ListView ListView { get { return listView; } }

		ListView listView;

        public MasterPage(){
			var masterPageItems = new List<MasterPageItem>();

			masterPageItems.Add(new MasterPageItem{
				Title = "Home",
				IconSource = "home.png",
				TargetType = typeof(HomePage)
			});

			masterPageItems.Add(new MasterPageItem{
				Title = "Resources",
				IconSource = "play.png",
				TargetType = typeof(ResourcesPage)
			});

			masterPageItems.Add(new MasterPageItem{
				Title = "Conversations",
				IconSource = "chat.png",
				TargetType = typeof(ChatSelection)
			});

			masterPageItems.Add(new MasterPageItem{
			Title = "Options",
			IconSource = "option.png",
			TargetType = typeof(OptionsPage)
			});

			listView = new ListView{
				ItemsSource = masterPageItems,
				ItemTemplate = new DataTemplate(() => {
						var imageCell = new ImageCell();
						imageCell.SetValue(TextCell.TextColorProperty, Color.Black);
						imageCell.SetBinding(TextCell.TextProperty, "Title");
						imageCell.SetBinding(ImageCell.ImageSourceProperty, "IconSource");
						return imageCell;
					}),
					VerticalOptions = LayoutOptions.FillAndExpand,
					SeparatorVisibility = SeparatorVisibility.Default
			};


			Icon = "drawable/menu.png";
			Title = "MENU";
			listView.RowHeight = 45;
			var menuLabel = new ContentView{
				Padding = new Thickness(10,10,0,10),
				Content = new Label{
					TextColor = Color.Black,
					Text = "MENU",
				}
			};

			var hotlineButton = new Button{
				Text = string.Format("Call Suicide Hotline")
			};
			hotlineButton.Clicked += (sender, args) =>{
				Device.OpenUri(new Uri(string.Format("tel:{0}", "+11111111111")));
			};

			var layout = new StackLayout{
				Spacing = 0,
				VerticalOptions = LayoutOptions.FillAndExpand
			};
			layout.Children.Add(menuLabel);
			layout.Children.Add(new BoxView(){
				Color = Color.Black, HeightRequest = 1, Opacity = 0.5
			});
			layout.Children.Add(listView);
			layout.Children.Add(hotlineButton);
			Content = layout;
		}// End of MasterPage() method.
	}// End of MasterPage class.
}// End of LOSSPortable namemspace.