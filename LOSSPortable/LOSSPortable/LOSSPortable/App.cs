using System;
using Xamarin.Forms;

namespace LOSSPortable
{
    public class App : Application
    {
        public App()
        {
            // The root page of your application

            //Application.Current.Resources = new ResourceDictionary();

            //var stackLayoutStyle = new Style(typeof(StackLayout))
            //{
            //    Setters =
            //    {
            //        new Setter {
            //            Property = StackLayout.BackgroundColorProperty, Value = Color.Black}
            //    }
            //};
            //Application.Current.Resources.Add("key", stackLayoutStyle);
//            loadSettings();
            MainPage = new LOSSPortable.RootPage();
        }

        protected override void OnStart()
        {
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

