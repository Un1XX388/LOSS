using System;
using Xamarin.Forms;

namespace LOSS
{
	public class App : Xamarin.Forms.Application
	{
		public App ()
		{
            MainPage = new RootPage();
		}

        protected override void OnStart()
        {
            base.OnStart();

        }

        protected override void OnSleep()
        {

        }

        protected override void OnResume()
        {

        }
	}
}