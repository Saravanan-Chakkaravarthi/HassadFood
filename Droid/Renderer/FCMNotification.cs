using System;
using Android.App;
using Android.Content;
using Android.Gms.Common;
using Android.Util;
using Firebase.Iid;
using HassadFood.Droid.Renderer;
using HassadFood.Interface;
using static Android.Provider.Settings;

[assembly: Xamarin.Forms.Dependency(typeof(FCMNotification))]
namespace HassadFood.Droid.Renderer
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    public class FCMNotification : FirebaseInstanceIdService, INotification
    {
        string refreshedToken;
        const string TAG = "FirebaseIDService";

        /// <summary>
        /// On the token refresh.
        /// </summary>
        public override void OnTokenRefresh()
        {
            try
            {
                refreshedToken = FirebaseInstanceId.Instance.Token;
                Log.Debug(TAG, "Refreshed token: " + refreshedToken);
            }
            catch (Exception ex)
            {
                Log.Debug(TAG, "Refreshed token: " + ex.Message);
            }
        }

        /// <summary>
        /// Is the play services available.
        /// </summary>
        /// <returns><c>true</c>, if play services available was ised, <c>false</c> otherwise.</returns>
        bool IsPlayServicesAvailable()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(Android.App.Application.Context);
            if (resultCode != ConnectionResult.Success)
            {
                if (!GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                    return false;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Registers the notification.
        /// </summary>
        /// <returns>The notification.</returns>
        public string RegisterNotification()
        {
            if (IsPlayServicesAvailable())
            {
                OnTokenRefresh();
                return refreshedToken;
            }
            return null;
        }

        /// <summary>
        /// Unregister notification.
        /// </summary>
        public void UnRegisterNotification()
        {
            try
            {
                Xamarin.Forms.Application.Current.Properties["FCM_Notification"] = "false";
                FirebaseInstanceId.Instance.DeleteInstanceId();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Devices the identifier.
        /// </summary>
        /// <returns>The identifier.</returns>
        public string DeviceID()
        {
            string temp = null;
            try
            {
                temp = Android.OS.Build.Serial;
                if (string.IsNullOrWhiteSpace(temp) || temp == Android.OS.Build.Unknown || temp == "0")
                {                    
                    var context = Android.App.Application.Context;
                    temp = Secure.GetString(context.ContentResolver, Secure.AndroidId);                    
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
            return temp;
        }

        public void UpdateApp()
        {
            string appPackageName = Application.Context.PackageName;
            try
            {
                var intent = new Intent(Intent.ActionView, Android.Net.Uri.Parse("market://details?id=" + appPackageName));
                // we need to add this, because the activity is in a new context.
                // Otherwise the runtime will block the execution and throw an exception
                intent.AddFlags(ActivityFlags.NewTask);

                Application.Context.StartActivity(intent);
            }
            catch (ActivityNotFoundException)
            {
                var intent = new Intent(Intent.ActionView, Android.Net.Uri.Parse("http://play.google.com/store/apps/details?id=" + appPackageName));
                // we need to add this, because the activity is in a new context.
                // Otherwise the runtime will block the execution and throw an exception
                intent.AddFlags(ActivityFlags.NewTask);

                Application.Context.StartActivity(intent);
            }
        }

    }
}
