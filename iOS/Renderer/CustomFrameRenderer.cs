using System;
using HassadFood;
using HassadFood.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomFrame), typeof(CustomFrameRenderer))]
namespace HassadFood.iOS
{
    public class CustomFrameRenderer : VisualElementRenderer<Frame>
    {
        public CustomFrameRenderer()
        {
            Layer.BorderColor = Color.FromHex("#59979797").ToCGColor();
            Layer.BorderWidth = 1;
        }
    }
}
