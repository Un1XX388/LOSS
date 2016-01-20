using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LOSSPortable
{

    public class ChatPage : ContentPage
    {
        private Button send;
        List<string> Messages = new List<string>();
        private Grid gridLayout;
        private StackLayout outerStack;
        private ScrollView innerScroll;
        String name;
        List<Message> msgs = new List<Message>(); //history of messaging



        public ChatPage(String inputname, List<Message> msgs)
        {
            this.msgs = msgs;
            name = inputname;

            Title = "Chat";
            Icon = "Accounts.png";

            //entry:
            var editor = new Entry();
            editor.Placeholder = "Enter Message: ";

            var label = new Label { Text = "Message " + this.getName(), FontSize = 30, BackgroundColor = Color.Blue, TextColor = Color.White, XAlign = TextAlignment.Center };
            send = new Button { Text = "Send" };

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

                var tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.Tapped += (s, e) => {
                };

                Message message = new Message("Sender: ", mes, "drawable/pokeball.png", " " + currentTime());
                msgs.Add(message);
                DisplayResponse(message);

                this.Content = outerStack;
            };

            Device.BeginInvokeOnMainThread(() =>   //automatically updates
            {

                gridLayout = new Grid
                {
                    RowSpacing = 0,
                    ColumnSpacing = 0,
                    Padding = new Thickness(0, 0, 0, 0),
                    VerticalOptions = LayoutOptions.Start,
                    BackgroundColor = Color.White,
                    RowDefinitions = { new RowDefinition { Height = GridLength.Auto } },

                    Children =
                        {
                            label
                        }

                };

                innerScroll = new ScrollView
                {
                    VerticalOptions = LayoutOptions.Start,
                    HorizontalOptions = LayoutOptions.Start,
                    Padding = new Thickness(10, 10, 10, 20),
                    BackgroundColor = Color.White,
                    Content = gridLayout

                };

                gridLayout.ChildAdded += delegate // Automatically scrolls to bottom of the chat 
                { innerScroll.ScrollToAsync(0, innerScroll.HeightRequest * 2, true); };


                outerStack = new StackLayout
                {
                    BackgroundColor = Color.White,
                    VerticalOptions = LayoutOptions.Start,
                    Spacing = 0,
                    Children =
                            {
                                innerScroll,
                                editor,
                                send
                            }

                };

                innerScroll.HeightRequest = 440;


                Content = outerStack;
                Title = "Message a Volunteer";
            });

            foreach (Message msg in msgs) //INITIALIZE HISTORY OF MESSAGES
            {
                DisplayResponse(msg);
                //DisplayAlert("ADDING:",msg.getMessage(),"ok"); 
            }

            this.Content = outerStack;
        }

        //individual message tapped in chat:
        async void OnLabelClicked(Label label, String msg, int Type)
        {
            var action = await DisplayActionSheet(null, null, null, "Hide Text", "Report");
            switch (action)
            {
                case "Report":
                    await DisplayAlert("Alert", "HERE COMES THE BAN HAMMER", "OK");
                    break;
                case "Hide Text":
                    label.Text = "\n" + "***" + "\t";
                    break;

            }
            this.Content = outerStack;

        }


        private void DisplayResponse(Message message)
        {
            String msg = message.getMessage();
            int numRows;
            if (msg == null || msg == "")
            {
                return;
            }
            else
            {
                numRows = msg.Length / 45;
            }

            Grid innerGrid = new Grid
            {
                BackgroundColor = Color.White,
                VerticalOptions = LayoutOptions.Start,
                Padding = 1,
                RowSpacing = 1,
                RowDefinitions = {
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(numRows+1, GridUnitType.Auto) }
                },
                ColumnDefinitions = {
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) },
            }


            };




            var profilePicture = new Image { };
            profilePicture.Source = message.getIcon();
            profilePicture.VerticalOptions = LayoutOptions.StartAndExpand;
            profilePicture.BackgroundColor = Color.White;
            innerGrid.Children.Add(profilePicture);


            Label name = new Label { Text = message.getSender(), BackgroundColor = Color.White, TextColor = Color.Purple, FontAttributes = FontAttributes.Bold }; //, XAlign = TextAlignment.Start
            name.VerticalOptions = LayoutOptions.StartAndExpand;
            innerGrid.Children.AddHorizontal(name);

            var datetime = DateTime.Now;
            Label time = new Label { Text = message.getTime() };
            innerGrid.Children.AddHorizontal(time);


            Label response = new Label { Text = message.getMessage(), BackgroundColor = Color.White, TextColor = Color.Purple, FontAttributes = FontAttributes.Bold }; //, XAlign = TextAlignment.Start             
            var tgr = new TapGestureRecognizer();
            tgr.Tapped += (s, e) => OnLabelClicked(response, msg, 1);
            response.GestureRecognizers.Add(tgr);
            response.VerticalOptions = LayoutOptions.StartAndExpand;
            response.BackgroundColor = Color.White;
            int labelLength = 2 + numRows;


            innerGrid.RowSpacing = 1;


            innerGrid.Children.Add(response, 0, 5, 1, labelLength);
            gridLayout.RowSpacing = 1;
            gridLayout.Children.AddVertical(innerGrid);

            gridLayout.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            //add buttons to grid, push that grid to the stack
        }

        public String getName()
        {
            return name;
        }

        public void setChat(List<Message> msgs)
        {
            this.msgs = msgs;
        }

        public String currentTime()
        {
            var datetime = DateTime.Now;
            String minutes = "" + datetime.Minute;
            if (minutes.Length == 1)
            { minutes = "0" + minutes; }

            return "" + datetime.Hour + ":" + minutes;

        }


    }


}