﻿using System;
using System.Collections.Generic;
using HassadFood.ViewModels;
using Xamarin.Forms;

namespace HassadFood.Views
{
    public partial class EmployeeLetterRequest : ContentPage
    {
        EmployeeLetterRequestModel viewModel;
        public EmployeeLetterRequest()
        {
            InitializeComponent();

            FlowDirection = Resx.AppResources.Culture.Parent.Name.ToString().Equals("ar") ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;

            BindingContext = viewModel = new EmployeeLetterRequestModel(this);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.NavigationStackCount = Navigation.NavigationStack.Count;
        }
    }
}
