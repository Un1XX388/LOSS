﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Acr.UserDialogs;
using XLabs.Forms.Controls;
using Plugin.TextToSpeech;

namespace LOSSPortable
{
    public class OptionsPage : ContentPage
    {
        StackLayout mainContent;
        Label event_label;
        Switch contrast_switcher;
        Switch geolocation_switcher;
        Switch anonymous_switcher;
        Switch speech_switcher;
        Switch push_switcher;
        Boolean loggedIn = false;
        Boolean result;
        Label login;
        String logText = "Login";
        PopupLayout _PopUpLayout;


        public OptionsPage()
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


            Title = "Options";
            this.Padding = new Thickness(0, Device.OnPlatform(20, 0, 0), 0, 0);
            /*
             * //Anonymous switch
             * //Notification settings
             * //Display name ( accounts)
             * Sync or reset information
             * Account info (link to creation/switch)
             * //Text-to-Speech
             * //geolocation toggle
             */

            event_label = new Label
            {
                Text = "Switch is now False",
                FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            Label generalSettings = new Label
            {
                Text = "General Settings",
                TextColor = Color.White,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.Start
            };

            Label contrast_label = new Label
            {
                Text = "High Contrast",
                FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center
            };

            contrast_switcher = new Switch
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                IsToggled = Helpers.Settings.ContrastSetting
            };
            contrast_switcher.Toggled += constrast_switcher_Toggled;

            StackLayout header_stack = new StackLayout
            {
                Children = { contrast_label },
                HorizontalOptions = LayoutOptions.Start
            };

            StackLayout switcher_stack = new StackLayout
            {
                Children = { contrast_switcher },
                HorizontalOptions = LayoutOptions.EndAndExpand
            };

            StackLayout row1_contrast = new StackLayout
            {
                Children = { header_stack, switcher_stack },
                Orientation = StackOrientation.Horizontal,
                Padding = new Thickness(5, 5)
            };

            //=============GEOLOCATION ROW=====================================
            //Label geolocation_label = new Label
            //{
            //    Text = "Geolocation",
            //    FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
            //    HorizontalOptions = LayoutOptions.Center
            //};

            //geolocation_switcher = new Switch
            //{
            //    HorizontalOptions = LayoutOptions.Center,
            //    VerticalOptions = LayoutOptions.CenterAndExpand,
            //    IsToggled = Helpers.Settings.locationSetting
            //};
            //geolocation_switcher.Toggled += geolocation_switcher_Toggled;

            //StackLayout geolocation_stack = new StackLayout
            //{
            //    Children = { geolocation_label },
            //    HorizontalOptions = LayoutOptions.Start
            //};

            //StackLayout geolocation_switcher_stack = new StackLayout
            //{
            //    Children = { geolocation_switcher },
            //    HorizontalOptions = LayoutOptions.EndAndExpand
            //};

            //StackLayout row2_geolocation = new StackLayout
            //{
            //    Children = { geolocation_stack, geolocation_switcher_stack },
            //    Orientation = StackOrientation.Horizontal,
            //    Padding = new Thickness(5, 5)
            //};

            //=============Anonymous Switch=====================================
            Label anonymous_label = new Label
            {
                Text = "Anonymous Mode",
                FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center
            };

            anonymous_switcher = new Switch
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                IsToggled = Helpers.Settings.AnonymousSetting

            };
            anonymous_switcher.Toggled += anonymous_switcher_Toggled;

            StackLayout anonymous_stack = new StackLayout
            {
                Children = { anonymous_label },
                HorizontalOptions = LayoutOptions.Start
            };

            StackLayout anonymous_switcher_stack = new StackLayout
            {
                Children = { anonymous_switcher },
                HorizontalOptions = LayoutOptions.EndAndExpand
            };

            StackLayout row3_anonymous = new StackLayout
            {
                Children = { anonymous_stack, anonymous_switcher_stack },
                Orientation = StackOrientation.Horizontal,
                Padding = new Thickness(5, 5)
            };


            //=============Speech Switch=====================================
            Label speech_label = new Label
            {
                Text = "Text-to-Speech",
                FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center
            };

            speech_switcher = new Switch
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                IsToggled = Helpers.Settings.SpeechSetting
            };
            speech_switcher.Toggled += speech_switcher_Toggled;

            // Speech speech = new Speech();

            StackLayout speech_stack = new StackLayout
            {
                Children = { speech_label },
                HorizontalOptions = LayoutOptions.Start
            };

            StackLayout speech_switcher_stack = new StackLayout
            {
                Children = { speech_switcher },
                HorizontalOptions = LayoutOptions.EndAndExpand
            };

            StackLayout row4_speech = new StackLayout
            {
                Children = { speech_stack, speech_switcher_stack},
                Orientation = StackOrientation.Horizontal,
                Padding = new Thickness(5, 5)
            };

            //=============Push Notification Switch=====================================
            Label push_label = new Label
            {
                Text = "Notifications",
                FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center
            };

            push_switcher = new Switch
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                IsToggled = Helpers.Settings.PushSetting
            };
            push_switcher.Toggled += push_switcher_Toggled;

            StackLayout push_stack = new StackLayout
            {
                Children = { push_label },
                HorizontalOptions = LayoutOptions.Start
            };

            StackLayout push_switcher_stack = new StackLayout
            {
                Children = { push_switcher },
                HorizontalOptions = LayoutOptions.EndAndExpand
            };

            StackLayout row5_push = new StackLayout
            {
                Children = { push_stack, push_switcher_stack },
                Orientation = StackOrientation.Horizontal,
                Padding = new Thickness(5, 5)
            };

            //================ Reset button  =========================================

            Button reset = new Button
            {
                Text = "Reset to Default",
                FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Button)),
                TextColor = Color.White,
                WidthRequest = 150
            };
            reset.Clicked += resetPressed;

            StackLayout row6_reset_sync = new StackLayout
            {
                Children = { reset },
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.EndAndExpand,
                Spacing = 10
            };



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

            //================ Support Settings - Reporting Problem  =========================================

            Label report = new Label
            {
                Text = "Support",
                TextColor = Color.White,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.Start
            };

            Label reportLink = new Label
            {
                Text = "Report A Problem",
                FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center
            };

            StackLayout row9_report = new StackLayout
            {
                Children = { reportLink },
                GestureRecognizers = {
                new TapGestureRecognizer {
                        Command = new Command (()=>Navigation.PushAsync(new ReportPage())),
                },
                },
                Orientation = StackOrientation.Horizontal,
                Padding = new Thickness(5, 5)
            };

            //============================ About Us  =========================================

            Label contact = new Label
            {
                Text = "LOSS App",
                TextColor = Color.White,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.Start
            };

            Label aboutUs = new Label
            {
                Text = "About Us",
                FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center
            };

            StackLayout row10_about = new StackLayout
            {
                Children = { aboutUs },
                GestureRecognizers = {
                
                new TapGestureRecognizer {
                   //     Command = new Command (()=>System.Diagnostics.Debug.WriteLine ("clicked")),
                        Command = new Command (()=>Navigation.PushAsync(new AboutPage())),
                },
                },
                Orientation = StackOrientation.Horizontal,
                Padding = new Thickness(5, 5)
            };


            //========== Page Content where everything needs to be inserted=============================================
            //if (count == 2)
            //{
            //    alternate = row8_login;
            //}

          //  Result("location on options page is"+geolocation_switcher.IsToggled);

            mainContent = new StackLayout
            {

                Children = { new BoxView() { Color = Color.Transparent, HeightRequest = 4  },
                    generalSettings,
                    new BoxView() { Color = Color.Gray, HeightRequest = 1, Opacity = 0.5  },
                    new BoxView() { Color = Color.Transparent, HeightRequest = 5  },
                    row1_contrast, //row2_geolocation,
                  //  row3_anonymous,
                    row4_speech, row5_push, row6_reset_sync,
                    //
                    new BoxView() { Color = Color.Transparent, HeightRequest = 1  },
                    account,
                    new BoxView() { Color = Color.Gray, HeightRequest = 1, Opacity = 0.5  },
                    new BoxView() { Color = Color.Transparent, HeightRequest = 5  },

                    row7_account,
                    new BoxView() { Color = Color.Gray, HeightRequest = 1, Opacity = 0.1  },
                    new BoxView() { Color = Color.Transparent, HeightRequest = 5  },

                    row8_login,
                    new BoxView() { Color = Color.Gray, HeightRequest = 1, Opacity = 0.1  },
                    new BoxView() { Color = Color.Transparent, HeightRequest = 5  },

                    new BoxView() { Color = Color.Transparent, HeightRequest = 1  },
                    report,
                    new BoxView() { Color = Color.Gray, HeightRequest = 1, Opacity = 0.5 },
                    new BoxView() { Color = Color.Transparent, HeightRequest = 5  },

                    row9_report,
                    new BoxView() { Color = Color.Gray, HeightRequest = 1, Opacity = 0.1  },
                    new BoxView() { Color = Color.Transparent, HeightRequest = 5  },

                    new BoxView() { Color = Color.Transparent, HeightRequest = 1  },
                    contact,
                    new BoxView() { Color = Color.Gray, HeightRequest = 1, Opacity = 0.5  },
                    new BoxView() { Color = Color.Transparent, HeightRequest = 5  },
                    row10_about,
                    new BoxView() { Color = Color.Gray, HeightRequest = 1, Opacity = 0.1  },
                    new BoxView() { Color = Color.Transparent, HeightRequest = 5  },

                    event_label },
                Padding = new Thickness(5, 5, 5, 5),
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            ScrollView content = new ScrollView
            {
                Content = mainContent,
                Orientation = ScrollOrientation.Vertical
            };
            this.Content = content;
        }

        //================ Functions (actions) for Each Toggle Switch   =========================================

        //default setting
        async void resetPressed(object sender, EventArgs e)
        {
            var result = await DisplayAlert("Reset application", "Reset app to default settings and delete any cached info?", "Yes", "No");

            if (result)
            {
                event_label.Text = String.Format("Settings have been reset");
                contrast_switcher.IsToggled = Helpers.Settings.ContrastSetting = false;
               // geolocation_switcher.IsToggled = Helpers.Settings.AnonymousSetting = true;
                anonymous_switcher.IsToggled = Helpers.Settings.AnonymousSetting = false;
                speech_switcher.IsToggled = Helpers.Settings.SpeechSetting = false;
                push_switcher.IsToggled = Helpers.Settings.PushSetting = true;
            }
            else
            {
                event_label.Text = String.Format("Reset canceled");
            }
        }

        //change background color - contrast
        void constrast_switcher_Toggled(object sender, ToggledEventArgs e)
        {

            if (contrast_switcher.IsToggled)
            {
                this.Content.BackgroundColor = Colors.contrastBg;
                event_label.Text = String.Format("High Contrast Mode Enabled? {0}", e.Value);
                Helpers.Settings.ContrastSetting = e.Value;

            }
            else
            {
                this.Content.BackgroundColor = Colors.background;
                event_label.Text = String.Format("High Contrast Mode Enabled? {0}", e.Value);
                Helpers.Settings.ContrastSetting = e.Value;

            }

        }


        //void geolocation_switcher_Toggled(object sender, ToggledEventArgs e)
        //{
        //    event_label.Text = String.Format("Geolocation enabled? {0}", e.Value);
        //    Helpers.Settings.locationSetting = e.Value;

        //}

        //Anonymous mode on and off
        //void anonymous_switcher_Toggled(object sender, ToggledEventArgs e)
        //{

        //    event_label.Text = String.Format("Anonymity enabled? {0}", e.Value);
        //    Helpers.Settings.AnonymousSetting = e.Value;
        //    if (Helpers.Settings.SpeechSetting == true)
        //    {
        //        CrossTextToSpeech.Current.Speak("Anonymous Mode?" + e.Value);
        //    }

        //}

        //Text-to-Speech Implementation
        void speech_switcher_Toggled(object sender, ToggledEventArgs e)
        {
            if (speech_switcher.IsToggled)
            {
                //var text = "Text to speech.";
                event_label.Text = String.Format("Text to Speech? {0}", e.Value);
                CrossTextToSpeech.Current.Speak("Text to Speech Mode On");
                Helpers.Settings.SpeechSetting = e.Value;

            }
            else
            {
                event_label.Text = String.Format("Text to Speech? {0}", e.Value);
                Helpers.Settings.SpeechSetting = e.Value;
            }
        }

        //Notifications On-Off
        void push_switcher_Toggled(object sender, ToggledEventArgs e)
        {
            event_label.Text = String.Format("Notifications enabled? {0}", e.Value);
            Helpers.Settings.PushSetting = e.Value;
            if (Helpers.Settings.SpeechSetting == true)
            {
                CrossTextToSpeech.Current.Speak("Notification Mode" + e.Value);
            }

        }

        //===========================================================================================================================

        public void defaultSetting()
        {
            Helpers.Settings.ContrastSetting = contrast_switcher.IsToggled = false;
            Helpers.Settings.AnonymousSetting = anonymous_switcher.IsToggled = false;
            Helpers.Settings.SpeechSetting = speech_switcher.IsToggled = false;
            Helpers.Settings.PushSetting = push_switcher.IsToggled = true;
        }

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
                    this.Result("Login {status} - User Name: {r.LoginText} - Password: {r.Password}");
                    event_label.Text = String.Format("Login With Another Account selected");

                    break;
                case "Create a New Account":
                    var p = await UserDialogs.Instance.LoginAsync(new LoginConfig
                    {
                        Message = "Create A New Account"
                    });
                    var created = p.Ok ? "Success" : "Cancelled";
                    this.Result("Login {created} - User Name: {p.LoginText} - Password: {p.Password}");
                
                    event_label.Text = String.Format("Create a New Account selected");
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
                result = await DisplayAlert("Log Out", "Are you sure?", "Yes", "No");
                if (result == false)
                {
                    // loggedIn = true;
                    login.Text = "Logout";
                    event_label.Text = String.Format("Logout canceled");
                    System.Diagnostics.Debug.WriteLine("User clicked on No, logout should still be displayed - loggedIn" + loggedIn);
                    return;
                }
                else
                {
                    loggedIn = false;
                    login.Text = "Login";
                    event_label.Text = String.Format("Logged out");
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

<<<<<<< HEAD
=======
        //===========================================================================================================================
        //returns Login Label needed to be displayed
        String Login()
        {
            login_check();
            return logText;

        }
        protected override Boolean OnBackButtonPressed() // back button pressed
        {
            ((RootPage)App.Current.MainPage).NavigateTo();
            return true;
        }
>>>>>>> 1c25b2906534bed115b4e7b3070a1b5a1ddc0201
    }
    
}
