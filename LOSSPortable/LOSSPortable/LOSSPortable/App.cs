using System;
using Xamarin.Forms;

namespace LOSSPortable
{
    public class App : Application
    {

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

        public void displaySNS(string message){
            Device.BeginInvokeOnMainThread(async () =>
            {
                ChatMessage msg = new ChatMessage { ToFrom = "ToFrom", Text = "Text", Time = "TIME"};
                MessagingCenter.Send<App, ChatMessage>(this, "Hi", msg);
                //System.Diagnostics.Debug.WriteLine("Message : " + message);
                //var note = new ViewNote(message);
                //await MainPage.Navigation.PushModalAsync(note);
            });
        }
    }
}

