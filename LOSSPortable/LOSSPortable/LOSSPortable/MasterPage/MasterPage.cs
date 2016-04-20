using System.Collections.Generic;
using Xamarin.Forms;
using System;
using Acr.UserDialogs;

namespace LOSSPortable
{
    public class MasterPage : ContentPage
    {
        public ListView ListView { get { return listView; } }

        ListView listView;
        Label login;
        String logText = "Login";
        Boolean loggedIn = false;
        Image logImage;
        MasterPageItem temp;
        ContentPage accountPage;

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

            masterPageItems.Add(new MasterPageItem
            {
                Title = "Conversations",
                IconSource = "chat.png",
                TargetType = typeof(ChatSelection)
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
                Device.OpenUri(new Uri(string.Format("tel:{0}", "+11111111111")));
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
          //  layout.Children.Add(login);
           // layout.Children.Add(row8_login);
            layout.Children.Add(hotlineButton);
            Content = layout;
        }// End of MasterPage() method.

        private void LoginButton_Clicked(object sender, EventArgs e)
        {
            login_check();
        }

        //This function checks if Login or Logout is pressed and prompts user accordingly.

        public async void login_check()
        {
            if (loggedIn == false) //login is currently displayed. set loggedIn to true to display logout
            {
                System.Diagnostics.Debug.WriteLine("Login currently displayed - loggedIn" + loggedIn);
                //entry pop up for login
                //    var r = await UserDialogs.Instance.LoginAsync(new LoginConfig
                //    {
                //        Message = "Enter Credentials",
                //        LoginPlaceholder = Helpers.Settings.EmailSetting,
                //        //  PasswordPlaceholder = Helpers.Settings.PasswordSetting
                //    });

                //    var status = r.Ok ? "Success" : "Cancelled";
                //    //  System.Diagnostics.Debug.WriteLine("after status = r.ok?");

                //    // this.Result($"Login {status} - User Name: {r.LoginText} - Password: {r.Password}");

                //    if (status == "Success")
                //    {
                //        Helpers.Settings.EmailSetting = r.LoginText;
                //       // Helpers.Settings.PasswordSetting = r.Password;
                //        loggedIn = true;
                //        login.Text = "Logout";
                //        logImage.Source = Device.OnPlatform(iOS: ImageSource.FromFile("logout64.png"), Android: ImageSource.FromFile("logout64.png"), WinPhone: ImageSource.FromFile("logout64.png"));
                //        System.Diagnostics.Debug.WriteLine("status is success then logout should be displayed - loggedIn" + loggedIn);
                //        return;
                //    }
                //    else if (status == "Cancelled")
                //    {
                //        // loggedIn = false;
                //        login.Text = "Login";
                //        logImage.Source = Device.OnPlatform(iOS: ImageSource.FromFile("login64.png"), Android: ImageSource.FromFile("login64.png"), WinPhone: ImageSource.FromFile("login64.png"));
                //        System.Diagnostics.Debug.WriteLine("status is cancelled then login should be displayed - loggedIn" + loggedIn);
                //        return;
                //    }
                logImage.Source = Device.OnPlatform(iOS: ImageSource.FromFile("login64.png"), Android: ImageSource.FromFile("login64.png"), WinPhone: ImageSource.FromFile("login64.png"));
                login.Text = "Logout";
                loggedIn = true;
                ((RootPage)App.Current.MainPage).NavigateTo();
            }
            else if (loggedIn == true)//if logout is displayed
            {
                System.Diagnostics.Debug.WriteLine("Logout currently displayed - loggedIn" + loggedIn);


                //pop up confirmation
                var result = await DisplayAlert("Log Out", "Are you sure?", "Yes", "No");
                if (result == false)
                {
                    // loggedIn = true;
                    login.Text = "Logout";
                    logImage.Source = Device.OnPlatform(iOS: ImageSource.FromFile("logout64.png"), Android: ImageSource.FromFile("logout64.png"), WinPhone: ImageSource.FromFile("logout64.png"));
                    System.Diagnostics.Debug.WriteLine("User clicked on No, logout should still be displayed - loggedIn" + loggedIn);
                    return;
                }
                else
                {
                    loggedIn = false;
                    login.Text = "Login";
                    logImage.Source = Device.OnPlatform(iOS: ImageSource.FromFile("login64.png"), Android: ImageSource.FromFile("login64.png"), WinPhone: ImageSource.FromFile("login64.png"));
                    System.Diagnostics.Debug.WriteLine("User clicked on Yes, login should be displayed - loggedIn" + loggedIn);
                    return;
                }
            }

        }

    }// End of MasterPage class.
}// End of LOSSPortable namemspace.