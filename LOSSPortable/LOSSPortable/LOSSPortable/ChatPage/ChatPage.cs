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

            //var label = new Label { Text = "Message " + this.getName(), FontSize = 30, BackgroundColor = Color.Blue, TextColor = Color.White, XAlign = TextAlignment.Center };
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

                Message message = new Message("Sender: ", mes, "drawable/prof.png", " " + currentTime() );
                msgs.Add(message);
                DisplayResponse(message);

                this.Content = outerStack;
            };

            Device.BeginInvokeOnMainThread(() =>   //automatically updates
            {

                gridLayout = new Grid
                {
                    RowSpacing = 3,
                    ColumnSpacing = 0,
                    Padding = new Thickness(0, 0, 0, 0),
                    VerticalOptions = LayoutOptions.Start,
                    BackgroundColor = Color.FromHex("CCCCFF"),
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
                    Padding = new Thickness(10, 10, 10, 20),
                    BackgroundColor = Color.FromHex("CCCCFF"),
                    Content = gridLayout

                };

                gridLayout.ChildAdded += delegate // Automatically scrolls to bottom of the chat 
                { innerScroll.ScrollToAsync(0, innerScroll.HeightRequest * 2, true); };


                outerStack = new StackLayout
                {
                    BackgroundColor = Color.FromHex("CCCCFF"),
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
                Title = "" + this.getName();
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
                    await DisplayAlert("Alert", "Reporting not implemented yet.", "OK");
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
                numRows = msg.Length / 30;
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

            if (message.getSender() == "User1: ")
            { innerGrid.BackgroundColor = Color.FromHex("9999ff"); }
            else
            { innerGrid.BackgroundColor = Color.FromHex("bf80ff"); }





            var profilePicture = new Image { };
            profilePicture.Source = message.getIcon();
            profilePicture.VerticalOptions = LayoutOptions.StartAndExpand;
            //profilePicture.BackgroundColor = Color.FromHex("CCCCFF");
            

            Label name = new Label { Text = message.getSender(), TextColor = Color.White, FontAttributes = FontAttributes.Bold, Font = Font.OfSize("Arial", 20) }; //, XAlign = TextAlignment.Start
            name.VerticalOptions = LayoutOptions.StartAndExpand;

            var datetime = DateTime.Now;
            Label time = new Label { Text = message.getTime(), TextColor = Color.White, Font = Font.OfSize("Arial", 20) };
            


            Label response = new Label { Text = message.getMessage(), TextColor = Color.White, Font = Font.OfSize("Arial", 18) }; //, XAlign = TextAlignment.Start             
            var tgr = new TapGestureRecognizer();
            
            /*if (message.getSide() == "right")
            {   
                name.HorizontalTextAlignment = TextAlignment.End;
                time.HorizontalTextAlignment = TextAlignment.End;
                response.HorizontalTextAlignment = TextAlignment.End;
            }
            */
            tgr.Tapped += (s, e) => OnLabelClicked(response, msg, 1);
            response.GestureRecognizers.Add(tgr);
            response.VerticalOptions = LayoutOptions.Start;
            int labelLength = 2 + numRows;


            innerGrid.RowSpacing = 1;

            
            gridLayout.RowSpacing = 1;
            /*if (message.getSide() == "right")
            {
                profilePicture.HorizontalOptions = LayoutOptions.End;
                name.HorizontalTextAlignment = TextAlignment.End;
                name.HorizontalOptions = LayoutOptions.End;
                time.HorizontalTextAlignment = TextAlignment.End;
                response.HorizontalOptions = LayoutOptions.End;
                response.HorizontalTextAlignment = TextAlignment.End;

                innerGrid.HorizontalOptions = LayoutOptions.End;
                innerGrid.BackgroundColor = Color.White;

                innerGrid.Children.Add(time); 
                innerGrid.Children.AddHorizontal(name);
                innerGrid.Children.AddHorizontal(profilePicture);
                
                
                innerGrid.Children.Add(response, 0, 5, 1, labelLength);
            }*/
                innerGrid.Children.Add(profilePicture);
                innerGrid.Children.AddHorizontal(name);
                innerGrid.Children.AddHorizontal(time);
                innerGrid.Children.Add(response, 0, 5, 1, labelLength);
            
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