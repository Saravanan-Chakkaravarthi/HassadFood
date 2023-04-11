using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using HassadFood.ViewModels;
using HassadFood.WebService;
using Xamarin.Forms;

namespace HassadFood.Views
{
    public partial class LeaveRequestScreen : ContentPage
    {
        LeaveRequestModel viewModel;
        public LeaveRequestScreen()
        {
            InitializeComponent();
            BindingContext = viewModel = new LeaveRequestModel(this);
            FlowDirection = Resx.AppResources.Culture.Parent.Name.ToString().Equals("ar") ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;

            /*ReassignList.ItemTapped += (sender, e) =>
            {
                if (viewModel.SearchView)
                {
                    viewModel.SearchView = false;
                    viewModel.ReassignUser = viewModel.SelectedReassignItem.display_name;
                    viewModel.ReassignName = viewModel.SelectedReassignItem.user_name;
                }
               ((ListView)sender).SelectedItem = null;
            };

            SearchText.TextChanged += (sender, e) =>
            {
                try
                {
                    string newValue = e.NewTextValue;
                    if (newValue.ToString().Length >= 3)
                    {
                        SearchText.IsEnabled = !string.IsNullOrEmpty(newValue.ToString());

                        var cleanedNewPlaceHolderValue = Regex.Replace((newValue.ToString() ?? string.Empty).ToLowerInvariant(), @"\s+", string.Empty);

                        IEnumerable SuggestionsItem = DataInfo.ReassignUserInformation.get_reassign_users;
                        if (!string.IsNullOrEmpty(cleanedNewPlaceHolderValue) && DataInfo.ReassignUserInformation.get_reassign_users != null)
                        {
                            var filteredSuggestions = SuggestionsItem.Cast<GetReassignUser>()
                               .Where(x => Regex.Replace(x.display_name.ToLowerInvariant(), @"\s+", string.Empty).Contains(cleanedNewPlaceHolderValue))
                               .OrderByDescending(x => Regex.Replace(x.ToString()
                               .ToLowerInvariant(), @"\s+", string.Empty)
                               .StartsWith(cleanedNewPlaceHolderValue, StringComparison.CurrentCulture)).ToList();
                            viewModel.ReassignUserInformation = new ReassignUserResponse();
                            viewModel.ReassignUserInformation.get_reassign_users = new List<GetReassignUser>();
                            if (filteredSuggestions.Count > 0)
                            {
                                foreach (var suggestion in filteredSuggestions)
                                {
                                    viewModel.ReassignUserInformation.get_reassign_users.Add(suggestion);
                                }
                            }
                        }
                        else
                        {
                            viewModel.ReassignUserInformation = new ReassignUserResponse();
                            viewModel.ReassignUserInformation.get_reassign_users = new List<GetReassignUser>();
                        }
                    }
                    else
                    {
                        viewModel.ReassignUserInformation = new ReassignUserResponse();
                        viewModel.ReassignUserInformation.get_reassign_users = new List<GetReassignUser>();
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    viewModel.ReassignUserInformation = new ReassignUserResponse();
                    viewModel.ReassignUserInformation.get_reassign_users = new List<GetReassignUser>();
                }
            };*/
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Title = Resx.AppResources.LeaveRequest;
            App.NavigationStackCount = Navigation.NavigationStack.Count;
        }
    }
}
