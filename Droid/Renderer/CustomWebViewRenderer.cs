using System;
using System.ComponentModel;
using Android.Content;
using HassadFood.Droid.Renderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(WebView), typeof(CustomWebViewRenderer))]
namespace HassadFood.Droid.Renderer
{
    public class CustomWebViewRenderer : WebViewRenderer
    {
        public CustomWebViewRenderer(Context context) : base(context)
        {
            AutoPackage = false;
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (Control != null)
            {
                Control.Settings.LoadWithOverviewMode = true;
                Control.Settings.UseWideViewPort = true;
                Control.Settings.BuiltInZoomControls = true;
                Control.Settings.DisplayZoomControls = true;
            }
            base.OnElementPropertyChanged(sender, e);
        }

    }
}