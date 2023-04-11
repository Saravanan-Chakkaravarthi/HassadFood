using System;
using System.Collections.Generic;
using HassadFood.ViewModels;
using HassadFood.WebService;
using Xamarin.Forms;

namespace HassadFood.Views
{
    public partial class LeaveSummaryScreen : ContentPage
    {
        LeaveSummaryModel viewModel;
        public LeaveSummaryScreen()
        {
            InitializeComponent();
            FlowDirection = Resx.AppResources.Culture.Parent.Name.ToString().Equals("ar") ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
            BindingContext = viewModel = new LeaveSummaryModel(this);
            LeaveSummaryList.ItemTapped += async (sender, e) =>
            {
                //Title = "";
                //var position = viewModel.LeaveSummaryInformation.get_leave_summary.IndexOf((GetLeaveSummary)((ListView)sender).SelectedItem);
                //await Navigation.PushAsync(new LeaveDetailsScreen(position));
                ((ListView)sender).SelectedItem = null;
            };

            //addMenu.Clicked += async (sender, e) => 
            //{
            //    Title = "";
            //    await Navigation.PushAsync(new LeaveRequestScreen());
            //};
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.NavigationStackCount = Navigation.NavigationStack.Count;
        }
    }
}
