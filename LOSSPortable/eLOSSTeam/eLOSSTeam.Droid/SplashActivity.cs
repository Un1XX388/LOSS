using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;


namespace eLOSSTeam.Droid
{
    [Activity(
        Label = "eLOSSTeam",
        Icon = "@drawable/App6",
        MainLauncher = true, 
        ConfigurationChanges = ConfigChanges.ScreenSize /*| ConfigChanges.Orientation*/,
        Theme = "@style/Theme.Splash",
        NoHistory = true           
        )]

    //this class displays the splash screen

    public class SplashActivity : Activity
    {

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            System.Threading.Thread.Sleep(2000);            //displays it for a few seconds before it launches the home screen
            this.StartActivity(typeof(MainActivity));          
        }


    }
}

