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

            /**
             * Checks to see if a conversation is already active, if true, when startConversation
             * is pressed, moves to the ChatPage, otherwise initiate conversations with server.
             */
            if (Helpers.Settings.ConversationOn == true)
            {
                continueConversationPath();
            }
            else
            {
                startConversationPath();
            }

            //Sets displayName field to value saved
            nameEntry.Text = Helpers.Settings.DisplayName;

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
                //called when the conversation is to be ended.
                Helpers.Settings.ConversationOn = false;
                startConversation.Text = "Start Conversation";
                await terminateConversation();
                startConversationPath();
            };

            Title = "Chat Selection";
            Icon = "Accounts.png";

            //Take this out once this is being assigned by the login
            Helpers.Settings.IsVolunteer = true;


            if (Helpers.Settings.IsVolunteer)
            {
                this.chatAvailability.IsVisible = true;
                readyToChat.IsVisible = true;
                readyToChat.IsEnabled = true;
                readyToChat.Toggled += readyToChatF;
                queryServerActiveConversation();
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
            nameEntry.IsEnabled = false;
            startConversation.Text = "Continue Conversation";
            endConversation.IsVisible = true;
            startConversation.Clicked -= InitiateConversationEvent;
            startConversation.Clicked += ContinueConversationEvent;
        }

        private void startConversationPath(){
                startConversation.Text = "Start Conversation";
                endConversation.IsVisible = false;
                nameEntry.IsEnabled = true;
                startConversation.Clicked -= ContinueConversationEvent;
                startConversation.Clicked += InitiateConversationEvent;
                }

        async void ContinueConversationEvent(object s, EventArgs e)
            {
                try
                {
                    //resume stored converation and go to chat page.
                    
                    await Navigation.PushAsync(new ChatPage(this.nickName, new List<ChatMessage>(), "12345", nameEntry.Text));  //navigate to a state page (not new).
                }
                catch (Exception E)
                {
                    System.Diagnostics.Debug.WriteLine("error" + E);
                }
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
            MessagingCenter.Subscribe<App, ChatMessage>(this, "Handshake", (sender, arg) => //adds message to log
            {
                Helpers.Settings.ToFromArn = arg.ToFrom;
                this.nickName = arg.Sender;
                Helpers.Settings.ConversationOn = true;
                continueConversationPath();
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


        protected override Boolean OnBackButtonPressed() // back button pressed
        {
            ((RootPage)App.Current.MainPage).NavigateTo();
            return true;
        }

        async private Task initiateConversation(){
                UserInfoItem message = new UserInfoItem { Item = new UserInfo { } }; //Helpers.Settings.EndpointArnSetting
                UserInfoJson messageJson = new UserInfoJson { operation = "newConversation", tableName = "User", payload = message };
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
                Boolean failure = false;
                try {
                    var response = JsonConvert.DeserializeObject<ConversationResponse>(myStr);
                    
                    System.Diagnostics.Debug.WriteLine("initiateConversationresponse : " + myStr);
                    System.Diagnostics.Debug.WriteLine("ToFromArn : " + response.ID);
                    if (response.Success == "true")
                    {
                        nickName = response.Nickname;
                        Helpers.Settings.ToFromArn = response.ID;
                        continueConversationPath();
                        await Navigation.PushAsync(new ChatPage(nickName, new List<ChatMessage>(), "12345", nameEntry.Text));
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
            Helpers.Settings.ToFromArn = "";
            System.Diagnostics.Debug.WriteLine("terminateConversation : " + myStr);
        }

        async private Task queryServerActiveConversation()
        {
            UserInfoItem message = new UserInfoItem { Item = new UserInfo {} }; //Helpers.Settings.EndpointArnSetting
            UserInfoJson messageJson = new UserInfoJson { operation = "currentConversations", tableName = "User", payload = message };
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
            Boolean failure = false;
            try
            {

                var response = JsonConvert.DeserializeObject<ConversationResponse>(myStr);
                if (response.Success == "true")
                {
                    nickName = response.Nickname;
                    Helpers.Settings.ToFromArn = response.ID;
                    System.Diagnostics.Debug.WriteLine(response.ID);
                    continueConversationPath();
                }
                else
                {
                    await DisplayAlert("Error", "Unable to connect to conversation request, please try again later.", "OK");
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                failure = true;
            }
            if (failure) { await DisplayAlert("Error", "Unable to connect to conversation request, please try again later.", "OK"); }
            System.Diagnostics.Debug.WriteLine("terminateConversation : " + myStr);
        }

        async private Task Handshake() //Function to create a Json object and send to server using a lambda function
        {
            try
            {
                UserInfoItem message = new UserInfoItem { Item = new UserInfo { Latitude = latitude, Longitude = longitude, Nickname = nameEntry.Text, Arn = Helpers.Settings.EndpointArnSetting } }; //Helpers.Settings.EndpointArnSetting
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