using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Threading.Tasks;
using Akavache;
using System.Reactive.Linq;
using System.Threading;
using Newtonsoft.Json;
using Amazon.Lambda.Model;
using Amazon.Util;
using System.IO;
using Amazon.Lambda;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;

namespace LOSSPortable
{


    public class ChatSelection : ContentPage
    {

        
      
        private StackLayout stackLayout;
        private StackLayout outerLayout;
        private ScrollView innerScroll;
        Entry nameEntry;
        private double longitude;
        private double latitude;


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


                await Handshake();
                try //open a new chatPage
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
                IsToggled = Helpers.Settings.ChatActiveSetting
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
                Helpers.Settings.ChatActiveSetting = true;
                chatAvailability.Text = "Ready to chat.";
                stackLayout.Children.Add(chatLink);
                System.Diagnostics.Debug.WriteLine("Added button chatLink.");
                HandshakeStart();
            }
            else
            {
                Helpers.Settings.ChatActiveSetting = false;
                chatAvailability.Text = "Not Ready to chat.";
                if (stackLayout.Children.Contains(chatLink))
                {
                    stackLayout.Children.Remove(chatLink);
                    System.Diagnostics.Debug.WriteLine("removed chatLink.");
                }
            }
            this.Content = outerLayout;
        }
        //--------------------------------HANDSHAKE-------------------------------
        public async void HandshakeStart()
        {
                await getLocation(); //check geolocation
                await Handshake(); //handshake attempt
                await DisplayAlert("hello", "Handshake finished. long= "+longitude+ " lat= "+latitude , "ok");
        }
        //-------------------------Caching---------------------------------


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


        async private Task Handshake() //Function to create a Json object and send to server using a lambda function
        {
            try
            {
                UserInfoItem message = new UserInfoItem { Item = new UserInfo { Latitude = latitude, Longitude = longitude, Nickname = "temp", Arn = "" + Helpers.Settings.EndpointArnSetting } }; //Helpers.Settings.EndpointArnSetting
                //await DisplayAlert("sending","Arn: " + Helpers.Settings.EndpointArnSetting, "ok");
                await DisplayAlert("sent ", "sent to server: " + latitude + " " + longitude + " " + nameEntry.Text + " " + Helpers.Settings.EndpointArnSetting, "ok");
                UserInfoJson messageJson = new UserInfoJson { operation = "create", tableName = "User", payload = message };
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


        //-------------------------geolocation-----------------------------


        async private Task getLocation()
        {
            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 100;
                locator.PositionChanged += OnPositionChanged;
                var position = await locator.GetPositionAsync(timeoutMilliseconds: 10000);
                latitude = Convert.ToDouble(position.Latitude);
                longitude = Convert.ToDouble(position.Longitude);

                //label2.Text = String.Format("Longitude: {0} Latitude: {1}", longtitude, latitude);
                //System.Diagnostics.Debug.WriteLine("Got Location!");
                //await DisplayAlert("type", "" + latitude.GetType(), "ok");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                //label2.Text = "Unable to find location";
                latitude = 0.00;
                longitude = 0.00;
            }

        }

        async private void OnPositionChanged(object sender, PositionEventArgs e)
        {
            var locator = CrossGeolocator.Current;
            var position = await locator.GetPositionAsync(timeoutMilliseconds: 10000);
            var latitude = position.Latitude.ToString();
            var longtitude = position.Longitude.ToString();
            //label2.Text = String.Format("Longitude: {0} Latitude: {1}", longtitude, latitude);
        }
    }

}