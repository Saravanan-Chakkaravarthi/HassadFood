using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace HassadFood.Droid
{
    [Activity(Theme = "@style/Theme.Splash", //Indicates the theme to use for this activity
        MainLauncher = true, //Set it as boot activity
        NoHistory = true,
        ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class Splash : Activity
    {
        private static int SPLASH_TIME_OUT = 3000;
        Handler handler;
        Action action;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your application here
            SetContentView(Resource.Layout.Splash);
            handler = new Handler();
            action = Callback;            
            //this.StartActivity(typeof(MainActivity));
        }

        void Callback()
        {
            StartActivity(typeof(MainActivity));
            handler.RemoveCallbacks(action);
            Finish();
        }

        protected override void OnStart()
        {
            base.OnStart();
            handler.PostDelayed(action, SPLASH_TIME_OUT);
        }

        protected override void OnStop()
        {
            base.OnStop();
            handler.RemoveCallbacks(action);
        }
    }   
}
