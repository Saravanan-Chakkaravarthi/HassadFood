using System;
using Android.Content;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Views.InputMethods;
using Android.Widget;
using HassadFood;
using HassadFood.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]
namespace HassadFood.Droid
{
    class CustomEntryRenderer : EntryRenderer
    {
        public CustomEntryRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Entry> e)
        {
            base.OnElementChanged(e);
            Control.SetBackgroundColor(global::Android.Graphics.Color.Transparent);

            // this.Control.Background = this.Resources.GetDrawable(Resource.Drawable.custom_entry_style);
            // Control.SetBackgroundDrawable(Resource.Drawable.custom_entry_style);
            Control.Background = ContextCompat.GetDrawable(Context, Resource.Drawable.custom_entry_style);
            //GradientDrawable gd = new GradientDrawable();
            //gd.SetCornerRadius(10);
            //gd.SetColor(global::Android.Graphics.Color.Gray);
            //Control.Background = gd;

            CustomEntry entry = (CustomEntry)this.Element;

            if (this.Control != null)
            {
                if (entry != null)
                {
                    SetReturnType(entry);

                    // Editor Action is called when the return button is pressed
                    Control.EditorAction += (object sender, TextView.EditorActionEventArgs args) =>
                    {
                        if (entry.ReturnType != ReturnType.Next)
                            entry.Unfocus();

                    // Call all the methods attached to base_entry event handler Completed
                    entry.InvokeCompleted();
                    };
                }
            }

        }

        void SetReturnType(CustomEntry entry)
        {
            ReturnType type = entry.ReturnType;

            switch (type)
            {
                case ReturnType.Go:
                    Control.ImeOptions = ImeAction.Go;
                    Control.SetImeActionLabel("Go", ImeAction.Go);
                    break;
                case ReturnType.Next:
                    Control.ImeOptions = ImeAction.Next;
                    Control.SetImeActionLabel("Next", ImeAction.Next);
                    break;
                case ReturnType.Send:
                    Control.ImeOptions = ImeAction.Send;
                    Control.SetImeActionLabel("Send", ImeAction.Send);
                    break;
                case ReturnType.Search:
                    Control.ImeOptions = ImeAction.Search;
                    Control.SetImeActionLabel("Search", ImeAction.Search);
                    break;
                default:
                    Control.ImeOptions = ImeAction.Done;
                    Control.SetImeActionLabel("Done", ImeAction.Done);
                    break;
            }
        }
    }
}
