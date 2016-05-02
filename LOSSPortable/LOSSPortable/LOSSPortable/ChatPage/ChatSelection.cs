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
        Entry nameEntry = new Entry { Placeholder = "Enter your display name: " };

        Switch readyToChat = new Switch { HorizontalOptions = LayoutOptions.EndAndExpand, IsVisible = false, IsEnabled = false, VerticalOptions = LayoutOptions.CenterAndExpand, IsToggled = Helpers.Settings.ChatActiveSetting };
        Label instructionLabel = new Label { Text = "Description of chat page goes here", FontSize = 22};
        Label chatAvailability = new Label { HorizontalOptions = LayoutOptions.StartAndExpand, Text = "Not Ready to chat.", IsVisible = false, HorizontalTextAlignment = TextAlignment.Center, FontSize = 20, FontFamily = "Arial" };
        Button update = new Button { Text = "Update", HeightRequest = 50, IsVisible = false, TextColor = Color.Black, BackgroundColor = Color.FromHex("B3B3B3"), BorderColor = Color.Black, FontAttributes = FontAttributes.Bold, Font = Font.OfSize("Arial", 22), BorderWidth = 1 };
        Button startConversation = new Button { HorizontalOptions = LayoutOptions.FillAndExpand, Text = "Start Conversation", WidthRequest = 100, HeightRequest = 50, TextColor = Color.Black, BackgroundColor = Color.FromHex("B3B3B3"), BorderColor = Color.Black, FontAttributes = FontAttributes.Bold, Font = Font.OfSize("Arial", 22), BorderWidth = 1 };
        Button endConversation = new Button { HorizontalOptions = LayoutOptions.FillAndExpand, Text = "End Conversation", WidthRequest = 100, HeightRequest = 50, TextColor = Color.Black, BackgroundColor = Color.FromHex("B3B3B3"), BorderColor = Color.Black, FontAttributes = FontAttributes.Bold, Font = Font.OfSize("Arial", 22), BorderWidth = 1 };
        private double longitude;
        private double latitude;
        private string nickName = "";
        

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

            //Sets displayName field to value saved
            nameEntry.Text = Helpers.Settings.DisplayName;
            Title = "Chat Selection";
            Icon = "Accounts.png";


            if (Helpers.Settings.IsVolunteer)
            {
                this.startConversation.IsVisible = false;
                this.chatAvailability.IsVisible = true;
                this.endConversation.IsVisible = false;
                readyToChat.IsVisible = true;
                readyToChat.IsEnabled = true;
                readyToChat.Toggled += readyToChatF;
                this.startConversation.IsEnabled = false;
                queryServerActiveConversation();
            }
            else
            {
                this.startConversation.IsVisible = true;
                this.chatAvailability.IsVisible = false;
                this.endConversation.IsVisible = false;
                readyToChat.IsVisible = false;
                readyToChat.IsEnabled = false;
                this.startConversation.IsEnabled = false;
                queryServerActiveConversation();
            }


            
            
         
            /**
             * When 'update' is pressed, the handshake is redone with the new 
             */
            update.Clicked += (s, e) =>
            {
                HandshakeStart();
                update.IsVisible = false;
                Helpers.Settings.DisplayName = nameEntry.Text;
            };

            /**
             * When the 'display name' field is changed, update button becomes visible.
             */
            nameEntry.TextChanged += (s, e) =>
            {
                update.IsVisible = true;
            };

            endConversation.Clicked += async (s, e) =>
            {
                await terminateConversation();
                startConversationPath();
            };
            if (Helpers.Settings.ChatActiveSetting)
            {
                this.chatAvailability.Text = "Available";
            }
            else
            {
                this.chatAvailability.Text = "Not available";
            }
            
            var tmp = new StackLayout
            {
                Children =
                {
                    nameEntry, update
                },
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.StartAndExpand 
            };

            var chatAvailtoChat = new StackLayout{
                                Children = {
                                    chatAvailability, 
                                    readyToChat}
                                ,
                                Orientation = StackOrientation.Horizontal
                            };


            Device.BeginInvokeOnMainThread(() =>   //automatically updates
            {
                stackLayout = new StackLayout
                {
                    Children =
                            {
                            instructionLabel,
                            tmp,
                            chatAvailtoChat,
                            startConversation, endConversation
                            },
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            
                };

                innerScroll = new ScrollView
                {
                    VerticalOptions = LayoutOptions.StartAndExpand,
                    HorizontalOptions = LayoutOptions.StartAndExpand,
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

        /**
         * If converation is acstive, then is disables display name changed,
         * makes end conversation button visible, changes text of start conversatino
         * button to continue conversation and changes
         */
        private void continueConversationPath()
        {
            if (Helpers.Settings.IsVolunteer)
            {
                startConversation.Text = "Enter Conversation";
                startConversation.IsVisible = true;
                startConversation.IsEnabled = true;
                nameEntry.IsEnabled = false;
                endConversation.IsVisible = true;
                startConversation.Clicked += ContinueConversationEvent;
            }
            else
            {
                startConversation.Text = "Enter Conversation";
                startConversation.IsEnabled = true;
                nameEntry.IsEnabled = false;
                endConversation.IsVisible = true;
                startConversation.Clicked -= InitiateConversationEvent;
                startConversation.Clicked -= ContinueConversationEvent;
                startConversation.Clicked += ContinueConversationEvent;
            }
        }

        private void startConversationPath(){
            if (Helpers.Settings.IsVolunteer)
            {
                startConversation.IsVisible = false;
                startConversation.IsEnabled = true;
                endConversation.IsVisible = false;
                nameEntry.IsEnabled = true;
                startConversation.Clicked -= ContinueConversationEvent;
            }
            else
            {
                startConversation.Text = "Start Conversation";
                startConversation.IsEnabled = true;
                endConversation.IsVisible = false;
                nameEntry.IsEnabled = true;
                startConversation.Clicked -= ContinueConversationEvent;
                startConversation.Clicked -= InitiateConversationEvent;
                startConversation.Clicked += InitiateConversationEvent;
            }        
        }

        async void ContinueConversationEvent(object s, EventArgs e)
            {
                startConversation.Clicked -= ContinueConversationEvent;
                
                try
                {
                    var stack = Navigation.NavigationStack;
                    if (stack[stack.Count - 1].GetType() != typeof(ChatPage))
                        await Navigation.PushAsync(new ChatPage(nameEntry.Text), false);
                }
                catch (Exception E)
                {
                    System.Diagnostics.Debug.WriteLine("error" + E);
                }
                startConversation.Clicked += ContinueConversationEvent;
            }

        
        
        async void InitiateConversationEvent(object s, EventArgs e)
                {
                    await initiateConversation();
                }

        void readyToChatF(object sender, ToggledEventArgs e)
        {
            if (readyToChat.IsToggled)
            {
                Helpers.Settings.ChatActiveSetting = true;
                chatAvailability.Text = "Available to chat";
            }
            else
            {
                Helpers.Settings.ChatActiveSetting = false;
                chatAvailability.Text = "Not available";
            }
            //this.Content = outerLayout;
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

            base.OnAppearing();
            MessagingCenter.Send<ChatSelection>(this, "Start");

            MessagingCenter.Subscribe<App, ChatMessage>(this, "Handshake", (sender, arg) => //adds message to log
            {
                if (Helpers.Settings.IsVolunteer)
                {
                    Helpers.Settings.ToFromArn = arg.ToFrom;
                    Constants.conv.name = arg.Sender;
                    continueConversationPath();
                }
            });

            MessagingCenter.Subscribe<App, ChatMessage>(this, "HandshakeEnd", (sender, arg) => //adds message to log
            {
                this.nickName = "";
                Helpers.Settings.ToFromArn = "";
                startConversationPath();
            });

            if (Helpers.Settings.HandShakeDone == false)
            {
                HandshakeStart();
                Helpers.Settings.HandShakeDone = true;
            }
            try {
                outerLayout.Focus();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Error : " + e);
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<App, ChatMessage>(this, "Handshake");
            MessagingCenter.Unsubscribe<App, ChatMessage>(this, "HandshakeEnd");
            MessagingCenter.Send<ChatSelection>(this, "End");
        }

        protected override Boolean OnBackButtonPressed() // back button pressed
        {
            ((RootPage)App.Current.MainPage).NavigateTo();
            return true;
        }

        async private Task initiateConversation(){
                UserInfoItem message = new UserInfoItem { Item = new UserInfo { } }; //Helpers.Settings.EndpointArnSetting
                UserInfoJson messageJson = new UserInfoJson { operation = "newConversation", tableName = "User", payload = message };
                string args = JsonConvert.SerializeObject(messageJson);
                System.Diagnostics.Debug.WriteLine("newConversation: " + args);
                var ir = new InvokeRequest()
                {
                    FunctionName = "arn:aws:lambda:us-east-1:987221224788:function:Test_Backend",
                    PayloadStream = AWSSDKUtils.GenerateMemoryStreamFromString(args),
                    InvocationType = InvocationType.RequestResponse
                };

                InvokeResponse resp = await AmazonUtils.LambdaClient.InvokeAsync(ir);
                resp.Payload.Position = 0;
                var sr = new StreamReader(resp.Payload);
                var myStr = sr.ReadToEnd();
                Boolean failure = false;
                try {
                    System.Diagnostics.Debug.WriteLine("newConversations : " + myStr);
                    var response = JsonConvert.DeserializeObject<ConversationResponse>(myStr);
                    if (response.Success == "true")
                    {
                        Constants.conv.name = response.Nickname;
                        Constants.conv.id = response.ID;
                        Constants.conv.msgs.Clear();
                        Helpers.Settings.ToFromArn = response.ID;
                        continueConversationPath();
                        var stack = Navigation.NavigationStack;
                        if (stack[stack.Count - 1].GetType() != typeof(ChatPage))
                            await Navigation.PushAsync(new ChatPage(nameEntry.Text), false);
                    }
                    else
                    {
                        await DisplayAlert("Error", "No available volunteers, please try again later.", "OK");
                    }
                }
                catch(Exception e)
                {
                    failure = true;
                    
                }
                if (failure)
                {
                    await DisplayAlert("Error", "No available volunteers, please try again later.", "OK");
                }
        }

        async private Task terminateConversation()
        {
            //ID = is from ToFrom Field
            UserInfoItem message = new UserInfoItem { Item = new UserInfo { ID = Helpers.Settings.ToFromArn } }; //Helpers.Settings.EndpointArnSetting
            UserInfoJson messageJson = new UserInfoJson { operation = "stopConversation", tableName = "User", payload = message };
            string args = JsonConvert.SerializeObject(messageJson);
            System.Diagnostics.Debug.WriteLine("stopConversation: " + args);
            var ir = new InvokeRequest()
            {
                FunctionName = "arn:aws:lambda:us-east-1:987221224788:function:Test_Backend",
                PayloadStream = AWSSDKUtils.GenerateMemoryStreamFromString(args),
                InvocationType = InvocationType.RequestResponse
            };


            InvokeResponse resp = await AmazonUtils.LambdaClient.InvokeAsync(ir);
            resp.Payload.Position = 0;
            var sr = new StreamReader(resp.Payload);
            var myStr = sr.ReadToEnd();
            System.Diagnostics.Debug.WriteLine("stopConversation : " + myStr);
            this.nickName = "";
            Helpers.Settings.ToFromArn = "";
            Constants.conv.name = "";
            Constants.conv.id = "";
        }

        async private Task queryServerActiveConversation()
        {
            UserInfoItem message = new UserInfoItem { Item = new UserInfo {} }; //Helpers.Settings.EndpointArnSetting
            UserInfoJson messageJson = new UserInfoJson { operation = "currentConversations", tableName = "User", payload = message };
            string args = JsonConvert.SerializeObject(messageJson);

            System.Diagnostics.Debug.WriteLine("currentConversation: " + args);
            var ir = new InvokeRequest()
            {
                FunctionName = "arn:aws:lambda:us-east-1:987221224788:function:Test_Backend",
                PayloadStream = AWSSDKUtils.GenerateMemoryStreamFromString(args),
                InvocationType = InvocationType.RequestResponse
            };


            InvokeResponse resp = await AmazonUtils.LambdaClient.InvokeAsync(ir);
            resp.Payload.Position = 0;
            var sr = new StreamReader(resp.Payload);
            var myStr = sr.ReadToEnd();
            Boolean failure = false;
            try
            {

                var response = JsonConvert.DeserializeObject<ConversationResponse>(myStr);
                System.Diagnostics.Debug.WriteLine("currentConversations : " + myStr);
                if (response.Success == "true")
                {
                    if (response.Conversations[0].ID != Helpers.Settings.ToFromArn)
                    {
                        Constants.conv.name = response.Conversations[0].Nickname;
                        Constants.conv.id = response.Conversations[0].ID;
                        Helpers.Settings.ToFromArn = response.Conversations[0].ID;
                        Constants.conv.msgs.Clear();
                    }
                    else
                    {
                        Constants.conv.name = response.Conversations[0].Nickname;
                        Constants.conv.id = response.Conversations[0].ID;
                    }
                    continueConversationPath();
                }
                else if (response.Success == "false")
                {
                    nickName = "";
                    Helpers.Settings.ToFromArn = "";
                    startConversationPath();
                }
                else
                {
                    await DisplayAlert("Error", "No active conversations.", "OK");
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                failure = true;
            }
            if (failure) { await DisplayAlert("Error", "No active conversations", "OK"); }
        }

        async private Task Handshake() //Function to create a Json object and send to server using a lambda function
        {
            try
            {
                UserInfoItem message = new UserInfoItem { Item = new UserInfo { Latitude = latitude, Longitude = longitude, Nickname = nameEntry.Text, Arn = Helpers.Settings.EndpointArnSetting, Available = true } }; //Helpers.Settings.EndpointArnSetting
                //await DisplayAlert("sending","Arn: " + Helpers.Settings.EndpointArnSetting, "ok");
                //await DisplayAlert("sent ", "sent to server: " + latitude + " " + longitude + " " + "Name" + " " + Helpers.Settings.EndpointArnSetting, "ok");
                UserInfoJson messageJson = new UserInfoJson { operation = "create", tableName = "User", payload = message };
                string args = JsonConvert.SerializeObject(messageJson);
                var ir = new InvokeRequest()
                {
                    FunctionName = "arn:aws:lambda:us-east-1:987221224788:function:Test_Backend",
                    PayloadStream = AWSSDKUtils.GenerateMemoryStreamFromString(args),
                    InvocationType = InvocationType.RequestResponse
                };
                

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
        #region Geolocation
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
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
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
        }
        #endregion
    }
}