using HassadFood.ViewModels;
using HassadFood.WebService;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HassadFood.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RequestInformationScreen : ContentPage
    {
        RequestInformationModel viewModel;

        public RequestInformationScreen(GetWorkList selected_notification)
        {
            InitializeComponent();

            BindingContext = viewModel = new RequestInformationModel(this, selected_notification);

            ReassignList.ItemTapped += (sender, e) =>
            {
                if (viewModel.SearchView)
                {
                    viewModel.SearchView = false;
                    viewModel.ReassignUser = viewModel.SelectedReassignItem.display_name;
                    viewModel.ReassignName = viewModel.SelectedReassignItem.user_name;
                }

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
                        viewModel.ReassignUserInformation = DataInfo.ReassignUserInformation;
                        //viewModel.ReassignUserInformation = new ReassignUserResponse();
                        //viewModel.ReassignUserInformation.p_return = new List<PReturn>();
                    }
                }
                catch (Exception ex)
                {

                    viewModel.ReassignUserInformation = new ReassignUserResponse();
                    viewModel.ReassignUserInformation.get_reassign_users = new List<GetReassignUser>();
                }
            };


          



        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.NavigationStackCount = Navigation.NavigationStack.Count;
        }
    }

}