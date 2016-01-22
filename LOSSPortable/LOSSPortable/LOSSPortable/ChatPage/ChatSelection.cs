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
        

        

        public ChatSelection()
        {
            Title = "Chat Selection";
            Icon = "Accounts.png";

            Message msg = new Message("Obama: ", "I AM DIABLO", "drawable/pokeball.png", "11:11");
            chatList.Add(msg);

            Message msg2 = new Message("Michelle: ", "I AM TOO", "drawable/pokeball.png", "5:05");
            chatList.Add(msg2);

            chat1 = new ChatPage("Janeth", chatList);
            chat2 = new ChatPage("Rafael", chatList);
            chat3 = new ChatPage("Ana", chatList);
            chat4 = new ChatPage("Anaiza", chatList);
            chat1.setChat(chatList);
            chat2.setChat(chatList);
            chat3.setChat(chatList);
            chat4.setChat(chatList);

            var label = new Label { Text = "Message a Volunteer", FontSize = 30, BackgroundColor = Color.Blue, TextColor = Color.White, XAlign = TextAlignment.Center };


            Device.BeginInvokeOnMainThread(() =>   //automatically updates
            {
                stackLayout = new StackLayout
                {
                    Children =
                            {
                                
                            }

                };

                outerLayout = new StackLayout
                {
                    Spacing = 0,
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
                stackLayout.Children.Add(CreateButton(chat.getName(), chat));
            }
            
            

            this.Content = outerLayout;

        }

        public Button CreateButton(String name, ChatPage chat)
        {
            Button ButtonTemp = new Button { Text = "Chat " + name };
            ButtonTemp.Clicked += async (s, e) =>
            {
                await Navigation.PushAsync(chat);  //navigate to a state page (not new).
            };
            return ButtonTemp;
        }
    }

 /*     next to fix:    
        -holding chat-> option to remove it
        -in chat-> update page so bottom message is shown
        -add images of people
*/
}