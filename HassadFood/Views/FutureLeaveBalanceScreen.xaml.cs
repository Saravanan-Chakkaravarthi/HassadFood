using System;
using System.Collections.Generic;
using HassadFood.ViewModels;
using Xamarin.Forms;

namespace HassadFood.Views
{
    public partial class FutureLeaveBalanceScreen : ContentPage
    {
        FutureLeaveBalanceModel viewModel;
        public FutureLeaveBalanceScreen()
        {
            InitializeComponent();
            FlowDirection = Resx.AppResources.Culture.Parent.Name.ToString().Equals("ar") ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
            BindingContext = viewModel = new FutureLeaveBalanceModel(this);
            FutureLeaveBalanceList.ItemTapped += async (sender, e) =>
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
