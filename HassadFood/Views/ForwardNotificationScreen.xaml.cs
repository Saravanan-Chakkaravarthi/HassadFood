using HassadFood.ViewModels;
using HassadFood.WebService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HassadFood.Views
{
    public partial class ForwardNotificationScreen : ContentPage
    {
        ForwardNotificationRequestModel viewModel;

        public ForwardNotificationScreen(GetWorkList selected_notification)
        {
            InitializeComponent();

            BindingContext = viewModel = new ForwardNotificationRequestModel(this, selected_notification);

          

          
         
          

            var tapGestureRecognizerHomeStack = new TapGestureRecognizer();
            tapGestureRecognizerHomeStack.Tapped += async (s, e) =>
            {
                if (!HomeStack.AnimationIsRunning("ScaleTo"))
                {
                    await HomeStack.ScaleTo(0.5, 50, Easing.CubicOut);
                    await HomeStack.ScaleTo(1, 50, Easing.CubicIn);
                }
                Title = "";
                await Navigation.PopToRootAsync();
            };
            HomeStack.GestureRecognizers.Add(tapGestureRecognizerHomeStack);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.NavigationStackCount = Navigation.NavigationStack.Count;
        }
    }

}

