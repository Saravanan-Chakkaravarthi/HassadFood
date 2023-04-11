using System;
using HassadFood.iOS.Renderer;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(WebView), typeof(CustomWebViewRenderer))]
namespace HassadFood.iOS.Renderer
{
    public class CustomWebViewRenderer : Xamarin.Forms.Platform.iOS.WebViewRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if (NativeView != null && e.NewElement != null)
                SetupControlSettings();
        }

        private void SetupControlSettings()
        {
            var webView = ((UIWebView)NativeView);
            webView.ScalesPageToFit = true;
        }
    }
}
