using Xamarin.Forms;
using XLabs.Forms.Controls;
using Plugin.TextToSpeech;
using XLabs;
using System;

namespace LOSSPortable
{
    public class GeneralAccountPage : ContentPage
    {
        StackLayout mainContent;
        Label event_label;
        Switch contrast_switcher;
        //     Switch geolocation_switcher;
        //        Switch anonymous_switcher;
        Switch speech_switcher;
        Switch push_switcher;

        public GeneralAccountPage()
        {
            if(Helpers.Settings.ContrastSetting == true)
            {
                BackgroundColor = Colors.contrastBg;
            }
            else
            {
                BackgroundColor = Colors.background;
            }

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
            Label login = new Label
            {
                Text = "Login/Register Portal",
                TextColor = Color.White,
                FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center
            };

            StackLayout row8_login = new StackLayout
            {
                Children = { login },
                GestureRecognizers = {
                new TapGestureRecognizer {
                        Command = new Command (
                            ()=>Navigation.PushAsync(new LoginPage())),
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
                        Command = new Command (()=>Navigation.PushAsync(new ReportPage())),
                },
                },
                Orientation = StackOrientation.Horizontal,
                Padding = new Thickness(5, 5)
            };

            //========== Page Content where everything needs to be inserted=============================================

            mainContent = new StackLayout
            {

                Children = { new BoxView() { Color = Color.Transparent, HeightRequest = 4  },
                            generalSettings,
                            new BoxView() { Color = Color.Gray, HeightRequest = 1, Opacity = 0.5  },
                            new BoxView() { Color = Color.Transparent, HeightRequest = 5  },
                            row1_contrast, //row2_geolocation,
                            row4_speech, row5_push, row6_reset_sync,

                            new BoxView() { Color = Color.Transparent, HeightRequest = 1  },
                            account,
                            new BoxView() { Color = Color.Gray, HeightRequest = 1, Opacity = 0.5  },
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
                event_label.Text = String.Format("High Contrast Mode Enabled? {0}", e.Value);
                Helpers.Settings.ContrastSetting = e.Value;

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
                CrossTextToSpeech.Current.Speak("Notification Mode" + e.Value);
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