using System;
using Xamarin.Forms;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace LOSSPortable
{
    public class App : Application
    {
        public static int ScreenWidth;
        public static int ScreenHeight;

		public Boolean HardwareAccelerated { get; set; }
        public Boolean ChatPageActive { get; set; }

        public App()
        {
            MainPage = new LOSSPortable.RootPage();
			HardwareAccelerated	= true;
            ChatPageActive = false;
        }

        protected override void OnStart()
        {
            AmazonUtils.updateInspirationalQuoteList();
            AmazonUtils.updateOnlineRList();
            MessagingCenter.Subscribe<ChatPage>(this, "Start", (sender) =>
            {
                ChatPageActive = true;
            });
            MessagingCenter.Subscribe<ChatPage>(this, "End", (sender) =>
            {
                ChatPageActive = false;
            });
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes

        }

        public Boolean chatDisplayed()
        {
            return ChatPageActive;
        }


        /*
            {
            "Seen": "False",
            "Text": "Hello",
            "Time": "2016-04-06 16:38:08.618757",
            "ToFrom": "U-79854921478565921900#R-6031175061964857259"
            }
         */
        public void parseMessageObject(string msg)
        {
            SNSMessage tmp = JsonConvert.DeserializeObject<SNSMessage>(msg);
            ChatMessage message = new ChatMessage { ToFrom = tmp.ToFrom, Time = tmp.Time, Text = tmp.Text, Sender = tmp.Sender };
            MessagingCenter.Send<App, ChatMessage>(this, "Hi", message);
        }
    }
}

