using System;
using System.Collections.Generic;
using HassadFood.ViewModels;
using Xamarin.Forms;

namespace HassadFood.Views
{
    public partial class CarLoanRequest : ContentPage
    {
        public CarLoanRequest()
        {
            CarLoanRequestModel viewModel;
            InitializeComponent();

            FlowDirection = Resx.AppResources.Culture.Parent.Name.ToString().Equals("ar") ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;

            BindingContext = viewModel = new CarLoanRequestModel(this);
        }

        public void ChangeText(string mobile, string address)
        {
            mobileNo.Text = mobileNo.Text.Insert(0, mobile);
            perAddress.Text = perAddress.Text.Insert(0, address);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.NavigationStackCount = Navigation.NavigationStack.Count;
        }
    }
}
