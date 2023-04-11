using System;
using System.Collections.Generic;
using HassadFood.Interface;
using HassadFood.ViewModels;
using Xamarin.Forms;

namespace HassadFood.Views
{
    public partial class LoginPage : ContentPage
    {
        LoginModel viewModel;
        ILoginManager _loginManager;
        public LoginPage(ILoginManager loginManager)
        {
            InitializeComponent();
            _loginManager = loginManager;
            FlowDirection = Resx.AppResources.Culture.Parent.Name.ToString().Equals("ar") ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            BindingContext = viewModel = new LoginModel(this, _loginManager);
            App.NavigationStackCount = Navigation.NavigationStack.Count;
        }
    }
}
