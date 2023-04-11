using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using FFImageLoading.Forms.Droid;
using Plugin.Permissions;
using Plugin.CurrentActivity;
using Xamarin.Forms;
using HassadFood.Interface;

namespace HassadFood.Droid
{
    [Activity(Label = "Hassad Food", Theme = "@style/MyTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        internal static MainActivity Instance { get; private set; }

        protected override void OnCreate(Bundle bundle)
        {
            Instance = this;

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(true);

            CrossCurrentActivity.Current.Init(this, bundle);

            CrossCurrentActivity.Current.Activity = this;
            Plugin.Fingerprint.CrossFingerprint.SetCurrentActivityResolver(() => CrossCurrentActivity.Current.Activity);

            var config = new FFImageLoading.Config.Configuration()
            {
                VerboseLogging = false,
                FadeAnimationEnabled = true,
                FadeAnimationDuration = 100,
                VerbosePerformanceLogging = false,
                VerboseMemoryCacheLogging = false,
                VerboseLoadingCancelledLogging = false,
                DiskCacheDuration = TimeSpan.FromMinutes(30),
                Logger = new CustomLogger(),
            };

            global::Xamarin.Forms.Forms.Init(this, bundle);

            LoadApplication(new App());
        }

        public override void OnBackPressed()
        {
            ActivityManager mngr = (ActivityManager)this.GetSystemService(Context.ActivityService);

            System.Collections.Generic.IList<ActivityManager.AppTask> taskList = mngr.AppTasks;

            if (App.NavigationStackCount <= 1)
            {
                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                alert.SetTitle("Alert");
                alert.SetMessage("Do you want to exit?");
                alert.SetPositiveButton("Yes", (senderAlert, args) =>
                {
                    base.OnBackPressed();
                });

                alert.SetNegativeButton("No", (senderAlert, args) =>
                {
                });

                Dialog dialog = alert.Create();
                dialog.Show();
            }
            else
                base.OnBackPressed();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }

    public class CustomLogger : FFImageLoading.Helpers.IMiniLogger
    {
        public void Debug(string message)
        {
            Console.WriteLine(message);
        }

        public void Error(string errorMessage)
        {
            Console.WriteLine(errorMessage);
        }

        public void Error(string errorMessage, System.Exception ex)
        {
            Error(errorMessage + System.Environment.NewLine + ex.ToString());
        }
    }
}
