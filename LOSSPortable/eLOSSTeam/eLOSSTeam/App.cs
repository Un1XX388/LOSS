using System;
using Xamarin.Forms;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace eLOSSTeam
{
    public class App : Application
    {
        public static int ScreenWidth;
        public static int ScreenHeight;

		public Boolean HardwareAccelerated { get; set; }
        public Boolean ChatPageActive { get; set; }
        public Boolean ChatSelectionPageActive { get; set; }

        public App()
        {
            MainPage = new eLOSSTeam.RootPage();
			HardwareAccelerated	= true;
            ChatPageActive = false;
        }

        protected override void OnStart()
        {
            Constants.date = DateTime.UtcNow.AddDays(-1);
            try
            {
                AmazonUtils.updateInspirationalQuoteList();
                AmazonUtils.updateOnlineRList();
                AmazonUtils.updateOnlineVList();
                AmazonUtils.updateOnlinePlaylist();
                AmazonUtils.updateMiscellaneousList();
            }
            catch(Exception e)
            {
                
            }
            
            ChatPageActive = false;
            ChatSelectionPageActive = false;
            MessagingCenter.Subscribe<ChatPage>(this, "Start", (sender) =>
            {
                ChatPageActive = true;
            });
            MessagingCenter.Subscribe<ChatPage>(this, "End", (sender) =>
            {
                ChatPageActive = false;
            });
            MessagingCenter.Subscribe<ChatSelection>(this, "Start", (sender) =>
            {
                ChatSelectionPageActive = true;
            });
            MessagingCenter.Subscribe<ChatSelection>(this, "End", (sender) =>
            {
                ChatSelectionPageActive = false;
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
        private static string oldTime = "";
        public void parseMessageObject(string msg)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                System.Diagnostics.Debug.WriteLine(msg);
                SNSMessagePCL tmp = JsonConvert.DeserializeObject<SNSMessagePCL>(msg);
                if (oldTime.Equals(tmp.Time))
                {
                    return;
                }else
                {
                    oldTime = tmp.Time;
                }
                ChatMessage message = new ChatMessage { ToFrom = tmp.ToFrom, Time = tmp.Time, Text = tmp.Text, Sender = tmp.Sender };
                if (tmp.Subject == "Handshake")
                {
                    MessagingCenter.Send<App, ChatMessage>(this, "Handshake", message);
                }
                else if (tmp.Subject == "HandshakeEnd")
                {
                    if (ChatSelectionPageActive || ChatPageActive) {
                        MessagingCenter.Send<App, ChatMessage>(this, "HandshakeEnd", message);
                    }
                    if (ChatPageActive)
                    {
                        MessagingCenter.Send<App, ChatMessage>(this, "ConversationEnd", message);
                    }
                    
                }
                else
                {   
                    MessagingCenter.Send<App, ChatMessage>(this, "Hi", message);
                }
            });
            
        }
    }
}

