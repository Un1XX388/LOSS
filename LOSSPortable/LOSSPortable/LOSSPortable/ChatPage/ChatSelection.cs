using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LOSSPortable
{
    

    public class ChatSelection : ContentPage
    {
        

        private StackLayout stackLayout;
        private StackLayout outerLayout;
        List<ChatPage> Chats = new List<ChatPage>();
        List<Button> Buttons = new List<Button>();
        List <Message> chatList = new List<Message>();
        ChatPage chat1;
        ChatPage chat2;
        ChatPage chat3;
        ChatPage chat4;
        Grid gridLayout;
        

        

        public ChatSelection()
        {
            Title = "Chat Selection";
            Icon = "Accounts.png";

            Message msg = new Message("User1: ", "Test Message 1", "drawable/prof.png", "11:11" );
            chatList.Add(msg);

            Message msg2 = new Message("User2: ", "Test Message 2", "drawable/prof.png", "5:05");
            chatList.Add(msg2);

            chat1 = new ChatPage("Bob", chatList);
            chat2 = new ChatPage("Tina", chatList);
            chat3 = new ChatPage("Gene", chatList);
            chat4 = new ChatPage("Linda", chatList);
            chat1.setChat(chatList);
            chat2.setChat(chatList);
            chat3.setChat(chatList);
            chat4.setChat(chatList);

            var label = new Label { Text = "Message a Volunteer", FontSize = 30, BackgroundColor = Color.FromHex("CCCCFF"), TextColor = Color.Black, XAlign = TextAlignment.Center };


            Device.BeginInvokeOnMainThread(() =>   //automatically updates
            {
                stackLayout = new StackLayout
                {   BackgroundColor= Color.FromHex("CCCCFF"),
                    Children =
                            {
                                
                            }

                };

                outerLayout = new StackLayout
                {
                    BackgroundColor = Color.FromHex("CCCCFF"),
                    Spacing = 2,
                    Children =
                    {
                        label,
                        stackLayout

                    }
                };

                
            });

            Chats.Add(chat1);
            Chats.Add(chat2);
            Chats.Add(chat3);
            Chats.Add(chat4);

            //upon sending a message
            

            foreach (ChatPage chat in Chats)
            {
                var profilePicture = new Image { };
                profilePicture.Source = "drawable/prof2.png";

                gridLayout = new Grid
                {   ColumnSpacing = 3,
                    ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) },
                    },
                    Children =
                        {
                        }
                };

                gridLayout.HorizontalOptions = LayoutOptions.Start;
                gridLayout.VerticalOptions = LayoutOptions.Start;
                gridLayout.Children.Add(profilePicture);

                gridLayout.Children.Add(CreateButton(chat.getName(), chat),1,4,0,1);
                stackLayout.Children.Add(gridLayout);
            }
            
            

            this.Content = outerLayout;

        }

        public Button CreateButton(String name, ChatPage chat)
        {

            Button ButtonTemp = new Button { Text = "" + name, WidthRequest = 100, HeightRequest = 50, TextColor = Color.Black, BackgroundColor=Color.FromHex("6666FF"), BorderColor=Color.Black, FontAttributes = FontAttributes.Bold, Font = Font.OfSize("Arial", 22)   };
            //ButtonTemp.HorizontalOptions = LayoutOptions.Start;
            
            gridLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
            ButtonTemp.Clicked += async (s, e) =>
            {
                await Navigation.PushAsync(chat);  //navigate to a state page (not new).
            };
            return ButtonTemp;
        }
    }

}