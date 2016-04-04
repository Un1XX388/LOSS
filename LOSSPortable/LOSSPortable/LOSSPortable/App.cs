using System;
using Xamarin.Forms;

namespace LOSSPortable
{
    public class App : Application
    {

		public Boolean HardwareAccelerated { get; set; }


        public App()
        {
            MainPage = new LOSSPortable.RootPage();
			HardwareAccelerated	= true;
        }

        protected override void OnStart()
        {
            AmazonUtils.updateInspirationalQuoteList();
            AmazonUtils.updateOnlineRList();

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


    }
}

