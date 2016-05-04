using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Threading.Tasks;
using Akavache;
using System.Reactive.Linq;
using PCLCrypto;
using System.Text;

using Acr.UserDialogs;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using System.Threading;
using Newtonsoft.Json;
using Amazon.Lambda.Model;
using Amazon.Util;
using Amazon.Lambda;
using System.IO;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;

namespace LOSSPortable
{

    public class ChatPage : ContentPage
    {
        private String talkingToNickname = "User";
        private Button send;
        List<string> Messages = new List<string>();
        private Grid gridLayout;
        private StackLayout outerStack;
        private ScrollView innerScroll;
        private int MessageCount;
        private Entry editor = new Entry();
        CancellationTokenSource ct = new CancellationTokenSource();
        private static Boolean terminate;
        private DateTime date;
        String name;
        //        List<Message> msgs = new List<Message>(); //history of messaging
        
        
        public ChatPage(string UserName)  //General constructor for chatPage
        {

            date = Constants.date;
            terminate = true;
            if (UserName == "" || UserName == "Enter your name: " || UserName == null)
            {
                this.name = "Anonymous";
            }
            else
            {
                if (UserName.Length > 10)
                { UserName = UserName.Substring(0, 10) + "..."; }

                this.name = UserName;
            }

            if (Helpers.Settings.ContrastSetting == true)
            {
                
                BackgroundColor = Colors.contrastBg;
            }
            else
            {
                BackgroundColor = Colors.background;

            }
            
            Title = "Chat";
            Icon = "Accounts.png";

            
            editor.Placeholder = "Enter Message: ";

            editor.Focused += delegate {

                System.Diagnostics.Debug.WriteLine("messages count = " + MessageCount);
                innerScroll.HeightRequest = innerScroll.HeightRequest / 3;
                 
                ScrollEvent();
                this.Content = outerStack;
            };
            
            editor.Unfocused += delegate { //hide the keyboard

                innerScroll.HeightRequest = innerScroll.HeightRequest * 3;
                //ScrollEvent();
                this.Content = outerStack;
            };
            

            send = new Button { Text = "Send", TextColor = Color.White, BackgroundColor = Color.Gray, HorizontalOptions = LayoutOptions.FillAndExpand};


            

            //upon sending a message
            send.Clicked += delegate {
                String mes = editor.Text;
                editor.Text = "";


                try
                {
                    Messages.Add(mes);

                }
                catch (NullReferenceException ex)
                {
                    DisplayAlert("Alert", "Processor Usage" + ex.Message + mes, "OK");
                }

                ChatMessage message = new ChatMessage();
                //constructor:
                message.ToFrom = "" + AmazonUtils.Credentials.GetIdentityId() + "#" + AmazonUtils.Credentials.GetIdentityId();
                message.Icon = "drawable/prof.png";
                message.Sender = name;
                message.Reciever = "Reciever";
                message.Text = mes;
                message.Time = DateTime.Now.ToString("HH:mm:ss:ffff");
                message.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff");
                message.Id = AmazonUtils.Credentials.GetIdentityId();
                // "Sender: ", mes, "drawable/prof.png", " " + currentTime() );
                //msgs.Add(message);

                //System.Diagnostics.Debug.WriteLine("message info: " + message.ToFrom + message.Time + message.Text);
                DisplayResponse(message);
                Constants.conv.msgs.Add(message);
                MessageCount++;
                sendMessage(message);
            };

            Device.BeginInvokeOnMainThread(() =>   //automatically updates
            {

                gridLayout = new Grid
                {
                    RowSpacing = 3,
                    ColumnSpacing = 0,
                    Padding = new Thickness(0, 0, 0, 0),
                    VerticalOptions = LayoutOptions.Start,
                    //BackgroundColor = Color.FromHex("CCCCFF"),
                    RowDefinitions = { new RowDefinition { Height = GridLength.Auto } },

                    Children =
                        {
                            //label
                        }

                };

                innerScroll = new ScrollView
                {
                    VerticalOptions = LayoutOptions.Start,
                    HorizontalOptions = LayoutOptions.Start,
                    Padding = new Thickness(5, 5, 5, 10),
                    //BackgroundColor = Color.FromHex("CCCCFF"),
                    Content = gridLayout

                };

                gridLayout.ChildAdded += delegate // Automatically scrolls to bottom of the chat 
                {
                     ScrollEvent();
                };

                outerStack = new StackLayout
                {
                    //BackgroundColor = Color.FromHex("CCCCFF"),
                    VerticalOptions = LayoutOptions.Start,
                    Spacing = 0,
                    Children =
                            {
                                innerScroll,
                                editor,
                                send
                            }

                };

            //var heig = r.ParentView.Height;
                //Device.Styles.
                Rectangle bounds = outerStack.Bounds;
                int innerSize = Convert.ToInt32(bounds.Height - (bounds.Height / 8));
                innerScroll.HeightRequest =  App.ScreenHeight - 200; //440  -- change of 200;
                System.Diagnostics.Debug.WriteLine("window size set to: " + innerSize);
                Content = outerStack;
                Title = "" + Constants.conv.name;
                
            });

            //refreshView();
        }

        //individual message tapped in chat-> allow to report a message, hide the text, or delete the message
        async void OnLabelClicked(object s, EventArgs e, Label label, ChatMessage msg, int Type)
        {
            var action = await DisplayActionSheet(null, null, null, "Hide Text", "Report", "Delete Message");
            switch (action)
            {
                case "Report":
                    //await DisplayAlert("Alert", "Reporting not implemented yet.", "OK");
                    reportMessage(msg);
                    break;
                case "Hide Text":
                    var labels = s as Label;
                    labels.IsVisible = false;
                    break;
                case "Delete Message":
                    deleteMessage(msg.Id);
                    break;

            }
            this.Content = outerStack;

        }


        private void DisplayResponse(ChatMessage message) //adds input message to log 
        {
            String msg = message.Text;
            int numRows;
            if (msg == null || msg == "")
            {
                return;
            }
            else
            {
                numRows = msg.Length / 25;
            }

            Grid innerGrid = new Grid
            {   //CCCCFF
                VerticalOptions = LayoutOptions.Start,

                Padding = 3,
                RowSpacing = 3,
                RowDefinitions = {
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(numRows+1, GridUnitType.Auto) }
                },
                ColumnDefinitions = {
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) },
            }
            };

            if (message.Id == "456")
            { innerGrid.BackgroundColor = Color.FromHex("f2f2f2"); }
            else
            { innerGrid.BackgroundColor = Color.FromHex("d9d9d9"); }





            var profilePicture = new Image { };
            profilePicture.Source = message.Icon;
            profilePicture.VerticalOptions = LayoutOptions.StartAndExpand;
            //profilePicture.BackgroundColor = Color.FromHex("CCCCFF");


            Label name = new Label { Text = message.Sender, TextColor = Color.Black, FontAttributes = FontAttributes.Bold, FontSize = 20, FontFamily = "Arial" }; //, XAlign = TextAlignment.Start
            name.VerticalOptions = LayoutOptions.StartAndExpand;

            var datetime = DateTime.Now;
            Label time = new Label { Text = message.Time.Substring(0,5), TextColor = Color.Black, FontSize = 20, FontFamily = "Arial" };
            


            Label response = new Label { Text = message.Text, TextColor = Color.Black, FontSize = 18, FontFamily = "Arial" }; //, XAlign = TextAlignment.Start             
            var tgr = new TapGestureRecognizer();


            tgr.Tapped += (s, e) => OnLabelClicked(s, e, response, message, 1);
            response.GestureRecognizers.Add(tgr);
            response.VerticalOptions = LayoutOptions.Start;
            int labelLength = 2 + numRows;


            innerGrid.RowSpacing = 1;


            gridLayout.RowSpacing = 1;

            innerGrid.Children.Add(profilePicture);
            //innerGrid.Children.AddHorizontal(name);
            //innerGrid.Children.AddHorizontal(time);
            innerGrid.Children.Add(name,1,4,0,1);
            innerGrid.Children.Add(time,4,0);

            innerGrid.Children.Add(response, 0, 5, 1, labelLength);

            gridLayout.Children.AddVertical(innerGrid);
            gridLayout.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            //add buttons to grid, push that grid to the stack
        }

        public String getName()
        {
            return name;
        }

        
        public String currentTime()
        {
            var datetime = DateTime.Now;
            String minutes = "" + datetime.Minute;
            if (minutes.Length == 1)
            { minutes = "0" + minutes; }

            return "" + datetime.Hour + ":" + minutes;

        }

        public void deleteMessage(String id) //given the ID of a message, delete said message from messages.
        {
            ChatMessage found = Constants.conv.msgs.Find(x => x.Id == id);
            Constants.conv.msgs.Remove(found);
            MessageCount--;
            
            refreshView();
        }

        public void refreshView() //refreshes the view of the layout to display modifications
        {
            foreach (ChatMessage msg in Constants.conv.msgs)
            {
                DisplayResponse(msg);
            }

            this.Content = outerStack;
        }

        public void reportMessage(ChatMessage msg) //Enables users to report selected message based on specific criteria
        {

            Navigation.PushAsync(new ReportMessage(msg));
        }

        
        public void ScrollEvent() //Animation for chatpage to scroll to the bottom and display the latest message. 
        {
            innerScroll.ScrollToAsync(0, innerScroll.HeightRequest * (MessageCount + 10), true);
            System.Diagnostics.Debug.WriteLine("message count= " + MessageCount);
        }

        public async void sendMessage(ChatMessage message) //Sending a message to the server and therefore enabling the recipient to view the message. 
        {
            System.Diagnostics.Debug.WriteLine("trying to send to server: ");   
            //message sending to server:
            try{
                //DisplayResponse(message);
                await PushMessage(message.Text);
                }
            catch(Exception E)
            {
                System.Diagnostics.Debug.WriteLine("error storing in server "+ E);
            };
            this.Content = outerStack;
        }
        //-------------------------Get from Server-------------------------
        public void messagesFromServer(MessageLst[] MessageList) //upon recieving a chat History log
        {
            List<ChatMessage> msgs = new List<ChatMessage>();
            foreach (MessageLst i in MessageList){
                Constants.conv.msgs.Add(singleMessageFromServer(i.ToFrom, i.Time, i.Text));
            }
            //setChat(msgs); //sets the chat to show the message history.
            refreshView();
        }

        public ChatMessage singleMessageFromServer(String ToFrom, String Time, String Message)    //upon recieving a single message, add it to the log of messages.
        {
            string Sender;
            if (ToFrom.StartsWith(Constants.conv.id))
            {
                Sender = this.name;
            }
            else
            {
                Sender = Constants.conv.name;
            }
            ChatMessage newMsg = new ChatMessage()
            {
                Text = Message,
                
                Sender = Sender,
                Icon = "drawable/prof.png",
                Time = "",
                ToFrom = ToFrom
            };
            DateTime dt = Convert.ToDateTime(Time);
            var diff = DateTime.UtcNow - DateTime.Now;
            
            newMsg.Time = (dt - diff).ToString("HH:mm"); //change to only include HH:mm
            return newMsg;
        }

        //-------------------------Server----------------------------------
        async private Task PushMessage(string text) //Function to create a Json object and send to server using a lambda function
        {
            try
            {
                MessageItem message = new MessageItem { Item = new ChatMessage { ToFrom = Helpers.Settings.ToFromArn, Text = text } };
                MessageJson messageJson = new MessageJson { operation = "create", tableName = "Message", payload = message };
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
                System.Diagnostics.Debug.WriteLine("Error: " + e);
            }
        }

        async private Task LoadMessages() //Function to create a Json object and send to server using a lambda function
        {
            try
            {
                UserInfoItem message = new UserInfoItem { Item = new MessageLst { ToFrom = Helpers.Settings.ToFromArn, Time = date.ToString("yyyy-MM-dd HH:mm:ss:ffff") } }; //Helpers.Settings.EndpointArnSetting
                UserInfoJson messageJson = new UserInfoJson { operation = "read", tableName = "Message", payload = message };
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
                try
                {
                    System.Diagnostics.Debug.WriteLine("Returned Messages : " + myStr);
                    var response = JsonConvert.DeserializeObject<ConversationResponse>(myStr);
                    if (response.Success == "true")
                    {
                        var messages = response.MessageList;
                        messagesFromServer(messages);
                    }
                }
                catch (Exception e)
                {

                }

                //                System.Diagnostics.Debug.WriteLine("Status code: " + resp.StatusCode);
                //                System.Diagnostics.Debug.WriteLine("Response content: " + myStr);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Error:" + e);
            }

        }

        protected async override void OnAppearing() //Functionality to start the page that reads the history from the server and displays the log.
        {
            base.OnAppearing();
            

            MessagingCenter.Send<ChatPage>(this, "Start");
            MessagingCenter.Unsubscribe<App, ChatMessage>(this, "Hi");
            MessagingCenter.Subscribe<App, ChatMessage>(this, "Hi", (sender, arg) => //adds message to log
            {
                try
                {
                    DateTime dt = Convert.ToDateTime(arg.Time);
                    var diff = DateTime.UtcNow - DateTime.Now;
                    arg.Time = (dt - diff).ToString("HH:mm"); //change to only include HH:mm
                }
                catch (Exception e)
                {
                    arg.Time = DateTime.Now.Hour.ToString("HH:mm");

                }
                arg.Icon = "drawable/prof.png";
                DisplayResponse(arg);
                Constants.conv.msgs.Add(arg);
                ScrollEvent();
            });
            MessagingCenter.Unsubscribe<App>(this, "ConversationEnd");
            MessagingCenter.Subscribe<App, ChatMessage>(this, "ConversationEnd", (sender, arg) => //adds message to log
            {
                MessagingCenter.Send<ChatPage>(this, "End");
                MessagingCenter.Unsubscribe<App>(this, "ConversationEnd");
                this.send.IsEnabled = false;
                if (terminate)
                {
                    terminateChat();
                }
            });
            await LoadMessages();
           
            this.MessageCount = Constants.conv.msgs.Count;
            
            this.Content = outerStack;
            ScrollEvent();
        }
        
        protected override void OnDisappearing() //leaving the page ->cache history
        {
            editor.Keyboard = null;
            editor.Unfocus();
            MessagingCenter.Unsubscribe<App, ChatMessage>(this, "Hi"); 
            MessagingCenter.Send<ChatPage>(this, "End");
            System.Diagnostics.Debug.WriteLine("storing");
            Constants.date = DateTime.UtcNow;
            System.Diagnostics.Debug.WriteLine("Date should be updated : " + Constants.date.ToString());
            base.OnDisappearing();
        }

        private async void terminateChat()
        {
            
            if (terminate) { 
                terminate = false; 
                await DisplayAlert("Conversation ended", "User has left the conversation", "OK");
            }   
        }
    }
}