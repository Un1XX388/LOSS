using System.Collections.Generic;
using Xamarin.Forms;
using System;
using Acr.UserDialogs;
using Plugin.TextToSpeech;

namespace LOSSPortable
{
    public class MasterPage : ContentPage
    {
        public ListView ListView { get { return listView; } }

        ListView listView;
        Boolean loggedIn = false;


        public MasterPage()
        {
            var masterPageItems = new List<MasterPageItem>();

            masterPageItems.Add(new MasterPageItem
            {
                Title = "Homepage",
                IconSource = "homepage.png",
                TargetType = typeof(HomePage)
            });

            masterPageItems.Add(new MasterPageItem
            {
                Title = "Resources",
                IconSource = "play.png",
                TargetType = typeof(ResourcesTabbedSwipePage)
            });

			//disabled conversations menu item on iOS for sponsor. Conversations tab only displayed on Android.
			Device.OnPlatform(Android: () => {
				
	            masterPageItems.Add(new MasterPageItem
	            {
	                Title = "Conversations",
	                IconSource = "chat.png",
	                TargetType = typeof(ChatSelection)
	            });
			}); 

            masterPageItems.Add(new MasterPageItem
            {
                Title = "Account",
                IconSource = "account.png",
                TargetType = typeof(AccountPage)
            });

            masterPageItems.Add(new MasterPageItem
            {
                Title = "Survey",
                IconSource = "survey.png",
                TargetType = typeof(Survey)
            });
            masterPageItems.Add(new MasterPageItem
            {
                Title = "About Us",
                IconSource = "about.png",
                TargetType = typeof(AboutPage)
            });

            listView = new ListView
            {
                ItemsSource = masterPageItems,
                ItemTemplate = new DataTemplate(() =>
                {
                    var imageCell = new ImageCell();
                    imageCell.SetValue(TextCell.TextColorProperty, Color.Black);
                    imageCell.SetValue(TextCell.TextProperty, FontAttributes.Bold);
                    imageCell.SetValue(TextCell.DetailColorProperty, Color.FromHex("B3B3B3"));
                    imageCell.SetBinding(TextCell.TextProperty, "Title");
                    imageCell.SetBinding(ImageCell.ImageSourceProperty, "IconSource");
                    return imageCell;
                }),
                VerticalOptions = LayoutOptions.FillAndExpand,
            };

            Icon = "drawable/menu.png";
            Title = "MENU";
            listView.RowHeight = 60;
			listView.BackgroundColor = Color.FromHex ("B3B3B3");
            Device.OnPlatform(Android: () => {
                listView.SeparatorVisibility = SeparatorVisibility.Default;
                listView.SeparatorColor = Color.FromHex("4D345D"); //separator between options
            },
            iOS: () => {
                listView.SeparatorVisibility = SeparatorVisibility.None;
                listView.SeparatorColor = Color.FromHex("4D345D"); //separator between options
            });

            var menuLabel = new ContentView
            {

                Padding = new Thickness(10, 10, 0, 10),
                Content = new Label
                {
                    TextColor = Color.Black,
                    FontSize = 18,
                    FontFamily = "Arial",
                    BackgroundColor = Color.FromHex("B3B3B3"),
                    Text = "MENU",
                }
            };

            var hotlineButton = new Button
            {
                Text = string.Format("Call Suicide Hotline")
            };
            hotlineButton.Clicked += (sender, args) => {
				Device.OpenUri(new Uri(string.Format("tel:{0}", "+1(800)2738255")));
            };

            var layout = new StackLayout
            {
                Spacing = 0,
                VerticalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = Color.FromHex("B3B3B3")
            };
            layout.Children.Add(menuLabel);
            layout.Children.Add(
                new BoxView()
                { //Line under settings 
                    Color = Color.FromHex("4D345D"),
                    HeightRequest = 6
                });

            layout.Children.Add(listView);
            layout.Children.Add(hotlineButton);
            Content = layout;
        }// End of MasterPage() method.
        
    }// End of MasterPage class.
}// End of LOSSPortable namemspace.