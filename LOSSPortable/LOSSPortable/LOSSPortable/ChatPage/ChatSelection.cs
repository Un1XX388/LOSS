using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Threading.Tasks;
using Akavache;
using System.Reactive.Linq;
using System.Threading;





namespace LOSSPortable
{


    public class ChatSelection : ContentPage
    {


        private String akKEY = "Cached2";   //key 
        private StackLayout stackLayout;
        private StackLayout outerLayout;
        private ScrollView innerScroll;
        List<ChatPage> Chats = new List<ChatPage>();
        List<String> ChatKey = new List<String>();
        //List<Button> Buttons = new List<Button>();
        List<Label> ChatLabel = new List<Label>();
        List<Message> chatList = new List<Message>();
        ChatPage chat1;
        int Chatid = 10000;


        Grid gridLayout;
        Contacts cont;
        Message msg;



        public ChatSelection()
        {


            if (Helpers.Settings.ContrastSetting == true)
            {
                BackgroundColor = Colors.contrastBg;
            }
            else
            {
                BackgroundColor = Colors.background;

            }

            cont = NewCont();

            Title = "Chat Selection";
            Icon = "Accounts.png";

            msg = new Message();
            msg = mesCons(msg, "123", "drawable/prof.png", "User1: ", "Test Message 1", "11:11");

            chatList.Add(msg);

            Message msg2 = new Message();
            msg2 = mesCons(msg2, "456", "drawable/prof.png", "User2: ", "Test Message 2", "2:41");
            chatList.Add(msg2);

            chat1 = new ChatPage("Bob", chatList, "00000");

            var label = new Label { Text = "Message a Volunteer", FontSize = 30, TextColor = Color.Black, XAlign = TextAlignment.Center };

            var editor = new Entry();
            editor.Placeholder = "Enter new volunteer: ";
            Button addV = new Button { Text = "Add a Volunteer", WidthRequest = 100, HeightRequest = 50, TextColor = Color.Black, BackgroundColor = Color.FromHex("B3B3B3"), BorderColor = Color.Black, FontAttributes = FontAttributes.Bold, Font = Font.OfSize("Arial", 22) };
            System.Diagnostics.Debug.WriteLine("finished caching.");

            addV.Clicked += delegate
            {   //create new chagpage, add it to list, refresh chatselection page

                ChatPage tempC = new ChatPage(editor.Text, chatList, "00001");
                Chats.Add(tempC);
                stackLayout.Children.Clear();
                ChatKey.Add("00001");
                cont.conv.Add("" + (Chatid++));
                System.Diagnostics.Debug.WriteLine("Added page to cached obj: " + Chatid);
                displayChats();

                this.Content = outerLayout;
            };


            Device.BeginInvokeOnMainThread(() =>   //automatically updates
            {
                stackLayout = new StackLayout
                {
                    Children =
                            {

                            }

                };

                innerScroll = new ScrollView
                {
                    VerticalOptions = LayoutOptions.Start,
                    HorizontalOptions = LayoutOptions.Start,
                    Padding = new Thickness(5, 5, 5, 10),
                    Content = stackLayout

                };

                outerLayout = new StackLayout
                {
                    Spacing = 2,
                    Children =
                    {
                        label,
                        innerScroll,
                        editor,
                        addV

                    }
                };


            });


            displayChats();

        }

        public Button CreateButton(String name, ChatPage chat)
        {

            Button ButtonTemp = new Button { Text = "" + name, WidthRequest = 100, HeightRequest = 50, TextColor = Color.Black, BackgroundColor = Color.FromHex("B3B3B3"), BorderColor = Color.Black, FontAttributes = FontAttributes.Bold, Font = Font.OfSize("Arial", 22) };
            //ButtonTemp.HorizontalOptions = LayoutOptions.Start;

            gridLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
            ButtonTemp.Clicked += async (s, e) =>
            {
                await Navigation.PushAsync(chat);  //navigate to a state page (not new).
            };
            return ButtonTemp;
        }

        public Message mesCons(Message message, String id, String icon, String sender, String text, String time)
        {
            message.id = id;
            message.icon = icon;
            message.sender = sender;
            message.text = text;
            message.time = time;
            return message;
        }
        async void saveMsg(Message message)
        {
            //await App.PManager.SaveTaskAsync(message);
        }

        public void displayChats()
        {
            foreach (ChatPage chat in Chats)
            {
                var profilePicture = new Image { };
                profilePicture.Source = "drawable/prof2.png";
                profilePicture.BackgroundColor = Color.White;

                gridLayout = new Grid   //grid is for a single image-chat button
                {
                    ColumnSpacing = 3,
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

                gridLayout.Children.Add(CreateButton(chat.getName(), chat), 1, 4, 0, 1);
                stackLayout.Children.Add(gridLayout);
            }



            this.Content = outerLayout;
        }

        //------------------------------------------------------------------------
        protected override Boolean OnBackButtonPressed() // back button pressed
        {
            ((RootPage)App.Current.MainPage).NavigateTo();
            
            return true;
        }

        protected async override void OnDisappearing() //leaving the page ->cache history
        {
            base.OnDisappearing();



            //cont.conv.Add( ChatKey);
            System.Diagnostics.Debug.WriteLine("Trying to store cache.");
            if (cont.conv.Count != 0)
            {
                await Store(akKEY, cont);
            }

            else {
                System.Diagnostics.Debug.WriteLine("Nothing to cache.");
            }

        }


        protected async override void OnAppearing() //read cache
        {
            base.OnAppearing();
            System.Diagnostics.Debug.WriteLine("Trying to get cache content");
            Chats.Clear();

            //using Properties dictionary:
            /*if (Application.Current.Properties.ContainsKey("123456789"))
            {
                cont = Application.Current.Properties["123456789"] as Contacts;
                await DisplayAlert("name", "> ---","ok");

            }
            else {
                await DisplayAlert("Cache","cache empty","---");
            }
            System.Diagnostics.Debug.WriteLine("Got cache content");
*/

            this.stackLayout.Children.Clear();

            cont = await Get(akKEY);

            System.Diagnostics.Debug.WriteLine("Cache accessed successfuly");


            if (cont.conv.Count == 0)
            {
                System.Diagnostics.Debug.WriteLine("NOTHING TAKEN FROM THE CACHE");
                //ChatPage tempC2 = new ChatPage("Temp", chatList, "00000");
                //Chats.Add(tempC2);
            }
            else {
                foreach (String chat in cont.conv)
                {
                    Chats.Add(new ChatPage("" + chat, chatList, chat));
                }

            }
            displayChats();
            this.Content = outerLayout;


            /*

            var cancellationTokenSource = new CancellationTokenSource();
            Task.Factory.StartNew(  await onAppearing()
                                  , TaskCreationOptions.LongRunning
                                  , cancellationTokenSource.Token);
        */
        }






        public async Task<Contacts> Get(string key)
        {

            try
            {
                /*IEnumerable<Contacts> cacheCont = await BlobCache.LocalMachine.GetAllObjects<Contacts>();
                System.Diagnostics.Debug.WriteLine("Loop begins");
                foreach (Contacts cached in cacheCont) 
                    {
                    System.Diagnostics.Debug.WriteLine("looping: " + cached.conv.Count);
                    }*/


                System.Diagnostics.Debug.WriteLine("fetching cached object");
                return await BlobCache.LocalMachine.GetOrCreateObject<Contacts>(key, NewCont);

            }
            catch (KeyNotFoundException)
            {
                System.Diagnostics.Debug.WriteLine("error");
                return new Contacts();

            }


        }



        //need to consider removing the async: use Task.run()
        //http://stackoverflow.com/questions/31425210/akavaches-getobjectt-hangs-when-awaited-any-idea-what-is-wrong-here

        public async Task Store<Contacts>(string key, Contacts value)
        {
            System.Diagnostics.Debug.WriteLine("storing " + cont.conv.Count);
            try
            {
                await BlobCache.LocalMachine.InsertObject(key, cont);
            }
            catch (InvalidOperationException)
            {
                System.Diagnostics.Debug.WriteLine("error");
            }

            System.Diagnostics.Debug.WriteLine("finished storing");
        }


        public Contacts NewCont()
        {
            System.Diagnostics.Debug.WriteLine("creating new conv");
            return new Contacts();
        }

    }

}