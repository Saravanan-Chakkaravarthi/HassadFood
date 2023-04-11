using System;
using HassadFood.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(DatePicker), typeof(CustomDatePickerRenderer))]
namespace HassadFood.iOS
{
    public class CustomDatePickerRenderer : DatePickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.DatePicker> e)
        {
            base.OnElementChanged(e);

            var element = this.Element as DatePicker;

            if (Control != null && element != null)
            {
                var date = (UIDatePicker)Control.InputView;
                date.Locale = new Foundation.NSLocale("en");
            }
        }
    }
}
