using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Plugin.TextToSpeech;
using Acr.UserDialogs;
using XLabs.Forms.Controls;


namespace LOSSPortable
{
    public class VolunteerPortal : ContentPage
    {
        Label login;
        Boolean loggedIn = false;
        String logText = "Login";
        PopupLayout _PopUpLayout;
     //   Label event_label;
        StackLayout mainContent;

        public VolunteerPortal()
        {
            //sets the background color based on settings
            if (Helpers.Settings.ContrastSetting == true)
            {
                BackgroundColor = Colors.contrastBg;
            }
            else
            {
                BackgroundColor = Colors.background;
            }


            Title = "Volunteer Portal";
            this.Padding = new Thickness(0, Device.OnPlatform(20, 0, 0), 0, 0);

            //================ Account Label and Account Settings  =========================================

            Label account = new Label
            {
                Text = "Account Settings",
                TextColor = Color.White,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.Start
            };
            //============================== Account used by user ================================
            Label accountType = new Label
            {
                Text = "Manage Account",
                FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center
            };
            //Edit Profile, Change Password, Login with different account, Create new account

            StackLayout row7_account = new StackLayout
            {
                Children = { accountType },
                GestureRecognizers = {
                new TapGestureRecognizer {
                     //   Command = new Command (()=>System.Diagnostics.Debug.WriteLine ("clicked")),
                     Command = new Command (()=>manageAccount()),
                },
                },
                Orientation = StackOrientation.Horizontal,
                Padding = new Thickness(5, 5)
            };

            //============================= Login ================================
            login = new Label
            {
                Text = logText,
                FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center
            };

            StackLayout row8_login = new StackLayout
            {
                Children = { login },
                GestureRecognizers = {
                new TapGestureRecognizer {
                        Command = new Command (
                            ()=>login_check()),
                },
                },
                Orientation = StackOrientation.Horizontal,
                Padding = new Thickness(5, 5)
            };



            mainContent = new StackLayout
            {
                Children = {     //
                    new BoxView() { Color = Color.Transparent, HeightRequest = 1  },
                    account,
                    new BoxView() { Color = Color.Gray, HeightRequest = 1, Opacity = 0.5  },
                    new BoxView() { Color = Color.Transparent, HeightRequest = 5  },

                    
                    row8_login,
                    new BoxView() { Color = Color.Gray, HeightRequest = 1, Opacity = 0.1  },
                    new BoxView() { Color = Color.Transparent, HeightRequest = 5  },

                    row7_account,
                    new BoxView() { Color = Color.Gray, HeightRequest = 1, Opacity = 0.1  },
                    new BoxView() { Color = Color.Transparent, HeightRequest = 5  },

                    //event_label
                },
                Padding = 10,
                VerticalOptions = LayoutOptions.FillAndExpand

            };

            ScrollView content = new ScrollView
            {
                Content = mainContent,
                Orientation = ScrollOrientation.Vertical
            };
            this.Content = content;

            login.FontSize = Device.GetNamedSize(NamedSize.Small, login);
            login.Style = new Style(typeof(Label))
            {
                BaseResourceKey = Device.Styles.BodyStyleKey,
                Setters = {
                new Setter { Property = Label.TextColorProperty,Value = Color.White },
                new Setter {Property = Label.FontFamilyProperty, Value = "Times New Roman" },

                }


            };

            if (Helpers.Settings.SpeechSetting == true)
            {
                CrossTextToSpeech.Current.Speak(login.Text);
            }
        }//end constructor VolunteerPortal
         //===========================================================================================================================

        //Displays options such as Edit Profile, Change Password, Login with Another Account, and Create a new account when Manage Account is tapped.
        async void manageAccount()
        {
            if (Helpers.Settings.SpeechSetting == true)
            {
                CrossTextToSpeech.Current.Speak("Manage Account");
            }
            var action = await DisplayActionSheet("Manage Account", "Cancel", null,
                //"Edit Profile", 
                "Change Password",
                "Login with Another Account", "Create a New Account");
            switch (action)
            {
                //case "Edit Profile":
                //    await Navigation.PushAsync(new ProfilePage());
                //    break;
                case "Change Password":
                    //var r = await UserDialogs.Instance.ConfirmAsync("", "Pick Title");
                    //var text = (r ? "Yes" : "No");
                    //this.Result($"Confirmation Choice: {text}");
                    customPopUp();
                    break;
                case "Login with Another Account":
                    var r = await UserDialogs.Instance.LoginAsync(new LoginConfig
                    {
                        Message = "Enter Credentials"
                    });
                    var status = r.Ok ? "Success" : "Cancelled";
                    //    this.Result("Login {status} - User Name: {r.LoginText} - Password: {r.Password}");
             //       event_label.Text = String.Format("Login With Another Account selected");

                    break;
                case "Create a New Account":
                    var p = await UserDialogs.Instance.LoginAsync(new LoginConfig
                    {
                        Message = "Create A New Account"
                    });
                    var created = p.Ok ? "Success" : "Cancelled";
                    //   this.Result("Login {created} - User Name: {p.LoginText} - Password: {p.Password}");

               //     event_label.Text = String.Format("Create a New Account selected");
                    break;
            }
        }

        //====================================== Customized pop-up for changing password ==========================

        StackLayout customPopUp()
        {
            _PopUpLayout = new PopupLayout();

            var oldPswd = new ExtendedEntryCell
            {

                Label = "Existing Password: ",
                Placeholder = "Password",
                IsPassword = true
                // LabelColor = Color.Black,
            };

            var newPswd = new ExtendedEntryCell
            {
                Label = "New Password: ",
                Placeholder = "Password",
                IsPassword = true
                // LabelColor = Color.Black
            };

            var confirmPswd = new ExtendedEntryCell()
            {
                Label = "Confirm Password: ",
                Placeholder = "Password",
                IsPassword = true,
            };


            StackLayout passwordPop = new StackLayout
            {
                Children = {
                            new TableView
                            {
                                Root = new TableRoot("Change Password")
                                {
                                    new TableSection("Change Password")
                                    {
                                        oldPswd,
                                        newPswd,
                                        confirmPswd,
                                    }

                                }

                            },//end tableView
                            new StackLayout()
                            {
                                HorizontalOptions = LayoutOptions.Center,
                                Orientation = StackOrientation.Horizontal,
                                Children = {
                                    new Button()
                                    {
                                        VerticalOptions = LayoutOptions.Start,
                                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                                        WidthRequest =140,
                                        Text = "Cancel",
                                        Command = new Command (()=> CancelPopup()),
                                    },
                                    new Button()
                                    {
                                        VerticalOptions = LayoutOptions.Start,
                                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                                        WidthRequest =140,
                                        Text = "OK",
                                        Command = new Command (()=> closePopUp(oldPswd,newPswd,confirmPswd)),
                                    }
                                }
                            }
                        },
                HorizontalOptions = LayoutOptions.StartAndExpand
            };

            _PopUpLayout.Content = mainContent;
            Content = _PopUpLayout;

            var PopUp = new StackLayout
            {
                WidthRequest = 300, // Important, the Popup has to have a size to be showed
                HeightRequest = 270,
                BackgroundColor = Color.FromHex("3f3f3f"), // for Android and WP
                Orientation = StackOrientation.Horizontal,
                Children = { passwordPop }//The StackLayout (all passwords)
            };

            _PopUpLayout.ShowPopup(PopUp);

            return passwordPop;

        }

        //===========================================================================================================================

        // Closes Pop Up when Cancel button is pressed
        void CancelPopup()
        {
            _PopUpLayout.DismissPopup();
            ScrollView content = new ScrollView
            {
                Content = mainContent,
                Orientation = ScrollOrientation.Vertical
            };
            this.Content = content;
        }

        //===========================================================================================================================

        //This function takes care of conditions:
        //1. If old password and new password are same, it would prompt the user again with error message
        //2. if new password and confirmation password are same, it would prompt the user again with error message


        void closePopUp(ExtendedEntryCell oldPswd, ExtendedEntryCell newPswd, ExtendedEntryCell confirmPswd)
        {
            StackLayout temp;


            if (_PopUpLayout.IsPopupActive)
            {
                String oldPass = oldPswd.Text;
                System.Diagnostics.Debug.WriteLine(oldPass);

                String newPass = newPswd.Text;
                System.Diagnostics.Debug.WriteLine(newPass);

                String confirmPass = confirmPswd.Text;
                System.Diagnostics.Debug.WriteLine(confirmPass);

                var existingPass = new Label
                {
                    Text = "Please pick a new password",
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    TextColor = Color.Red,
                    FontSize = 20
                };

                var mismatch = new Label
                {
                    Text = "Passwords don't match.",
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    TextColor = Color.Red,
                    FontSize = 20
                };

                _PopUpLayout.DismissPopup();
                ScrollView content = new ScrollView
                {
                    Content = mainContent,
                    Orientation = ScrollOrientation.Vertical
                };
                this.Content = content;

                if (newPass != confirmPass)
                {   //keep displaying pop up and display error message to user

                    temp = customPopUp();
                    temp.Children.Add(mismatch);

                    var PopUp = new StackLayout
                    {
                        WidthRequest = 300, // Important, the Popup has to have a size to be showed
                        HeightRequest = 320,
                        BackgroundColor = Color.FromHex("3f3f3f"), // for Android and WP
                        Orientation = StackOrientation.Horizontal,
                        Children = { temp },//The StackLayout (all passwords)
                        Padding = 5
                    };

                    _PopUpLayout.ShowPopup(PopUp);
                    System.Diagnostics.Debug.WriteLine("Passwords don't match");
                }
                else if (oldPass == newPass)
                {
                    temp = customPopUp();
                    temp.Children.Add(existingPass);

                    var PopUp = new StackLayout
                    {
                        WidthRequest = 300, // Important, the Popup has to have a size to be showed
                        HeightRequest = 320,
                        BackgroundColor = Color.FromHex("3f3f3f"), // for Android and WP
                        Orientation = StackOrientation.Horizontal,
                        Children = { temp },//The StackLayout (all passwords)
                        Padding = 5
                    };

                    _PopUpLayout.ShowPopup(PopUp);
                    System.Diagnostics.Debug.WriteLine("Please pick a new password");
                }
            }
            else //if no pop up is displayed
            {
                temp = customPopUp();
                _PopUpLayout.ShowPopup(temp);
            }
        }

        //===========================================================================================================================

        //This function checks if Login or Logout is pressed and prompts user accordingly.

        public async void login_check()
        {
            if (loggedIn == false) //login is currently displayed. set loggedIn to true to display logout
            {
                System.Diagnostics.Debug.WriteLine("Login currently displayed - loggedIn" + loggedIn);
                //entry pop up for login
                var r = await UserDialogs.Instance.LoginAsync(new LoginConfig
                {
                    Message = "Enter Credentials",
                    LoginPlaceholder = Helpers.Settings.EmailSetting,
                    PasswordPlaceholder = Helpers.Settings.PasswordSetting
                });

                var status = r.Ok ? "Success" : "Cancelled";
                //  System.Diagnostics.Debug.WriteLine("after status = r.ok?");

                // this.Result($"Login {status} - User Name: {r.LoginText} - Password: {r.Password}");
                if (status == "Success")
                {
                    Helpers.Settings.EmailSetting = r.LoginText;
                    Helpers.Settings.PasswordSetting = r.Password;
                    loggedIn = true;
                    login.Text = "Logout";
                    System.Diagnostics.Debug.WriteLine("status is success then logout should be displayed - loggedIn" + loggedIn);
                    return;
                }
                else if (status == "Cancelled")
                {
                    // loggedIn = false;
                    login.Text = "Login";
                    System.Diagnostics.Debug.WriteLine("status is cancelled then login should be displayed - loggedIn" + loggedIn);
                    return;
                }
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
             //       event_label.Text = String.Format("Logout canceled");
                    System.Diagnostics.Debug.WriteLine("User clicked on No, logout should still be displayed - loggedIn" + loggedIn);
                    return;
                }
                else
                {
                    loggedIn = false;
                    login.Text = "Login";
             //       event_label.Text = String.Format("Logged out");
                    System.Diagnostics.Debug.WriteLine("User clicked on Yes, login should be displayed - loggedIn" + loggedIn);
                    return;
                }
            }

        }

        //===========================================================================================================================
        //temporary function

        void Result(string msg)
        {
            UserDialogs.Instance.Alert(msg);
        }

        //==================================================== Back Button Pressed ==============================================================

        protected override Boolean OnBackButtonPressed() // back button pressed
        {
            ((RootPage)App.Current.MainPage).NavigateTo();
            return true;
        }
    }

}
