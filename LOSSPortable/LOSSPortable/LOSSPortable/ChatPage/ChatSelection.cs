using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Threading.Tasks;
using Akavache;
using System.Reactive.Linq;
using System.Threading;





namespace LOSSPortable
{


    public class ChatSelection : ContentPage
    {

        
      
        private StackLayout stackLayout;
        private StackLayout outerLayout;
        private ScrollView innerScroll;
        Entry nameEntry;



        Switch readyToChat;
        Label chatAvailability = new Label { Text = "Not Ready to chat.", HorizontalTextAlignment = TextAlignment.Center, FontSize = 20, FontFamily = "Arial" };

        Button chatLink = new Button { Text = "Chat" , WidthRequest = 100, HeightRequest = 50, TextColor = Color.Black, BackgroundColor = Color.FromHex("B3B3B3"), BorderColor = Color.Black, FontAttributes = FontAttributes.Bold, Font = Font.OfSize("Arial", 22) };
        

        


public ChatSelection()
        {


            if (Helpers.Settings.ContrastSetting == true) //contrast mode
            {
                BackgroundColor = Colors.contrastBg;
            }
            else
            {
                BackgroundColor = Colors.background;

            }

            Title = "Chat Selection";
            Icon = "Accounts.png";

            Label Desc = new Label { Text = "\nUpon toggling an attempt to connect you to an available volunteer will be made.\nIf you would like to be connected based on distance, ensure your location can be detected. \n\nFeel free to remain Anonymous by not entering a name.", FontSize = 20, FontFamily = "Arial" };
            Entry nameEntry = new Entry { Placeholder = "Enter your name: " };

            chatLink.Clicked += async (s, e) =>
            {
                try
                {
                    await Navigation.PushAsync(new ChatPage("Volunteer", new List<ChatMessage>(), "12345", nameEntry.Text));  //navigate to a state page (not new).
                }
                catch (Exception E)
                {
                    System.Diagnostics.Debug.WriteLine ("error"+E);
                }
            };

            readyToChat = new Switch
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand,

            };
            readyToChat.Toggled += readyToChatF;

            


            Device.BeginInvokeOnMainThread(() =>   //automatically updates
            {
                stackLayout = new StackLayout
                {
                    Children =
                            {
                            Desc,
                            nameEntry,
                            readyToChat,
                            chatAvailability

                            }

                };

                innerScroll = new ScrollView
                {
                    VerticalOptions = LayoutOptions.Start,
                    HorizontalOptions = LayoutOptions.Start,
                    Padding = new Thickness(5, 5, 5, 10),
                    Content = stackLayout

                };

                outerLayout = new StackLayout
                {
                    Spacing = 2,
                    Children =
                    {
                        innerScroll,

                    }
                };

                this.Content = outerLayout;
            });
            

        }
        void readyToChatF(object sender, ToggledEventArgs e)
        {

            System.Diagnostics.Debug.WriteLine("Switch toggled");

            if (readyToChat.IsToggled)
            {
                chatAvailability.Text = "Ready to chat.";
                stackLayout.Children.Add(chatLink);
                System.Diagnostics.Debug.WriteLine("Added button chatLink.");
            }
            else
            {
                chatAvailability.Text = "Not Ready to chat.";
                if (stackLayout.Children.Contains(chatLink))
                {
                    stackLayout.Children.Remove(chatLink);
                    System.Diagnostics.Debug.WriteLine("removed chatLink.");
                }
            }
            this.Content = outerLayout;
        }

        //------------------------------------------------------------------------
        protected async override void OnAppearing()
        {
            try
            {
                base.OnAppearing();
                outerLayout.Focus();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("bug: "+e);
            }
        }


        protected override Boolean OnBackButtonPressed() // back button pressed
        {
            ((RootPage)App.Current.MainPage).NavigateTo();
            
            return true;
        }

       

    }

}