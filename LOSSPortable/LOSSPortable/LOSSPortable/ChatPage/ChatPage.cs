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


namespace LOSSPortable
{

    public class ChatPage : ContentPage
    {
        
        CancellationTokenSource ct = new CancellationTokenSource();
        
        //purpose is to allow only one end conversation alert
        private static Boolean terminate; 

        Button sendButton;
        Editor textEditor;
        ScrollView messageView;
        StackLayout mainStack;
        StackLayout messageStack;
        StackLayout editorStack;

        //store width of the screen
        double width;
        //Keeps track of who the most recent message is from so that the header label can be properly displayed
        string previousUser = "";
        // display name of current user
        string mainUser;
        // display name of user, current user is conversing with
        string convWithUser;


        public ChatPage()
        {
            terminate = true;

            mainUser = checkName(Helpers.Settings.DisplayName);
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

            //Text entry for messages, height request is approximentally two lines of text
            textEditor = new Editor()
            {
                Keyboard = Keyboard.Default,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HeightRequest = 50,
                TextColor = Color.Purple,
            };
            //Called when keyboard appears/disappears (300 value is hardcoded to scroll the text up/down when the keyboard appears)
            textEditor.Focused += (s, e) => TextEditor_Focused(s, e, messageView.ScrollY + 300);
            textEditor.Unfocused += (s, e) => TextEditor_Unfocused(s, e, messageView.ScrollY - 300);

            //Frame that contains the text editor
            Frame textEditorFrame = new Frame()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                BackgroundColor = Color.White,
                Content = textEditor,
                Padding = new Thickness(8, 1, 1, 1)
            };

            //Adds seperator between chat text entry box and send button
            BoxView verticalLine = new BoxView()
            {
                WidthRequest = 2,
                HeightRequest = 40,
                Color = Color.Purple,
            };

            sendButton = new Button()
            {
                Text = ">",
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

            //contains the text editor, seperator and send button
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

            //stack that contains all the chat messages
            messageStack = new StackLayout()
            {

                Spacing = 0,
                Padding = new Thickness(10, 5),
                VerticalOptions = LayoutOptions.Start
            };
            //all messages are contained in this scrollview
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

            Content = mainStack;
        }

        #region event listeners for SEND, EDITOR, MESSAGE FRAME
        /*
        * Called when text editor is not focused, removes keyboard and scrolls to height
        */
        private async void TextEditor_Unfocused(object sender, FocusEventArgs e, double height)
        {
            await Task.Delay(300);
            await this.messageView.ScrollToAsync(0, height, false);
        }

        /*
        * Called when text editor is not focused, removes keyboard and scrolls to height
        */
        private async void TextEditor_Focused(object sender, FocusEventArgs e, double height)
        {
            await Task.Delay(300);
            await this.messageView.ScrollToAsync(0, height, false);
        }

        /*
         * Called when send button clicked, if text is whitespace or empty, returns empty
         * Otherwise, creates new ChatMessage object, adds it to display, static storage
         * pushes to server before scrolling to bottom of chat view.
         */
        private async void SendButtonClicked(object sender, EventArgs e)
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
            ScrolltoBottom(true);
            await PushMessage(text);
        }

        /*
         * Called when a chat message in the scroll view is tapped
         * Listener is established for each message containing the ChatMessage
         * used to display the message. 
         * Options include "Hide text", "Report", and "Delete Message"
         * Hide text - temporarily hides message till nexts reload of page
         * Delete message - deletes the message from the local cache (still located on server)
         * Report message - sends Chat Message object to report page for content reporting
         */

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

        //Removes current chat page from stack and creates new ReportMessage page with selected MessageObject
        private void reportMessage(ChatMessage msg)
        {
            Navigation.PopAsync();
            Navigation.PushAsync(new ReportMessage(msg));
        }

        /*
         * async tasks that scrolls to bottom of page with boolean parameter animate scroll
         */
        private async void ScrolltoBottom(bool anim)
        {
            await this.messageView.ScrollToAsync(this.messageStack, ScrollToPosition.End, anim);
        }
        /*
         * Scrolls to bottom of page without animation after a specified delay
         */
        private async void waitScroll(int delay)
        {
            await Task.Delay(delay);
            ScrolltoBottom(false);
        }

        /*
         * gets the width of the screen when the size is allocated to the page
         */
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            this.width = width;
        }

        /*
         * removes message from local cache (not server)
         */
        public void deleteMessage(ChatMessage msg) //given the ID of a message, delete said message from messages.
        {
            Constants.conv.msgs.Remove(msg);
        }

        /*
         * function to check that a display name fits conformity
         */
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


        #region Chat display methods
        
        /*
         * driver function for adding a chat message to the display screen
         * determines if message is from main user or other user, and if a header
         * needs to be added
         */
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

        /*
         * adds single message label to screen, which side is determiend by bool user
         * message is stored in frame and has attached an on tapped listener
         */
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

        /*
         * Adds header label with user name and time above certain messages
         */
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
        /*
         * Adds a space between two messages with the given distance
         */
        private void addSpace(int dist)
        {
            this.messageStack.Children.Add(new BoxView { Color = Color.Transparent, HeightRequest = dist });
        }

        #endregion

        /* 
         * Load messages from local cache into chat screen
         */
        public void LoadMessages()
        {
            foreach (ChatMessage msg in Constants.conv.msgs)
            {
                AddMessageToChat(msg);
            }
        }

        /*
         * iterates through messages recieved from server, parses them into ChatMessage
         * and then adds them to local cache and displays the message.
         */
        public void messagesFromServer(MessageLst[] MessageList) //upon recieving a chat History log
        {
            List<ChatMessage> msgs = new List<ChatMessage>();
            foreach (MessageLst i in MessageList){
                ChatMessage message = singleMessageFromServer(i.ToFrom, i.Time, i.Text);
                AddMessageToChat(message);
                Constants.conv.msgs.Add(message);
            }
        }

        /*
         * Parses a single server message into a chat message, determines who the sender was
         * of the original messaging by comparing the user ID with the beginning of the ToFrom
         * field.
         */
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
        /*
         * generates the local time from the server UTC provided time
         * format is hh:mm tt (04:52 pm) for example
         */
        private string generateLocalTime(string time)
        {
            DateTime dt = Convert.ToDateTime(time);
            var diff = DateTime.UtcNow - DateTime.Now;

            return (dt - diff).ToString("hh:mm tt");
        }

        /*
         * Sends message to server
         */
        public async void sendMessage(ChatMessage message)
        {

            try
            {
                await PushMessage(message.Text);
            }
            catch (Exception E)
            {
                System.Diagnostics.Debug.WriteLine("error storing in server " + E);
            };
        }

        
        /*
         * method that creates json object from chat message that is then sent to the server
         */
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


        /*
         * Loads all the messages from server from a given conversation between two users after a specific time
         * returns results in MessageList, each message is then processed by helper functions.
         */
        async private Task LoadMessagesFromServer()
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

        /*
         * Called when the screen becomes focused again. Reattaches the Messaging Center listeners
         * for recieving new mesages and notifying other classes. Also, loads messages from cache and server and scrolls to bottom of page
         */
        protected async override void OnAppearing() //Functionality to start the page that reads the history from the server and displays the log.
        {
            base.OnAppearing();
            

            MessagingCenter.Send<ChatPage>(this, "Start");
            
            //handles incoming messages from SNS
            MessagingCenter.Unsubscribe<App, ChatMessage>(this, "Hi");
            MessagingCenter.Subscribe<App, ChatMessage>(this, "Hi", (sender, arg) => //adds message to log
            {
                try
                {
                    arg.Time = generateLocalTime(arg.Time);
                }
                catch (Exception e)
                {
                    arg.Time = DateTime.Now.Hour.ToString("hh:mm tt");

                }
                AddMessageToChat(arg);
                Constants.conv.msgs.Add(arg);
                waitScroll(100);
            });

            //Handles the event for when the companion user terminates the conversation
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
        
        /* 
         * Called when the page loses the focus, hides keyboard, changes the date for future server query
         */
        protected override void OnDisappearing()
        {
            textEditor.Keyboard = null;
            textEditor.Unfocus();
            MessagingCenter.Unsubscribe<App, ChatMessage>(this, "Hi"); 
            MessagingCenter.Send<ChatPage>(this, "End");

            Constants.date = DateTime.UtcNow;
            base.OnDisappearing();
        }

        /*
         * Called when the other user terminates chat, display pop up message notifying user that other uesr
         * has left the chat.
         */
        private async void terminateChat()
        {
            
            if (terminate) { 
                terminate = false; 
                await DisplayAlert("Conversation ended", "User has left the conversation", "OK");
            }   
        }
    }
}