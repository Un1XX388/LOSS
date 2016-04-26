using Xamarin.Forms;
using XLabs.Forms.Controls;
using Plugin.TextToSpeech;
using XLabs;
using System;
using Acr.UserDialogs;
using Newtonsoft.Json;
using Amazon.Lambda.Model;
using Amazon.Util;
using Amazon.Lambda;
using System.IO;
using System.Text.RegularExpressions;

namespace LOSSPortable
{
    public class RegisteredAccountPage : ContentPage
    {
        Entry username;
        Label email;
        StackLayout mainContent;
        Label event_label;
        Switch contrast_switcher;
        //     Switch geolocation_switcher;
        //        Switch anonymous_switcher;
        Switch speech_switcher;
        Switch push_switcher;
        PopupLayout _PopUpLayout;


        public RegisteredAccountPage()
        {
            if (Helpers.Settings.ContrastSetting == true)
            {
                BackgroundColor = Colors.contrastBg;
            }
            else
            {
                BackgroundColor = Colors.background;
            }
            this.Padding = new Thickness(0, Device.OnPlatform(20, 0, 0), 0, 0);

            Image edit = new Image
            {
                Source = Device.OnPlatform(
                        iOS: ImageSource.FromFile("edituser.png"),
                        Android: ImageSource.FromFile("edituser.png"),
                        WinPhone: ImageSource.FromFile("edituser.png")),
                HorizontalOptions = LayoutOptions.End
            };

            username = new Entry { Text = Helpers.Settings.UsernameSetting, BackgroundColor = Color.White, TextColor = Color.Black, IsEnabled = false, HorizontalOptions = LayoutOptions.StartAndExpand };
            StackLayout user = new StackLayout { Children = { username, edit } };
            email = new Label { Text = "Email:  " + Helpers.Settings.EmailSetting, TextColor = Color.White, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label))};

            event_label = new Label
            {
                Text = "Switch is now False",
                FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            Label generalSettings = new Label
            {
                Text = "Settings",
                TextColor = Color.White,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.Start
            };

            Label contrast_label = new Label
            {
                Text = "High Contrast",
                TextColor = Color.White,
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

            //=============Speech Switch=====================================
            Label speech_label = new Label
            {
                Text = "Text-to-Speech",
                TextColor = Color.White,
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
                Children = { speech_stack, speech_switcher_stack },
                Orientation = StackOrientation.Horizontal,
                Padding = new Thickness(5, 5)
            };

            //=============Push Notification Switch=====================================
            Label push_label = new Label
            {
                Text = "Notifications",
                TextColor = Color.White,
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
                Spacing = 5
            };



            //================ Account Label and Account Settings  =========================================

            Label account = new Label
            {
                Text = "Account Settings",
                TextColor = Color.White,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.Start
            };



            //============================= Login ================================
            Label changePassword = new Label
            {
                Text = "Change Password",
                TextColor = Color.White,
                FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center
            };

            StackLayout change_password = new StackLayout
            {
                Children = { changePassword },
                GestureRecognizers = {
                new TapGestureRecognizer {
                        Command = new Command (
                            ()=>customPopUp()),
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
                TextColor = Color.White,
                FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center
            };

            StackLayout row9_report = new StackLayout
            {
                Children = { reportLink },
                GestureRecognizers = {
                new TapGestureRecognizer {
                        Command = new Command (()=> reportProblem()),
                },
                },
                Orientation = StackOrientation.Horizontal,
                Padding = new Thickness(5, 5)
            };

            Button logout = new Button
            {
                Text = "LOGOUT",
                TextColor = Colors.barBackground,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = Color.FromHex("ffffe6"),
                HeightRequest = 40,
                WidthRequest = 300
            };
            logout.Clicked += Logout_Clicked;

            //========== Page Content where everything needs to be inserted=============================================

            mainContent = new StackLayout
            {

                Children = { new BoxView() { Color = Color.Transparent, HeightRequest = 4  },
                            username,
                            new BoxView() { Color = Color.Gray, HeightRequest = 1, Opacity = 0.5  },
                            email,
                            new BoxView() { Color = Color.Gray, HeightRequest = 1, Opacity = 0.5  },
                            new BoxView() { Color = Color.Transparent, HeightRequest = 5  },
                            generalSettings,
                            new BoxView() { Color = Color.Gray, HeightRequest = 1, Opacity = 0.5  },
                            new BoxView() { Color = Color.Transparent, HeightRequest = 5  },
                            row1_contrast, //row2_geolocation,
                            row4_speech, row5_push,
                            change_password,
             
                            row6_reset_sync,

                            new BoxView() { Color = Color.Transparent, HeightRequest = 1  },
                            report,
                            new BoxView() { Color = Color.Gray, HeightRequest = 1, Opacity = 0.5 },
                            new BoxView() { Color = Color.Transparent, HeightRequest = 5  },

                            row9_report,
                            new BoxView() { Color = Color.Gray, HeightRequest = 1, Opacity = 0.1  },
                         //   new BoxView() { Color = Color.Transparent, HeightRequest = 5  },
                            logout

                        //    event_label
                        },
                Padding = new Thickness(5, 5, 5, 5),
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            ScrollView content = new ScrollView
            {
                Content = mainContent,
                Orientation = ScrollOrientation.Vertical
            };
            this.Content = content;

        }//end GeneralAccountPage()


        private void reportProblem()
        {
            if (Helpers.Settings.SpeechSetting == true)
            {
                CrossTextToSpeech.Current.Speak("Report A Problem");
            }
            Navigation.PushAsync(new ReportPage());
        }

        private async void Logout_Clicked(object sender, EventArgs e)
        {

            if (Helpers.Settings.SpeechSetting == true)
            {
                CrossTextToSpeech.Current.Speak("Logout");
            }
            try
            {
                System.Diagnostics.Debug.WriteLine(Helpers.Settings.EndpointArnSetting);
                UserItem user = new UserItem { Item = new UserLogin { Arn = Helpers.Settings.EndpointArnSetting} };
                MessageJson messageJson = new MessageJson { operation = "logout", tableName = "User", payload = user };
                string args = JsonConvert.SerializeObject(messageJson);
                //System.Diagnostics.Debug.WriteLine(args);
                var ir = new InvokeRequest()
                {
                    FunctionName = "arn:aws:lambda:us-east-1:987221224788:function:Test_Backend",
                    PayloadStream = AWSSDKUtils.GenerateMemoryStreamFromString(args),
                    InvocationType = InvocationType.RequestResponse
                };
                //System.Diagnostics.Debug.WriteLine("Before invoke: " + ir.ToString());


                InvokeResponse resp = await AmazonUtils.LambdaClient.InvokeAsync(ir);
                resp.Payload.Position = 0;
                var sr = new StreamReader(resp.Payload);
                var myStr = sr.ReadToEnd();

                System.Diagnostics.Debug.WriteLine("Status code: " + resp.StatusCode);
                System.Diagnostics.Debug.WriteLine("Response content: " + myStr);

                response tmp = JsonConvert.DeserializeObject<response>(myStr);
                if (tmp.Success == "true")
                {
                    Helpers.Settings.LoginSetting = false;
                    UserDialogs.Instance.ShowSuccess("You have been successfully logged out.");
                    ((RootPage)App.Current.MainPage).NavigateTo();
                }
                else
                {
                    UserDialogs.Instance.ShowError("Unable to log out.");
                }
            }
            catch(Exception e2)
            {
                System.Diagnostics.Debug.WriteLine("Error:" + e2);
            }

        }
        //================ Functions (actions) for Each Toggle Switch   =========================================

        //default setting
        async void resetPressed(object sender, EventArgs e)
        {
            if(Helpers.Settings.SpeechSetting == true)
            {
                CrossTextToSpeech.Current.Speak("Reset to Default Settings?");
            }
            var result = await DisplayAlert("Reset application", "Reset app to default settings and delete any cached info?", "Yes", "No");

            if (result)
            {
                event_label.Text = String.Format("Settings have been reset");
                contrast_switcher.IsToggled = Helpers.Settings.ContrastSetting = false;
                // geolocation_switcher.IsToggled = Helpers.Settings.AnonymousSetting = true;
                //      anonymous_switcher.IsToggled = Helpers.Settings.AnonymousSetting = false;
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
                event_label.Text = String.Format("High Contrast Mode Enabled?", e.Value);
                Helpers.Settings.ContrastSetting = e.Value;

            }

            if (Helpers.Settings.SpeechSetting == true && Helpers.Settings.ContrastSetting == true)
            {
                CrossTextToSpeech.Current.Speak("Contrast Mode On");
            }
            else if(Helpers.Settings.SpeechSetting == true && Helpers.Settings.ContrastSetting == false)
            {
                CrossTextToSpeech.Current.Speak("Contrast Mode Off");
            }
        }

        //void geolocation_switcher_Toggled(object sender, ToggledEventArgs e)
        //{
        //    event_label.Text = String.Format("Geolocation enabled? {0}", e.Value);
        //    Helpers.Settings.locationSetting = e.Value;
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
                CrossTextToSpeech.Current.Speak("Notification Mode On");
            }
        }

        StackLayout customPopUp()
        {
            _PopUpLayout = new PopupLayout();

            var email = new ExtendedEntryCell
            {
                Label = "Email: ",
                Placeholder = "Email"
                // LabelColor = Color.Black
            };
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
                                        email,
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
                                        Command = new Command (()=> closePopUp(email,oldPswd,newPswd,confirmPswd)),
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
                HeightRequest = 320,
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


        void closePopUp(ExtendedEntryCell email, ExtendedEntryCell oldPswd, ExtendedEntryCell newPswd, ExtendedEntryCell confirmPswd)
        {
            StackLayout temp;

            String email_id = email.Text.TrimEnd();

            String oldPass = oldPswd.Text;
            System.Diagnostics.Debug.WriteLine(oldPass);

            String newPass = newPswd.Text;
            System.Diagnostics.Debug.WriteLine(newPass);

            String confirmPass = confirmPswd.Text;
            System.Diagnostics.Debug.WriteLine(confirmPass);

            if (_PopUpLayout.IsPopupActive)
            {


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

                var no_email = new Label
                {
                    Text = "Please enter your e-mail address.",
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    TextColor = Color.Red,
                    FontSize = 20
                };

                var invalid_email = new Label
                {
                    Text = "Please enter a valid e-mail address.",
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

                if (email_id == null)
                {
                    temp = customPopUp();
                    // temp.Children.Add(no_email);

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
                    System.Diagnostics.Debug.WriteLine("Please enter your e-mail ID.");

                }
                else if (!(Regex.Match(email_id, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").Success))
                {
                    //Not Valid email    
                    temp = customPopUp();
                  //  UserDialogs.Instance.ShowError("Invalid Email");

                    temp.Children.Add(invalid_email);

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
                    System.Diagnostics.Debug.WriteLine("Invalid email");

                }
                else if (newPass != confirmPass)
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
                else
                {
                    updatePassword(email_id, oldPass, newPass);
                    UserDialogs.Instance.ShowSuccess("Password has been changed successfully.");
                }//update password
            }
            else //if no pop up is displayed
            {
                temp = customPopUp();
                _PopUpLayout.ShowPopup(temp);
            }
        }

        //==================================================== update password on server ==============================================================

        public async void updatePassword(String email, String oldPass, String newPass)
        {
            if(Helpers.Settings.SpeechSetting == true)
            {
                CrossTextToSpeech.Current.Speak("Change Password");
            }

            try
            {
                UserItem user = new UserItem { Item = new UserLogin { Email = email, Password = oldPass.TrimEnd(), NewPassword = newPass, Arn = "" + Helpers.Settings.EndpointArnSetting } };
                MessageJson messageJson = new MessageJson { operation = "update", tableName = "User", payload = user };
                string args = JsonConvert.SerializeObject(messageJson);

                //System.Diagnostics.Debug.WriteLine(args);
                var ir = new InvokeRequest()
                {
                    FunctionName = "arn:aws:lambda:us-east-1:987221224788:function:Test_Backend",
                    PayloadStream = AWSSDKUtils.GenerateMemoryStreamFromString(args),
                    InvocationType = InvocationType.RequestResponse
                };
                //System.Diagnostics.Debug.WriteLine("Before invoke: " + ir.ToString());

                InvokeResponse resp = await AmazonUtils.LambdaClient.InvokeAsync(ir);
                resp.Payload.Position = 0;
                var sr = new StreamReader(resp.Payload);
                var myStr = sr.ReadToEnd();

                //                System.Diagnostics.Debug.WriteLine("Status code: " + resp.StatusCode);
                //                System.Diagnostics.Debug.WriteLine("Response content: " + myStr);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Error:" + e);
            }
        }
        //==================================================== Back Button Pressed ==============================================================

        protected override Boolean OnBackButtonPressed() // back button pressed
        {
            ((RootPage)App.Current.MainPage).NavigateTo();
            return true;
        }

    }
}