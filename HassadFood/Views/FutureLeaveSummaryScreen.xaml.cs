using System;
using System.Collections.Generic;
using HassadFood.ViewModels;
using Xamarin.Forms;

namespace HassadFood.Views
{
    public partial class FutureLeaveSummaryScreen : ContentPage
    {
        FutureLeaveSummaryModel viewModel;
        public FutureLeaveSummaryScreen()
        {
            InitializeComponent();
            FlowDirection = Resx.AppResources.Culture.Parent.Name.ToString().Equals("ar") ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
            BindingContext = viewModel = new FutureLeaveSummaryModel(this);
            FutureLeaveSummaryList.ItemTapped += async (sender, e) =>
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
