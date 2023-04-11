using System;
using System.Linq;
using CoreGraphics;
using HassadFood;
using HassadFood.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ResolutionGroupName("Sobha")]
[assembly: ExportEffect(typeof(LayoutShadowEffect), "LayoutShadowEffect")]
namespace HassadFood.iOS
{
    public class LayoutShadowEffect : PlatformEffect
    {
        protected override void OnAttached()
    {
        try
        {
            var effect = (ShadowEffect)Element.Effects.FirstOrDefault(e => e is ShadowEffect);
            if (effect != null)
            {
                Container.Layer.BackgroundColor = effect.BGColor.ToCGColor();  
                Container.Layer.CornerRadius = effect.Radius;
                Container.Layer.ShadowColor = effect.Color.ToCGColor();
                Container.Layer.ShadowOffset = new CGSize(effect.DistanceX, effect.DistanceY);
                Container.Layer.ShadowOpacity = 1.0f;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Cannot set property on attached control. Error: ", ex.Message);
        }
    }

    protected override void OnDetached()
    {
            Container.Layer.ShadowOpacity = 0;
    }
}
}
