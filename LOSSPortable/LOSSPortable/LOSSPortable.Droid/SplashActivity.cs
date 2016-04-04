using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;


namespace LOSSPortable.Droid
{
    [Activity(
        Label = "Loss App",
        Icon = "@drawable/App6",
        MainLauncher = true, 
        ConfigurationChanges = ConfigChanges.ScreenSize /*| ConfigChanges.Orientation*/,
        Theme = "@style/Theme.Splash",
        NoHistory = true
        )]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            System.Threading.Thread.Sleep(2000);
            this.StartActivity(typeof(MainActivity));          
        }


    }
}

