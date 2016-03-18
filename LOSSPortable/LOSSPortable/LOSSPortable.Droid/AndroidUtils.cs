using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Util;
using Android.Media;
using System;

namespace LOSSPortable.Droid
{
    public class AndroidUtils
    {

        private static int REQUEST_CODE = 1001;

        public static void ShowNotification(Context context, string contentTitle,
                string contentText)
        {
            // Intent
            var intent = new Intent(context, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop);
            var pendingIntent = PendingIntent.GetActivity(context, 0, intent, PendingIntentFlags.OneShot);

            Notification.Builder builder = new Notification.Builder(context)
                .SetContentTitle(contentTitle)
                .SetContentText(contentText)
                .SetDefaults(NotificationDefaults.Sound | NotificationDefaults.Vibrate)
                .SetSmallIcon(Resource.Drawable.App6)
                .SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Notification))
                .SetAutoCancel(true)
                .SetContentIntent(pendingIntent);

            // Get the notification manager:
            NotificationManager notificationManager =
                context.GetSystemService(Context.NotificationService) as NotificationManager;
            
            notificationManager.Notify(1001, builder.Build());
            
        }

        public static void ShowNotification(Context context, String contentText)
        {
            ShowNotification(context, "", contentText);
        }
    }
}