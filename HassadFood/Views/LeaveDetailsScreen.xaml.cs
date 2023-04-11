using System;
using System.Collections.Generic;
using HassadFood.Interface;
using HassadFood.ViewModels;
using HassadFood.WebService;
using Xamarin.Forms;

namespace HassadFood.Views
{
    public partial class LeaveDetailsScreen : ContentPage
    {
        IUserDetails userDetails;
        ToolbarItem EditMenu, DeleteMenu;
        LeaveDetailsModel viewModel;
        public LeaveDetailsScreen(int position)
        {
            InitializeComponent();
            FlowDirection = Resx.AppResources.Culture.Parent.Name.ToString().Equals("ar") ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
            BindingContext = viewModel = new LeaveDetailsModel(this, position);

            /*EditMenu = new ToolbarItem() { Text = "Edit", Order = ToolbarItemOrder.Primary, Priority = 0 };
            DeleteMenu = new ToolbarItem() { Text = "Delete", Order = ToolbarItemOrder.Primary, Priority = 1 };

            if (LeaveDetail.approval_status.Equals("Saved For Later"))
            {
                this.ToolbarItems.Add(EditMenu);
                this.ToolbarItems.Add(DeleteMenu);
            }
            else if (LeaveDetail.approval_status.Equals("Work in Progress"))
            {
                this.ToolbarItems.Add(EditMenu);
            }*/

        }
    }
}
