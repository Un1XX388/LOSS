using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content;
using Android.Util;
using Android.Text;
using System;

namespace LOSSPortable.Droid
{
    [Service]
    public class GCMIntentService : IntentService
    {
        static PowerManager.WakeLock sWakeLock;
        static object LOCK = new object();

        public static void RunIntentInService(Context context, Intent intent)
        {
            lock (LOCK)
            {
                if (sWakeLock == null)
                {
                    // This is called from BroadcastReceiver, there is no init.
                    var pm = PowerManager.FromContext(context);
                    sWakeLock = pm.NewWakeLock(
                    WakeLockFlags.Partial, "My WakeLock Tag");
                }
            }

            sWakeLock.Acquire();
            intent.SetClass(context, typeof(GCMIntentService));
            context.StartService(intent);
        }

        protected override void OnHandleIntent(Intent intent)
        {
            try
            {
                Context context = this.ApplicationContext;
                string action = intent.Action;

                if (action.Equals("com.google.android.c2dm.intent.REGISTRATION"))
                {
                    HandleRegistration(intent);
                }
                else if (action.Equals("com.google.android.c2dm.intent.RECEIVE"))
                {
                    HandleMessage(intent);
                }
            }
            finally
            {
                lock (LOCK)
                {
                    //Sanity check for null as this is a public method
                    if (sWakeLock != null)
                        sWakeLock.Release();
                }
            }
        }


        //Only function different from specific code on Amazon website
        private void HandleRegistration(Intent intent)
        {
            string registrationId = intent.GetStringExtra("registration_id");
            string error = intent.GetStringExtra("error");
            string unregistration = intent.GetStringExtra("unregistered");

            if (string.IsNullOrEmpty(error)) {
                AmazonUtils.RegisterDevice(AmazonUtils.Platform.Android, registrationId);
            }    
        }

        private void HandleMessage(Intent intent)
        {
            string message = string.Empty;
            Bundle extras = intent.Extras;
            foreach (string key in extras.KeySet()){
                string value = extras.GetString(key);
                System.Diagnostics.Debug.WriteLine("key : " + key + " || content : " + value);

            }
            if (!string.IsNullOrEmpty(extras.GetString("message")))
            {
                message = extras.GetString("message");
            }
            else
            {
                message = extras.GetString("default");
            }

            //Log.Info("Messages", "message received = " + message);
            var current = (App)Xamarin.Forms.Application.Current;
            if (current.ChatPageActive)
            { 
                current.displaySNS(message);
                //AndroidUtils.ShowNotification(this, "Message", message);
            }
            else
            {
                AndroidUtils.ShowNotification(this, "SNS Push", message);
            }
        }
    }
}