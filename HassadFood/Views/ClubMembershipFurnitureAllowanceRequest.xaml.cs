using System;
using System.Collections.Generic;
using HassadFood.ViewModels;
using Xamarin.Forms;

namespace HassadFood.Views
{
    public partial class ClubMembershipFurnitureAllowanceRequest : ContentPage
    {
        ClubMembershipFurnitureAllowanceRequestModel viewModel;
        public ClubMembershipFurnitureAllowanceRequest()
        {
            InitializeComponent();

            FlowDirection = Resx.AppResources.Culture.Parent.Name.ToString().Equals("ar") ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;

            BindingContext = viewModel = new ClubMembershipFurnitureAllowanceRequestModel(this);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.NavigationStackCount = Navigation.NavigationStack.Count;
        }
    }
}
