using System;
using System.Collections.Generic;
using HassadFood.ViewModels;
using HassadFood.WebService;
using Xamarin.Forms;

namespace HassadFood.Views
{
    public partial class WorkListScreen : ContentPage
    {
        WorkListModel viewModel;
        public WorkListScreen()
        {
            InitializeComponent();
            BindingContext = viewModel = new WorkListModel(this);
            FlowDirection = Resx.AppResources.Culture.Parent.Name.ToString().Equals("ar") ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
            NotificationList.ItemTapped += async (sender, e) =>
            {
                var position = viewModel.WorkListInformation.get_worklist_user.IndexOf((GetWorklistUser)((ListView)sender).SelectedItem);
                Title = "";
                await Navigation.PushAsync(new WorkListDetailScreen(position));
                ((ListView)sender).SelectedItem = null;
            };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Title = "Notifications";
        }
    }
}
