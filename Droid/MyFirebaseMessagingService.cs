using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Media;
using Android.OS;
using Android.Util;
using Firebase.Messaging;

namespace HassadFood.Droid
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class MyFirebaseMessagingService : FirebaseMessagingService
    {
        const string TAG = "MyFirebaseMsgService";
        public override void OnMessageReceived(RemoteMessage message)
        {
            Log.Debug(TAG, "From: " + message.From);
            Log.Debug(TAG, "Notification Message Body: " + message.GetNotification().Body);
            SendNotification(message.GetNotification().Body);
            // CreateNotificationChannel();
        }

        void SendNotification(string messageBody)
        {
            PackageManager pm = PackageManager;
            Intent launchIntent = pm.GetLaunchIntentForPackage(PackageName);

            CreateNotificationChannel();

            var pendingIntentFlags = (Build.VERSION.SdkInt >= BuildVersionCodes.S)
            ? PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Mutable
            : PendingIntentFlags.UpdateCurrent;

            var pendingIntent = PendingIntent.GetActivity(this, 0, launchIntent, pendingIntentFlags);

            var notificationBuilder = new Notification.Builder(this)
                .SetSmallIcon(Resource.Drawable.ic_launcher)
                .SetContentTitle("Hassad Food")
                .SetContentText(messageBody)
                .SetAutoCancel(true)
                .SetStyle(new Notification.BigTextStyle()
                .BigText(messageBody))
                .SetContentIntent(pendingIntent);

            var notificationManager = NotificationManager.FromContext(this);
            notificationManager.Notify(DateTime.Now.Millisecond, notificationBuilder.Build());
        }

        void CreateNotificationChannel()
        {
            if(Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                return;
            } else
            {
                var channel = new NotificationChannel("NOTIFICATION_ID",
                                         "FCM Notifications",
                                         NotificationImportance.High)
                {

                    Description = "Firebase Cloud Messages appear in this channel"
                };

                var notificationManager = (NotificationManager)GetSystemService(Context.NotificationService);
                notificationManager.CreateNotificationChannel(channel);
            }
        }
       
    }
}
