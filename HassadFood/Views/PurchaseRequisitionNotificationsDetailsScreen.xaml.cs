using HassadFood.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HassadFood.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PurchaseRequisitionNotificationsDetailsScreen : ContentPage
    {
        PurchaseNotificationsDetailsModel viewModel;
        public PurchaseRequisitionNotificationsDetailsScreen()
        {
            InitializeComponent();
            BindingContext = viewModel = new PurchaseNotificationsDetailsModel(this);
            ListVwNotifications.ItemTapped += async (sender, e) =>
            {
                ((ListView)sender).SelectedItem = null;
            };


            //var tapGestureShowFooter = new TapGestureRecognizer();
            //tapGestureShowFooter.Tapped += async (s, e) =>
            //{
            //    if (!Footer.IsVisible)
            //    {
            //        Footer.IsVisible = true;
            //        ImgFooterIndicator.Source = "ic_circular_down_arrow.png";
            //        // ScrlVwParent.sc
            //        //await ScrlVwParent.ScrollToAsync(0, ScrlVwParent.Height, true);
            //    }
            //    else
            //    {
            //        Footer.IsVisible = false;
            //        ImgFooterIndicator.Source = "ic_circular_up_arrow.png";
            //        // await ScrlVwParent.ScrollToAsync(0, 0, true);
            //    }
            //};
            //StackFooterIndicator.GestureRecognizers.Add(tapGestureShowFooter);


            //var tapGestureRecognizerContact = new TapGestureRecognizer();
            //tapGestureRecognizerContact.Tapped += async (s, e) =>
            //{
            //    if (!Contact.AnimationIsRunning("ScaleTo"))
            //    {
            //        await Contact.ScaleTo(0.5, 50, Easing.CubicOut);
            //        await Contact.ScaleTo(1, 50, Easing.CubicIn);
            //    }
            //    Title = "";
            //    await Navigation.PushAsync(new ContactUsScreen());
            //};
            //Contact.GestureRecognizers.Add(tapGestureRecognizerContact);

            //var tapGestureRecognizerReport = new TapGestureRecognizer();
            //tapGestureRecognizerReport.Tapped += async (s, e) =>
            //{
            //    if (!Report.AnimationIsRunning("ScaleTo"))
            //    {
            //        await Report.ScaleTo(0.5, 50, Easing.CubicOut);
            //        await Report.ScaleTo(1, 50, Easing.CubicIn);
            //    }
            //    Title = "";
            //    await Navigation.PushAsync(new ReportIncidentsScreen());
            //};
            //Report.GestureRecognizers.Add(tapGestureRecognizerReport);

            //var tapGestureRecognizerHomeStack = new TapGestureRecognizer();
            //tapGestureRecognizerHomeStack.Tapped += async (s, e) =>
            //{
            //    if (!HomeStack.AnimationIsRunning("ScaleTo"))
            //    {
            //        await HomeStack.ScaleTo(0.5, 50, Easing.CubicOut);
            //        await HomeStack.ScaleTo(1, 50, Easing.CubicIn);
            //    }
            //    Title = "";
            //    await Navigation.PopToRootAsync();
            //};
            //HomeStack.GestureRecognizers.Add(tapGestureRecognizerHomeStack);




        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.NavigationStackCount = Navigation.NavigationStack.Count;
        }
    }

}