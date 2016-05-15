using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Newtonsoft.Json;
using Amazon.Lambda.Model;
using Amazon.Util;
using Amazon.Lambda;
using System.IO;


namespace eLOSSTeam
{

    public class ChatPage : ContentPage
    {
        
        CancellationTokenSource ct = new CancellationTokenSource();
        private static Boolean terminate;

        Button sendButton;
        Editor textEditor;
        ScrollView messageView;
        StackLayout mainStack;
        StackLayout messageStack;
        StackLayout editorStack;
        ScrollView rootScrollView;
        double width;
        string previousUser = "";
        string mainUser;
        string convWithUser;


        public ChatPage()  //General constructor for chatPage
        {
            terminate = true;

            mainUser = checkName(Helpers.Settings.UsernameSetting);
            convWithUser = checkName(Constants.conv.name);


            if (Helpers.Settings.ContrastSetting == true)
            {
                
                BackgroundColor = Colors.contrastBg;
            }
            else
            {
                BackgroundColor = Colors.background;

            }

            Title = convWithUser;
            Icon = "Accounts.png";

            textEditor = new Editor()
            {
                Keyboard = Keyboard.Default,
                BackgroundColor = Color.White,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HeightRequest = 50,
                TextColor = Color.Purple
            };

            textEditor.Focused += (s, e) => TextEditor_Focused(s, e);
            textEditor.Unfocused += (s, e) => TextEditor_Unfocused(s, e);


            Frame textEditorFrame = new Frame()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                BackgroundColor = Color.White,
                Content = textEditor,
                Padding = new Thickness(8, 0, 0, 0)
            };

            BoxView verticalLine = new BoxView()
            {
                WidthRequest = 2,
                HeightRequest = 40,
                Color = Color.Purple,
            };

            sendButton = new Button()
            {
                Text = ">",
                BorderRadius = 2,
                BackgroundColor = Color.White,
                TextColor = Color.Purple,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Start,
                HeightRequest = 50,
                WidthRequest = 50,
                FontSize = 30,
            };

            sendButton.Clicked += SendButtonClicked;

            Frame sendButtonFrame = new Frame()
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                BackgroundColor = Color.White,
                Content = sendButton,
                Padding = new Thickness(0, 0, 4, 4)
            };

            editorStack = new StackLayout()
            {
                Padding = new Thickness(1),
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    textEditorFrame, verticalLine, sendButtonFrame
                },
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.EndAndExpand,
                BackgroundColor = Color.White,
            };


            messageStack = new StackLayout()
            {

                Spacing = 0,
                Padding = new Thickness(10, 5),
                VerticalOptions = LayoutOptions.Start
            };

            messageView = new ScrollView()
            {
                Content = messageStack,
                Orientation = ScrollOrientation.Vertical,
                VerticalOptions = LayoutOptions.Fill
            };

            mainStack = new StackLayout
            {
                Children =
                {
                    messageView, editorStack
                },
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                BackgroundColor = Constants.backGroundColor
            };

            if(Device.OS == TargetPlatform.iOS)
            {
                rootScrollView = new ScrollView()
                {
                    Content = mainStack,
                    HorizontalOptions = LayoutOptions.Fill,
                    VerticalOptions = LayoutOptions.Fill,
                    BackgroundColor = Constants.backGroundColor
                };

                Content = rootScrollView;
            }
            else
            {
                Content = mainStack;
            }
            
        }

        #region event listeners for SEND, EDITOR, MESSAGE FRAME
        /*
        * Called when text editor is not focused, removes keyboard and scrolls to height
        */
        private async void TextEditor_Unfocused(object sender, FocusEventArgs e)
        {
            await Task.Delay(300);
            ScrolltoBottom(false);
        }

        /*
        * Called when text editor is not focused, removes keyboard and scrolls to height
        */
        private async void TextEditor_Focused(object sender, FocusEventArgs e)
        {
            await Task.Delay(300);
            ScrolltoBottom(false);
        }

        private void SendButtonClicked(object sender, EventArgs e)
        {
            string text = textEditor.Text;
            if (String.IsNullOrWhiteSpace(text))
            {
                return;
            }
            textEditor.Text = "";
            ChatMessage msgObj = new ChatMessage()
            {
                ToFrom = Helpers.Settings.ToFromArn,
                Text = text,
                Sender = this.mainUser,
                Time = DateTime.Now.ToString("hh:mm tt"),
            };
            
            AddMessageToChat(msgObj);
            Constants.conv.msgs.Add(msgObj);
            ScrolltoBottom(false);
            PushMessage(text);
        }

        private async void MessageFrameTapped_Tapped(object sender, EventArgs e, ChatMessage msgObj)
        {
            var action = await DisplayActionSheet(null, null, null, "Hide text", "Report", "Delete Message");
            Frame frame;
            switch (action)
            {
                case "Hide text":
                    frame = sender as Frame;
                    frame.IsVisible = false;
                    break;
                case "Report":
                    reportMessage(msgObj);
                    break;
                case "Delete Message":
                    frame = sender as Frame;
                    frame.IsVisible = false;
                    deleteMessage(msgObj);
                    break;
            }
        }

        #endregion

        #region Utility functions for SCROLL, REPORT, EVENTS
        public void reportMessage(ChatMessage msg) //Enables users to report selected message based on specific criteria
        {
            Navigation.PopAsync();
            Navigation.PushAsync(new ReportMessage(msg));
        }

        private async void ScrolltoBottom(bool anim)
        {
            if(Device.OS == TargetPlatform.iOS)
            {
                await this.rootScrollView.ScrollToAsync(this.mainStack, ScrollToPosition.End, anim);
                await this.messageView.ScrollToAsync(this.messageStack, ScrollToPosition.End, anim);
            }
            else
            {
                await this.messageView.ScrollToAsync(this.messageStack, ScrollToPosition.End, anim);
            }
            
        }

        private async void waitScroll(int delay)
        {
            await Task.Delay(delay);
            ScrolltoBottom(false);
        }

        
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            this.width = width;
        }

        public void deleteMessage(ChatMessage msg) //given the ID of a message, delete said message from messages.
        {
            Constants.conv.msgs.Remove(msg);
        }

        private string checkName(string name)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                return "Anonymous";
            }
            if (name.Length > 24)
            {
                return name.Substring(0, 24);
            }
            return name;
        }

        #endregion


        public void LoadMessages() 
        {
            foreach (ChatMessage msg in Constants.conv.msgs)
            {
                AddMessageToChat(msg);
                ScrolltoBottom(false);
            }
        }

        private void AddMessageToChat(ChatMessage msgObj)
        {
            bool val;
            if (msgObj.Sender == this.mainUser)
            {
                val = true;
            }
            else
            {
                val = false;
            }
            if (msgObj.Sender != this.previousUser)
            {
                addSpace(3);
                addNameLabel(msgObj, val);
                addSpace(1);
                addSingleMessage(msgObj, val);
                this.previousUser = msgObj.Sender;
            }
            else
            {
                addSpace(1);
                addSingleMessage(msgObj, val);
            }

        }

        private void addSingleMessage(ChatMessage msgObj, bool user)
        {
            Label message = new Label()
            {
                Text = msgObj.Text,
                TextColor = Color.Black,
                FontSize = 18,
                FontFamily = "Arial",
                HorizontalTextAlignment = TextAlignment.Start,

            };
            Frame frame = new Frame()
            {
                Padding = new Thickness(5),
                Content = message,
                HorizontalOptions = user ? LayoutOptions.EndAndExpand : LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                BackgroundColor = user ? Constants.rightMessageColor : Constants.leftMessageColor,
                WidthRequest = width * 0.70,
                //HasShadow = true
            };
            TapGestureRecognizer messageFrameTapped = new TapGestureRecognizer();
            messageFrameTapped.Tapped += (s, e) => MessageFrameTapped_Tapped(s, e, msgObj);
            frame.GestureRecognizers.Add(messageFrameTapped);
            this.messageStack.Children.Add(frame);
        }

        private void addNameLabel(ChatMessage msgObj, bool user)
        {
            
            Label message = new Label()
            {
                Text = msgObj.Sender + " - " + msgObj.Time,
                TextColor = Color.Black,
                FontSize = 15,
                FontFamily = "Arial",
                FontAttributes = FontAttributes.Italic,
                HorizontalTextAlignment = user ? TextAlignment.End : TextAlignment.Start
            };
            Frame frame = new Frame()
            {
                Padding = new Thickness(5),
                Content = message,
                HorizontalOptions = user ? LayoutOptions.EndAndExpand : LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
            };
            this.messageStack.Children.Add(frame);
        }

        private void addSpace(int dist)
        {
            this.messageStack.Children.Add(new BoxView { Color = Color.Transparent, HeightRequest = dist });
        }


        


        public async void sendMessage(ChatMessage message) 
        {

            try{
                await PushMessage(message.Text);
            }
            catch(Exception E)
            {
                System.Diagnostics.Debug.WriteLine("error storing in server "+ E);
            };
        }

        

        //-------------------------Get from Server-------------------------
        public void messagesFromServer(MessageLst[] MessageList) //upon recieving a chat History log
        {
            List<ChatMessage> msgs = new List<ChatMessage>();
            foreach (MessageLst i in MessageList){
                ChatMessage message = singleMessageFromServer(i.ToFrom, i.Time, i.Text);
                AddMessageToChat(message);
                Constants.conv.msgs.Add(message);
            }
        }

        public ChatMessage singleMessageFromServer(String ToFrom, String Time, String Message)    //upon recieving a single message, add it to the log of messages.
        {
            string Sender;
            if (ToFrom.StartsWith(Constants.conv.id))
            {
                Sender = this.mainUser;
            }
            else
            {
                Sender = Constants.conv.name;
            }

            ChatMessage newMsg = new ChatMessage()
            {
                Text = Message,
                Sender = Sender,
                Time = generateLocalTime(Time),
                ToFrom = ToFrom
            };

            return newMsg;
        }

        private string generateLocalTime(string time)
        {
            DateTime dt = Convert.ToDateTime(time);
            var diff = DateTime.UtcNow - DateTime.Now;

            return (dt - diff).ToString("hh:mm tt");
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

        async private Task LoadMessagesFromServer() //Function to create a Json object and send to server using a lambda function
        {
            try
            {
                UserInfoItem message = new UserInfoItem { Item = new MessageLst { ToFrom = Helpers.Settings.ToFromArn, Time = Constants.date.ToString("yyyy-MM-dd HH:mm:ss:ffff") } }; //Helpers.Settings.EndpointArnSetting
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
                    arg.Time = generateLocalTime(arg.Time);
                }
                catch (Exception e)
                {
                    arg.Time = DateTime.Now.Hour.ToString("HH:mm");

                }
                AddMessageToChat(arg);
                Constants.conv.msgs.Add(arg);
                ScrolltoBottom(false);
            });
            MessagingCenter.Unsubscribe<App>(this, "ConversationEnd");
            MessagingCenter.Subscribe<App, ChatMessage>(this, "ConversationEnd", (sender, arg) => //adds message to log
            {
                MessagingCenter.Send<ChatPage>(this, "End");
                MessagingCenter.Unsubscribe<App>(this, "ConversationEnd");
                this.sendButton.IsEnabled = false;
                if (terminate)
                {
                    terminateChat();
                }
            });
            LoadMessages();
            await LoadMessagesFromServer();
            ScrolltoBottom(false);
        }
        
        protected override void OnDisappearing() //leaving the page ->cache history
        {
            textEditor.Keyboard = null;
            textEditor.Unfocus();
            MessagingCenter.Unsubscribe<App, ChatMessage>(this, "Hi"); 
            MessagingCenter.Send<ChatPage>(this, "End");

            Constants.date = DateTime.UtcNow;
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