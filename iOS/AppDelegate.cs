using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using FFImageLoading.Forms.Touch;
using Foundation;
using UIKit;
using Xamarin.Forms;

namespace HassadFood.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

            FFImageLoading.Forms.Platform.CachedImageRenderer.Init();

            var config = new FFImageLoading.Config.Configuration()
            {
                VerboseLogging = false,
                FadeAnimationEnabled = true,
                FadeAnimationDuration = 100,
                VerbosePerformanceLogging = false,
                VerboseMemoryCacheLogging = false,
                VerboseLoadingCancelledLogging = false,
                DiskCacheDuration = TimeSpan.FromMinutes(30),
                Logger = new CustomLogger()
            };
            UINavigationBarAppearance a = new UINavigationBarAppearance();
            a.ConfigureWithTransparentBackground();
            a.BackgroundColor = UIColor.FromRGB(12,51,90);
            a.TitleTextAttributes = new UIStringAttributes() { ForegroundColor = UIColor.White };
            


            UINavigationBar.Appearance.StandardAppearance = a;
            UINavigationBar.Appearance.CompactAppearance = a;
            UINavigationBar.Appearance.ScrollEdgeAppearance = a;
            UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;

            RegisterPushNotification();

            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }

        async void RegisterPushNotification()
        {
            await System.Threading.Tasks.Task.Delay(10000);
            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                var pushSettings = UIUserNotificationSettings.GetSettingsForTypes(
                                   UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound,
                                   new NSSet());

                UIApplication.SharedApplication.RegisterUserNotificationSettings(pushSettings);
                UIApplication.SharedApplication.RegisterForRemoteNotifications();
            }
            else
            {
                UIRemoteNotificationType notificationTypes = UIRemoteNotificationType.Alert | UIRemoteNotificationType.Badge | UIRemoteNotificationType.Sound;
                UIApplication.SharedApplication.RegisterForRemoteNotificationTypes(notificationTypes);
            }
        }

        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
            if (application.ApplicationState == UIApplicationState.Active)
            {
                var apsDictionary = userInfo["aps"] as NSDictionary;

                string body, Caption = null;
                if (apsDictionary != null)
                {
                    if (apsDictionary["alert"] is NSDictionary)
                    {
                        var alertDictionary = apsDictionary["alert"] as NSDictionary;

                        if (alertDictionary.ContainsKey(new NSString("title")))
                            Caption = alertDictionary["title"].ToString();

                        body = alertDictionary["body"].ToString();
                    }
                    else
                    {
                        body = apsDictionary["alert"].ToString();
                    }
                }
                else
                {
                    apsDictionary = userInfo["notification"] as NSDictionary;

                    if (apsDictionary.ContainsKey(new NSString("title")))
                        Caption = apsDictionary["title"].ToString();

                    body = apsDictionary["text"].ToString();
                }

                UIAlertController okayAlertController = UIAlertController.Create(Caption, body, UIAlertControllerStyle.Alert);
                okayAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
                UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(okayAlertController, true, null);
            }
        }

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            // Get current device token
            var DeviceToken = deviceToken.Description;
            if (!string.IsNullOrWhiteSpace(DeviceToken))
            {
                DeviceToken = DeviceToken.Trim('<').Trim('>');
            }

            // Get previous device token
            var oldDeviceToken = NSUserDefaults.StandardUserDefaults.StringForKey("PushDeviceToken");

            // Has the token changed?
            if (string.IsNullOrEmpty(oldDeviceToken) || !oldDeviceToken.Equals(DeviceToken))
            {
                //TODO: Put your own logic here to notify your server that the device token has changed/been created!
            }
            DeviceToken = Regex.Replace(DeviceToken, @"\s", "");
            // Save new device token 
            System.Console.WriteLine("Token: " + DeviceToken);
            NSUserDefaults.StandardUserDefaults.SetString(DeviceToken, "PushDeviceToken");

        }

        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            UIAlertView alert = new UIAlertView()
            {
                Title = "Error registering push notifications",
                Message = error.LocalizedDescription
            };
            alert.AddButton("OK");
            alert.Show();
        }

        public override void ReceivedLocalNotification(UIApplication application, UILocalNotification notification)
        {
            // reset our badge
            UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
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

        public void Error(string errorMessage, Exception ex)
        {
            Error(errorMessage + System.Environment.NewLine + ex.ToString());
        }
    }
}
