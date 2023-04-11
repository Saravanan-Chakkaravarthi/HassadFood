using System;
using System.Linq;
using HassadFood.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ResolutionGroupName("Sobha")]
[assembly: ExportEffect(typeof(LayoutShadowEffect), "LayoutShadowEffect")]
namespace HassadFood.Droid
{
    public class LayoutShadowEffect: PlatformEffect
    {
        protected override void OnAttached()
        {
            try
            {
                //Container.SetBackgroundResource(Resource.Drawable.viewshadow);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cannot set property on attached control. Error: ", ex.Message);
            }
        }

        protected override void OnDetached()
        {
            
        }
    }
}
