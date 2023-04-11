using System;
using Foundation;
using HassadFood.Interface;
using HassadFood.iOS.Renderer;
using ObjCRuntime;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(FCMNotification))]
namespace HassadFood.iOS.Renderer
{
    public class FCMNotification : INotification
    {
        /// <summary>
        /// Devices the identifier.
        /// </summary>
        /// <returns>The identifier.</returns>
        public string DeviceID()
        {
            string temp = null;
            try
            {
                temp = UIKit.UIDevice.CurrentDevice.IdentifierForVendor.AsString();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
            return temp;
        }

        /// <summary>
        /// Registers the notification.
        /// </summary>
        /// <returns>The notification.</returns>
        public string RegisterNotification()
        {
            return NSUserDefaults.StandardUserDefaults.StringForKey("PushDeviceToken");//InstanceId.SharedInstance.Token;
        }
        /// <summary>
        /// Unregister notification.
        /// </summary>
        public void UnRegisterNotification()
        {
        }

        public void UpdateApp()
        {
            string bundleID = NSBundle.MainBundle.BundleIdentifier;
            bool isSimulator = ObjCRuntime.Runtime.Arch == Arch.SIMULATOR;
            NSUrl itunesLink;
            if (isSimulator)
            {
                itunesLink = new NSUrl("https://itunes.apple.com/us/genre/ios/id36?mt=8");
            }
            else
            {
                itunesLink = new NSUrl("https://itunes.apple.com/app/hassad-food/id1294667714?mt=8");
            }
            UIApplication.SharedApplication.OpenUrl(itunesLink, new NSDictionary() { }, null);
        }
    }
}
