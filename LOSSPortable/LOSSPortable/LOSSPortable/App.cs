using System;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace LOSSPortable
{
    public class App : Application
    {
        public static ParseManager PManager { get; set; }
        OptionsPage temp = new OptionsPage();

        public App()
        {

            // The root page of your application


            //Application.Current.Resources = new ResourceDictionary();


            //Current.Resources.Add("BarColor", Color.FromRgb(121, 248, 81));
            //var navigationStyle = new Style(typeof(NavigationPage));

            //if (Helpers.Settings.ContrastSetting == true)
            //{
            //    barBackgroundColorSetter = new Setter { Property = NavigationPage.BarBackgroundColorProperty, Value = Color.Black };

            //}
            //else
            //{
            //    barBackgroundColorSetter = new Setter { Property = NavigationPage.BarBackgroundColorProperty, Value = Colors.barBackground };

            //}


 
            MainPage = new LOSSPortable.RootPage();
            //    MainPage.BackgroundColor = Color.FromHex("3A263C");

        }

        protected override void OnStart()
        {
            // Handle when your app starts



            //if (Helpers.Settings.FirstTimeSetting == true)
            //{
            //    temp.defaultSetting();
            //    Helpers.Settings.FirstTimeSetting = false;
            //    System.Diagnostics.Debug.WriteLine("FIRST TIME");
            //}
            //else
            //{
            //    Helpers.Settings.FirstTimeSetting = false;

            //    temp.loadSettings();
            //}
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
            // Helpers.Settings.FirstTimeSetting = false;

            temp.loadSettings();

        }
    }
}
