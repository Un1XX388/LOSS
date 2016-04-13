using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Acr.UserDialogs;
using Plugin.TextToSpeech;
using LOSSPortable;
using LOSSPortable.Helpers;

namespace LOSSPortable.Droid
{
    [Activity(
       // MainLauncher = true, 
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
        Theme = "@style/DefaultTheme"
        )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            
            global::Xamarin.Forms.Forms.Init(this, bundle);
            UserDialogs.Init(this);

            // SetTheme(Resource.Style.DefaultTheme);

            // CrossTextToSpeech.Current.Init();
            //if (Settings.ContrastSetting == true)
            //{
            //    SetTheme(Resource.Style.ContrastTheme);

            //}
            //else
            //{
            //    SetTheme(Resource.Style.DefaultTheme);
            //}

            //screen size for idan:
            App.ScreenWidth = (int)(Resources.DisplayMetrics.WidthPixels / Resources.DisplayMetrics.Density);
            App.ScreenHeight = (int)(Resources.DisplayMetrics.HeightPixels / Resources.DisplayMetrics.Density);

            LoadApplication(new App());

            RegisterForGCM();
        }

        private void RegisterForGCM()
        {
            string senders = Constants.GoogleConsoleProjectId;
            Intent intent = new Intent("com.google.android.c2dm.intent.REGISTER");
            intent.SetPackage("com.google.android.gsf");
            intent.PutExtra("app", PendingIntent.GetBroadcast(this, 0, new Intent(), 0));
            intent.PutExtra("sender", senders);
            StartService(intent);
        }
    }
}

