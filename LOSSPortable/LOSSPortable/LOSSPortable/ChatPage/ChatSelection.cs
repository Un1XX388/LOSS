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
		Label instructionLabel = new Label { Text = "Description of chat page goes here", FontSize = 22, TextColor = Color.White};
        Label chatAvailability = new Label { HorizontalOptions = LayoutOptions.StartAndExpand, Text = "Not Ready to chat.", IsVisible = false, HorizontalTextAlignment = TextAlignment.Center, FontSize = 20, FontFamily = "Arial" };
        Button update = new Button { Text = "Update", HeightRequest = 50, IsVisible = false, TextColor = Color.Black, BackgroundColor = Color.FromHex("B3B3B3"), BorderColor = Color.Black, FontAttributes = FontAttributes.Bold, Font = Font.OfSize("Arial", 22), BorderWidth = 1 };
        Button startConversation = new Button { HorizontalOptions = LayoutOptions.FillAndExpand, Text = "Start Conversation", WidthRequest = 100, HeightRequest = 50, TextColor = Color.Black, BackgroundColor = Color.FromHex("B3B3B3"), BorderColor = Color.Black, FontAttributes = FontAttributes.Bold, Font = Font.OfSize("Arial", 22), BorderWidth = 1 };
        Button endConversation = new Button { HorizontalOptions = LayoutOptions.FillAndExpand, Text = "End Conversation", WidthRequest = 100, HeightRequest = 50, TextColor = Color.Black, BackgroundColor = Color.FromHex("B3B3B3"), BorderColor = Color.Black, FontAttributes = FontAttributes.Bold, Font = Font.OfSize("Arial", 22), BorderWidth = 1 };
        private double longitude;
        private double latitude;
        
        

        public ChatSelection() //general constructor for chat selection. initializes the page with layout, images, colors, buttons,  and actions
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
            nameEntry.Text = String.IsNullOrWhiteSpace(Helpers.Settings.UsernameSetting) ? "Anonymous" : Helpers.Settings.UsernameSetting;
            Title = "Chat Selection";
            Icon = "Accounts.png";

            //spinning circle that appears in upper right corner while waiting on server response
            ActivityIndicator loading = new ActivityIndicator()
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Color = Color.Black,
                BindingContext = this,
                IsEnabled = true,
                IsRunning = false

            };
            loading.SetBinding(ActivityIndicator.IsVisibleProperty, "IsBusy");

            /* Changes the UI based on whether the user is a logged in volunteer or a basic user*/
            if (Helpers.Settings.IsVolunteer)
            {
                this.instructionLabel.Text = "If a chat request has been made, press the 'Enter Conversation' button to enter the converation. To terminate a conversation, press the 'Terminate Converation'. To toggle availability for chat requests, use the switch below.";
                this.startConversation.IsVisible = false;
                this.chatAvailability.IsVisible = true;
                this.endConversation.IsVisible = false;
                this.nameEntry.IsVisible = false;
                this.nameEntry.IsEnabled = false;
                this.nameEntry.Text = String.IsNullOrWhiteSpace(Helpers.Settings.UsernameSetting) ? "Anonymous" : Helpers.Settings.UsernameSetting;
                readyToChat.IsVisible = true;
                readyToChat.IsEnabled = true;
                readyToChat.Toggled += readyToChatF;
                this.startConversation.IsEnabled = false;
                queryServerActiveConversation();
            }
            else
            {
                this.instructionLabel.Text = "To request a converation with a volunteer certified to support those in needs, press the 'Start Conversation' button below. To end a conversation, press the 'End Conversation' button.";
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
                Helpers.Settings.UsernameSetting = nameEntry.Text;
                this.startConversation.IsEnabled = false;
                HandshakeStart();
                update.IsVisible = false;
                
            };

            /**
             * When the 'display name' field is changed, update button becomes visible.
             */
            nameEntry.TextChanged += (s, e) =>
            {
                update.IsVisible = true;
            };

            /*
             * Called when end conversation button pressed, resets UI to start conversation setup
             * Also sends server notification to let connected party know the conversation is over
             */
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


            Device.BeginInvokeOnMainThread(() =>   //automatically updates when page loads
            {
                stackLayout = new StackLayout
                {
                    Children = {
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
                this.nameEntry.IsVisible = false;
                this.nameEntry.IsEnabled = false;
                this.nameEntry.Text = String.IsNullOrWhiteSpace(Helpers.Settings.UsernameSetting) ? "Anonymous" : Helpers.Settings.UsernameSetting;
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
        /**
         * path to reset the UI to start conversation for basic or volunteer users
         */       
        private void startConversationPath(){
            if (Helpers.Settings.IsVolunteer)
            {
                startConversation.IsVisible = false;
                startConversation.IsEnabled = true;
                endConversation.IsVisible = false;
                this.nameEntry.IsVisible = false;
                this.nameEntry.IsEnabled = false;
                this.nameEntry.Text = String.IsNullOrWhiteSpace(Helpers.Settings.UsernameSetting) ? "Anonymous" : Helpers.Settings.UsernameSetting;
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
        /**
         * event called when the continue button is clicked, disables the listerners
         * and try's to active a new chat page, initializes the page with the continue 
         * conversation event.
         */
        async void ContinueConversationEvent(object s, EventArgs e)
            {
                startConversation.Clicked -= ContinueConversationEvent;
                
                try
                {
                    var stack = Navigation.NavigationStack;
                    if (stack[stack.Count - 1].GetType() != typeof(ChatPage))
                        await Navigation.PushAsync(new ChatPage(), false);
                }
                catch (Exception E)
                {
                    System.Diagnostics.Debug.WriteLine("error" + E);
                }
                startConversation.Clicked += ContinueConversationEvent;
            }

        
        /**
         * Event called when the page is in start conversation mode and the start 
         * conversation button is tapped by the user.
         */
        async void InitiateConversationEvent(object s, EventArgs e)
                {
                    await initiateConversation();
                }

        /**
         * Toggle function only visible to volunteers that allows them to 
         * set whether or not they can be contacted by basic users.
         */
        void readyToChatF(object sender, ToggledEventArgs e)
        {
            if (readyToChat.IsToggled)
            {
                Helpers.Settings.ChatActiveSetting = true;
                chatAvailability.Text = "Available to chat";
                HandshakeStart();
            }
            else
            {
                Helpers.Settings.ChatActiveSetting = false;
                chatAvailability.Text = "Not available";
                HandshakeStart();
            }
            //this.Content = outerLayout;
        }
        //--------------------------------HANDSHAKE-------------------------------
        //Starts the handshake by getting the geolocation and then starting the handshake to find a nearby volunteer
        public async Task HandshakeStart() 
        {
            this.IsBusy = true;
            this.startConversation.IsEnabled = false;
            await getLocation(); //check geolocation
            await Handshake(); //handshake attempt
            this.startConversation.IsEnabled = true;
            this.IsBusy = false;
        }
        //-------------------------Caching---------------------------------


        //------------------------------------------------------------------------
       /**
        * initializes message center listeners for incoming messages and termination message
        * also checks for handshake, and initializes it if it hasn't been done before
        */
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
                Helpers.Settings.ToFromArn = "";
                startConversationPath();
            });

            if (!Helpers.Settings.HandShakeDone)
            {
                await HandshakeStart();
            }
            try {
                outerLayout.Focus();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Error : " + e);
            }
        }

        /**
         * Called on class loss of focus, changes page visible boolean in App class
         * 
         */
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Send<ChatSelection>(this, "End");
        }

        /**
         * Returns user to home page intead of exiting app
         * 
         */
        protected override Boolean OnBackButtonPressed() // back button pressed navigate to the homepage.
        {
            ((RootPage)App.Current.MainPage).NavigateTo();
            return true;
        }

        /**
         * Called when the start button is tapped, generates request to server
         * checks response and either actives enter conversation mode or prompts
         * user with failed to start message.
         */
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
                            await Navigation.PushAsync(new ChatPage(), false);
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
        /**
         * Called when the end conversation button is pressed by the user
         * Generates request to server, which then terminates the conversation 
         * for both users.
         */
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
            Helpers.Settings.ToFromArn = "";
            Constants.conv.name = "";
            Constants.conv.id = "";
        }
        /**
         * Queries the server for active conversations, called when the page is loaded
         * if there exists an active conversation, returns conversation information to
         * user and activates continue conversation option.
         */
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
        /**
         * Generates or updates user account when the page is accessed or information is changed.
         * Sends request to server which updates the user table.
         */
        async private Task Handshake() 
        {
            try
            {
                string operation;
                if (!Helpers.Settings.HandShakeDone)
                {
                    operation = "create";
                    Helpers.Settings.HandShakeDone = true;
                }
                else
                {
                    operation = "update";
                }
                UserInfoItem message;
                if (Helpers.Settings.IsVolunteer)
                {
                    if(Helpers.Settings.ChatActiveSetting){
                        message = new UserInfoItem { Item = new UserInfo { Latitude = latitude, Longitude = longitude, Nickname = Helpers.Settings.UsernameSetting, Arn = Helpers.Settings.EndpointArnSetting, Available = true } };
                    }else{
                        message = new UserInfoItem { Item = new UserInfo { Latitude = latitude, Longitude = longitude, Nickname = Helpers.Settings.UsernameSetting, Arn = Helpers.Settings.EndpointArnSetting, Available = false } };
                    }
                }
                else{
                    message = new UserInfoItem { Item = new UserInfo { Latitude = latitude, Longitude = longitude, Nickname = this.nameEntry.Text, Arn = Helpers.Settings.EndpointArnSetting, Available = true } }; //Helpers.Settings.EndpointArnSetting
                }
                Helpers.Settings.UsernameSetting = this.nameEntry.Text;
                UserInfoJson messageJson = new UserInfoJson { operation = operation, tableName = "User", payload = message };
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
        //creates the geolocation based on your specific coordinates. if disabled, returns (0,0)
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

        async private void OnPositionChanged(object sender, PositionEventArgs e) //updates geolocation
        {
            var locator = CrossGeolocator.Current;
            var position = await locator.GetPositionAsync(timeoutMilliseconds: 10000);
            var latitude = position.Latitude.ToString();
            var longtitude = position.Longitude.ToString();
        }
        #endregion 
    }
}