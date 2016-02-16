using Xamarin.Forms;

namespace LOSSPortable
{
    public class App : Application
    {
        public static ParseManager PManager { get; set; }

        public App()
        {
            // The root page of your application
            Application.Current.Resources = new ResourceDictionary();


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
