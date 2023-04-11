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
    public partial class PurchaseDistributionNotificationsDetailsScreen : ContentPage
    {
        PurchaseNotificationsDetailsModel viewModel;
        public PurchaseDistributionNotificationsDetailsScreen()
        {
            InitializeComponent();
            BindingContext = viewModel = new PurchaseNotificationsDetailsModel(this);
            ListVwNotifications.ItemTapped += async (sender, e) =>
            {
                ((ListView)sender).SelectedItem = null;
            };





        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.NavigationStackCount = Navigation.NavigationStack.Count;
        }
    }

}