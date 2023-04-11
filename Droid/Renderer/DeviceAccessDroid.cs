using System;
using Android;
using Android.Content;
using Android.OS;
using Android.Support.V4.Content;
using HassadFood.Droid.Renderer;
using HassadFood.Interface;

[assembly: Xamarin.Forms.Dependency(typeof(DeviceAccessDroid))]
namespace HassadFood.Droid.Renderer
{
    public class DeviceAccessDroid : IDeviceAccess
    {
        public void OpenDeviceSettings()
        {
            try
            {
                MainActivity.Instance.StartActivity(new Intent(Android.Provider.Settings.ActionApplicationDetailsSettings, Android.Net.Uri.Parse("package:" + Android.App.Application.Context.PackageName)));
                //StartActivity(new Intent( Android.Provider.Settings.ActionApplicationDetailsSettings,Android.Net.Uri.Parse("package:" + Android.App.Application.Context.PackageName)));
                //  Xamarin.Forms.Forms.Context.StartActivity(new Android.Content.Intent(Android.Provider.Settings.ActionApplicationDetailsSettings));
            }
            catch (Exception ex)
            {

            }
        }

        public bool GetCameraPermissionAsync()
        {
            if ((int)Build.VERSION.SdkInt < 23)
            {
                return true;
            }
            else
            {
                //ActivityCompat.RequestPermissions (Android.App.Application.Context as Android.App.Activity , new string[] { Manifest.Permission.Camera }, 123);
                var permissionCheck = ContextCompat.CheckSelfPermission(Android.App.Application.Context, Manifest.Permission.Camera);

                return (permissionCheck == Android.Content.PM.Permission.Granted);
            }
        }

        public bool GetStoragePermissionAsync()
        {
            if ((int)Build.VERSION.SdkInt < 23)
            {
                return true;
            }
            else
            {
                //ActivityCompat.RequestPermissions (Android.App.Application.Context as Android.App.Activity , new string[] { Manifest.Permission.Camera }, 123);
                var permissionCheck = ContextCompat.CheckSelfPermission(Android.App.Application.Context, Manifest.Permission.WriteExternalStorage);

                return (permissionCheck == Android.Content.PM.Permission.Granted);
            }
        }
    }
}
