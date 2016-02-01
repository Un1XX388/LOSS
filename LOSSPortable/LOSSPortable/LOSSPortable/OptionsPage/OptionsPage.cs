using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

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

        public OptionsPage()
        {
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


            event_label = new Label{
                Text = "Switch is now False",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand};

            Label contrast_label = new Label{
                Text = "High Contrast",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center};

            contrast_switcher = new Switch {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                IsToggled = false};
            contrast_switcher.Toggled += constrast_switcher_Toggled;

            StackLayout header_stack = new StackLayout{
                Children = {contrast_label},
                HorizontalOptions = LayoutOptions.Start};

            StackLayout switcher_stack = new StackLayout{
                Children = {contrast_switcher},
                HorizontalOptions = LayoutOptions.EndAndExpand};

            StackLayout row1_contrast = new StackLayout{
                Children = {header_stack, switcher_stack},
                Orientation = StackOrientation.Horizontal,
                Padding = new Thickness(5, 5)};

            //=============GEOLOCATION ROW=====================================
            Label geolocation_label = new Label
            {
                Text = "Geolocation",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center
            };

            geolocation_switcher = new Switch
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                IsToggled = true
            };
            geolocation_switcher.Toggled += geolocation_switcher_Toggled;

            StackLayout geolocation_stack = new StackLayout
            {
                Children = { geolocation_label },
                HorizontalOptions = LayoutOptions.Start
            };

            StackLayout geolocation_switcher_stack = new StackLayout
            {
                Children = { geolocation_switcher },
                HorizontalOptions = LayoutOptions.EndAndExpand
            };

            StackLayout row2_geolocation = new StackLayout
            {
                Children = { geolocation_stack, geolocation_switcher_stack},
                Orientation = StackOrientation.Horizontal,
                Padding = new Thickness(5, 5)
            };

            //=============Anonymous Switch=====================================
            Label anonymous_label = new Label
            {
                Text = "Anonymous Mode",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center
            };

            anonymous_switcher = new Switch
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                IsToggled = false
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

            StackLayout row3_anonymous= new StackLayout
            {
                Children = { anonymous_stack, anonymous_switcher_stack},
                Orientation = StackOrientation.Horizontal,
                Padding = new Thickness(5, 5)
            };


            //=============Speech Switch=====================================
            Label speech_label = new Label
            {
                Text = "Text-to-Speech",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center
            };

            speech_switcher = new Switch
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                IsToggled = false
            };
            speech_switcher.Toggled += speech_switcher_Toggled;

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
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center
            };

            push_switcher = new Switch
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                IsToggled = true
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

            //================ Account Label, Reset and Sync buttons =========================================

            Label account = new Label
            {
                Text = "Account Settings",
                TextColor = Color.White,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.Start
            };

            Button accounts = new Button
            {
                Text = "ACCOUNTS",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Button)),
                TextColor = Color.White,
                WidthRequest = 150
            };
            accounts.Clicked += accountsPressed;

            Button reset = new Button
            {
                Text = "RESET",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Button)),
                TextColor = Color.White,
                WidthRequest = 150
            };
            reset.Clicked += resetPressed;

            StackLayout row6_reset_sync = new StackLayout
            {
                Children = { accounts, reset },
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Spacing = 10
            };



            //========== Page Content where everything needs to be inserted=============================================

             
            mainContent = new StackLayout
            {
                Children = { new BoxView() { Color = Color.Transparent, HeightRequest = 4  },
                    row1_contrast, row2_geolocation, row3_anonymous, row4_speech, row5_push, 
                    new BoxView() { Color = Color.Transparent, HeightRequest = 8  },
                    account,
                    new BoxView() { Color = Color.Gray, HeightRequest = 1, Opacity = 0.5  },
                    new BoxView() { Color = Color.Transparent, HeightRequest = 3  },
                    row6_reset_sync, event_label }
                    , Padding = new Thickness(5,5,5,5)
            };
            this.Content = mainContent;
        }

        async void accountsPressed(object sender, EventArgs e)
        {
            var result = await DisplayActionSheet("Account Options", "Cancel", null, "Create Account", "Login", "Logout", "Switch Accounts");
            event_label.Text = String.Format("Sync option chosen: {0}", result);
        }

        async void resetPressed(object sender, EventArgs e)
        {
            var result = await DisplayAlert("Reset application", "Reset app to default settings and delete any cached info?", "Yes", "No");
            
            if (result)
            {
                event_label.Text = String.Format("Settings have been reset");
                contrast_switcher.IsToggled = false;
                geolocation_switcher.IsToggled = true;
                anonymous_switcher.IsToggled = false;
                speech_switcher.IsToggled = false;
                push_switcher.IsToggled = true;
            }
            else
            {
                event_label.Text = String.Format("Reset canceled");
            }
        }

        void constrast_switcher_Toggled(object sender, ToggledEventArgs e)
        {
            if (contrast_switcher.IsToggled)
            {
                this.Content.BackgroundColor = Color.Black;
                event_label.Text = String.Format("High Contrast Mode Enabled? {0}", e.Value);
            }
            else
            {
                this.Content.BackgroundColor = Color.Transparent;
                event_label.Text = String.Format("High Contrast Mode Enabled? {0}", e.Value);
            }
            
        }

        void geolocation_switcher_Toggled(object sender, ToggledEventArgs e)
        {
            event_label.Text = String.Format("Geolocation enabled? {0}", e.Value);
        }

        void anonymous_switcher_Toggled(object sender, ToggledEventArgs e)
        {
            event_label.Text = String.Format("Anonymity enabled? {0}", e.Value);
        }

        void speech_switcher_Toggled(object sender, ToggledEventArgs e)
        {
            event_label.Text = String.Format("Text-to-Speech enabled? {0}", e.Value);
        }

        void push_switcher_Toggled(object sender, ToggledEventArgs e)
        {
            event_label.Text = String.Format("Notifications enabled? {0}", e.Value);
        }
    }
}
