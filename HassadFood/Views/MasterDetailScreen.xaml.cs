using System;
using System.Collections.Generic;
using HassadFood.Interface;
using Xamarin.Forms;

namespace HassadFood.Views
{
    public partial class MasterDetailScreen : MasterDetailPage
    {
        public MasterDetailScreen(ILoginManager loginManager)
        {
            InitializeComponent();

            FlowDirection = Resx.AppResources.Culture.Parent.Name.ToString().Equals("ar") ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;

            NavigationPage.SetHasNavigationBar(this, false);
            Detail = new NavigationPage(new MainPage(loginManager));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Title = "";
        }
    }
}
