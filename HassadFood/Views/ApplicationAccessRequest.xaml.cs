using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using HassadFood.ViewModels;
using HassadFood.WebService;
using Xamarin.Forms;

namespace HassadFood.Views
{
    public partial class ApplicationAccessRequest : ContentPage
    {
        ApplicationAccessRequestModel viewModel;
        public ApplicationAccessRequest()
        {
            InitializeComponent();

            FlowDirection = Resx.AppResources.Culture.Parent.Name.ToString().Equals("ar") ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;

            BindingContext = viewModel = new ApplicationAccessRequestModel(this);

            ApplicationList.ItemTapped += (sender, e) =>
            {
                if (viewModel.SearchApplicationView)
                {
                    viewModel.SearchApplicationView = false;
                    viewModel.ApplicationAccess = viewModel.SelectedApplicationItem.application_name;
                }
               //((ListView)sender).SelectedItem = null;
            };

            SearchApplicationText.TextChanged += (sender, e) =>
            {
                try
                {
                    string newValue = e.NewTextValue;
                    if (newValue.ToString().Length >= 3)
                    {
                        SearchApplicationText.IsEnabled = !string.IsNullOrEmpty(newValue.ToString());

                        var cleanedNewPlaceHolderValue = Regex.Replace((newValue.ToString() ?? string.Empty).ToLowerInvariant(), @"\s+", string.Empty);

                        IEnumerable SuggestionsItem = DataInfo.ApplicationAccessDropDownInformation.p_applicationaccess_list;
                        if (!string.IsNullOrEmpty(cleanedNewPlaceHolderValue) && DataInfo.ApplicationAccessDropDownInformation.p_applicationaccess_list != null)
                        {
                            var filteredSuggestions = SuggestionsItem.Cast<PApplicationaccessList>()
                               .Where(x => Regex.Replace(x.application_name.ToLowerInvariant(), @"\s+", string.Empty).Contains(cleanedNewPlaceHolderValue))
                               .OrderByDescending(x => Regex.Replace(x.ToString()
                               .ToLowerInvariant(), @"\s+", string.Empty)
                               .StartsWith(cleanedNewPlaceHolderValue, StringComparison.CurrentCulture)).ToList();
                            viewModel.ApplicationAccessDropDownInformation = new ApplicationAccessDropDownResponse();
                            viewModel.ApplicationAccessDropDownInformation.p_applicationaccess_list = new List<PApplicationaccessList>();
                            if (filteredSuggestions.Count > 0)
                            {
                                foreach (var suggestion in filteredSuggestions)
                                {
                                    viewModel.ApplicationAccessDropDownInformation.p_applicationaccess_list.Add(suggestion);
                                }
                            }
                        }
                        else
                        {
                            viewModel.ApplicationAccessDropDownInformation = DataInfo.ApplicationAccessDropDownInformation;
                        }
                    }
                    else
                    {
                        viewModel.ApplicationAccessDropDownInformation = DataInfo.ApplicationAccessDropDownInformation;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    viewModel.ApplicationAccessDropDownInformation = DataInfo.ApplicationAccessDropDownInformation;
                }
            };

            ResponsibilityList.ItemTapped += (sender, e) =>
            {
                if (viewModel.SearchResponsibilityView)
                {
                    viewModel.SearchResponsibilityView = false;
                    viewModel.ResponsibilityName = viewModel.SelectedResponsibilityItem.responsibility_name;
                }
               //((ListView)sender).SelectedItem = null;
            };

            SearchResponsibilityText.TextChanged += (sender, e) =>
            {
                try
                {
                    string newValue = e.NewTextValue;
                    if (newValue.ToString().Length >= 3)
                    {
                        SearchResponsibilityText.IsEnabled = !string.IsNullOrEmpty(newValue.ToString());

                        var cleanedNewPlaceHolderValue = Regex.Replace((newValue.ToString() ?? string.Empty).ToLowerInvariant(), @"\s+", string.Empty);

                        IEnumerable SuggestionsItem = DataInfo.ApplicationAccessRNameInformation.p_rname_list;
                        if (!string.IsNullOrEmpty(cleanedNewPlaceHolderValue) && DataInfo.ApplicationAccessRNameInformation.p_rname_list != null)
                        {
                            var filteredSuggestions = SuggestionsItem.Cast<PRnameList>()
                               .Where(x => Regex.Replace(x.responsibility_name.ToLowerInvariant(), @"\s+", string.Empty).Contains(cleanedNewPlaceHolderValue))
                               .OrderByDescending(x => Regex.Replace(x.ToString()
                               .ToLowerInvariant(), @"\s+", string.Empty)
                               .StartsWith(cleanedNewPlaceHolderValue, StringComparison.CurrentCulture)).ToList();
                            viewModel.ApplicationAccessRNameInformation = new ApplicationAccessRNameResponse();
                            viewModel.ApplicationAccessRNameInformation.p_rname_list = new List<PRnameList>();
                            if (filteredSuggestions.Count > 0)
                            {
                                foreach (var suggestion in filteredSuggestions)
                                {
                                    viewModel.ApplicationAccessRNameInformation.p_rname_list.Add(suggestion);
                                }
                            }
                        }
                        else
                        {
                            viewModel.ApplicationAccessDropDownInformation = DataInfo.ApplicationAccessDropDownInformation;
                        }
                    }
                    else
                    {
                        viewModel.ApplicationAccessDropDownInformation = DataInfo.ApplicationAccessDropDownInformation;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    viewModel.ApplicationAccessRNameInformation = new ApplicationAccessRNameResponse();
                    viewModel.ApplicationAccessRNameInformation.p_rname_list = new List<PRnameList>();
                }
            };

            CompanyList.ItemTapped += (sender, e) =>
            {
                if (viewModel.SearchCompanyView)
                {
                    viewModel.SearchCompanyView = false;
                    viewModel.CompanyName = viewModel.SelectedCompanyItem.company_name;
                }
               //((ListView)sender).SelectedItem = null;
            };

            SearchCompanyText.TextChanged += (sender, e) =>
            {
                try
                {
                    string newValue = e.NewTextValue;
                    if (newValue.ToString().Length >= 3)
                    {
                        SearchCompanyText.IsEnabled = !string.IsNullOrEmpty(newValue.ToString());

                        var cleanedNewPlaceHolderValue = Regex.Replace((newValue.ToString() ?? string.Empty).ToLowerInvariant(), @"\s+", string.Empty);

                        IEnumerable SuggestionsItem = DataInfo.ApplicationAccessDropDownInformation.p_company_list;
                        if (!string.IsNullOrEmpty(cleanedNewPlaceHolderValue) && DataInfo.ApplicationAccessDropDownInformation.p_company_list != null)
                        {
                            var filteredSuggestions = SuggestionsItem.Cast<PCompanyList>()
                               .Where(x => Regex.Replace(x.company_name.ToLowerInvariant(), @"\s+", string.Empty).Contains(cleanedNewPlaceHolderValue))
                               .OrderByDescending(x => Regex.Replace(x.ToString()
                               .ToLowerInvariant(), @"\s+", string.Empty)
                               .StartsWith(cleanedNewPlaceHolderValue, StringComparison.CurrentCulture)).ToList();
                            viewModel.ApplicationAccessDropDownInformation = new ApplicationAccessDropDownResponse();
                            viewModel.ApplicationAccessDropDownInformation.p_company_list = new List<PCompanyList>();
                            if (filteredSuggestions.Count > 0)
                            {
                                foreach (var suggestion in filteredSuggestions)
                                {
                                    viewModel.ApplicationAccessDropDownInformation.p_company_list.Add(suggestion);
                                }
                            }
                        }
                        else
                        {
                            viewModel.ApplicationAccessDropDownInformation = DataInfo.ApplicationAccessDropDownInformation;
                        }
                    }
                    else
                    {
                        viewModel.ApplicationAccessDropDownInformation = DataInfo.ApplicationAccessDropDownInformation;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    viewModel.ApplicationAccessDropDownInformation = DataInfo.ApplicationAccessDropDownInformation;
                }
            };

            EnvironmentList.ItemTapped += (sender, e) =>
            {
                if (viewModel.SearchEnvironmentView)
                {
                    viewModel.SearchEnvironmentView = false;
                    viewModel.EnvironmentRequired = viewModel.SelectedEnvironmentItem.evinronment_type;
                }
               //((ListView)sender).SelectedItem = null;
            };

            SearchEnvironmentText.TextChanged += (sender, e) =>
            {
                try
                {
                    string newValue = e.NewTextValue;
                    if (newValue.ToString().Length >= 3)
                    {
                        SearchEnvironmentText.IsEnabled = !string.IsNullOrEmpty(newValue.ToString());

                        var cleanedNewPlaceHolderValue = Regex.Replace((newValue.ToString() ?? string.Empty).ToLowerInvariant(), @"\s+", string.Empty);

                        IEnumerable SuggestionsItem = DataInfo.ApplicationAccessDropDownInformation.p_environment_list;
                        if (!string.IsNullOrEmpty(cleanedNewPlaceHolderValue) && DataInfo.ApplicationAccessDropDownInformation.p_environment_list != null)
                        {
                            var filteredSuggestions = SuggestionsItem.Cast<PEnvironmentList>()
                               .Where(x => Regex.Replace(x.evinronment_type.ToLowerInvariant(), @"\s+", string.Empty).Contains(cleanedNewPlaceHolderValue))
                               .OrderByDescending(x => Regex.Replace(x.ToString()
                               .ToLowerInvariant(), @"\s+", string.Empty)
                               .StartsWith(cleanedNewPlaceHolderValue, StringComparison.CurrentCulture)).ToList();
                            viewModel.ApplicationAccessDropDownInformation = new ApplicationAccessDropDownResponse();
                            viewModel.ApplicationAccessDropDownInformation.p_environment_list = new List<PEnvironmentList>();
                            if (filteredSuggestions.Count > 0)
                            {
                                foreach (var suggestion in filteredSuggestions)
                                {
                                    viewModel.ApplicationAccessDropDownInformation.p_environment_list.Add(suggestion);
                                }
                            }
                        }
                        else
                        {
                            viewModel.ApplicationAccessDropDownInformation = DataInfo.ApplicationAccessDropDownInformation;
                        }
                    }
                    else
                    {
                        viewModel.ApplicationAccessDropDownInformation = DataInfo.ApplicationAccessDropDownInformation;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    viewModel.ApplicationAccessDropDownInformation = DataInfo.ApplicationAccessDropDownInformation;
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
