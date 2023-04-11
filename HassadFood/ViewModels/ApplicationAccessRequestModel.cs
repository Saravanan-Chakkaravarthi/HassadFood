using System;
using System.ComponentModel;
using System.Diagnostics;
using HassadFood.Interface;
using HassadFood.WebService;
using Xamarin.Forms;

namespace HassadFood.ViewModels
{
    public class ApplicationAccessRequestModel : ViewModelBase
    {
        Page _page;
        IUserDetails userDetails;
        public ApplicationAccessRequestModel(Page page) : base(page)
        {
            Title = "Application Access Request Page";
            userDetails = DependencyService.Get<IUserDetails>();
            _page = page;
            applicationAccess = "Select Access";
            responsibilityName = "Select Responsibility";
            companyName = "Select Company";
            environmentRequired = "Select Environment";
            SubmissionDate = DateTime.Now;
            GetApplicationAccessDropDown();
        }

        PApplicationaccessList selectedApplicationItem;

        public PApplicationaccessList SelectedApplicationItem
        {
            get { return selectedApplicationItem; }
            set 
            { 
                SetProperty(ref selectedApplicationItem, value);
                if (selectedApplicationItem != null)
                    GetApplicationAccessRName(selectedApplicationItem.application_id.ToString());
            }
        }

        string applicationAccess;

        public string ApplicationAccess
        {
            get { return applicationAccess; }
            set { SetProperty(ref applicationAccess, value); }
        }

        string searchApplicationText;

        public string SearchApplicationText
        {
            get { return searchApplicationText; }
            set { if (searchApplicationText != value) { searchApplicationText = value; OnPropertyChanged("SearchApplicationText"); } }
        }

        bool searchApplicationView;

        public bool SearchApplicationView
        {
            get { return searchApplicationView; }
            set { SetProperty(ref searchApplicationView, value); }
        }

        PRnameList selectedResponsibilityItem;

        public PRnameList SelectedResponsibilityItem
        {
            get { return selectedResponsibilityItem; }
            set { SetProperty(ref selectedResponsibilityItem, value); }
        }

        string responsibilityName;

        public string ResponsibilityName
        {
            get { return responsibilityName; }
            set { SetProperty(ref responsibilityName, value); }
        }

        string searchResponsibilityText;

        public string SearchResponsibilityText
        {
            get { return searchResponsibilityText; }
            set { if (searchResponsibilityText != value) { searchApplicationText = value; OnPropertyChanged("SearchResponsibilityText"); } }
        }

        bool searchResponsibilityView;

        public bool SearchResponsibilityView
        {
            get { return searchResponsibilityView; }
            set { SetProperty(ref searchResponsibilityView, value); }
        }

        PCompanyList selectedCompanyItem;

        public PCompanyList SelectedCompanyItem
        {
            get { return selectedCompanyItem; }
            set { SetProperty(ref selectedCompanyItem, value); }
        }

        string companyName;

        public string CompanyName
        {
            get { return companyName; }
            set { SetProperty(ref companyName, value); }
        }

        string searchCompanyText;

        public string SearchCompanyText
        {
            get { return searchCompanyText; }
            set { if (searchCompanyText != value) { searchCompanyText = value; OnPropertyChanged("SearchCompanyText"); } }
        }

        bool searchCompanyView;

        public bool SearchCompanyView
        {
            get { return searchCompanyView; }
            set { SetProperty(ref searchCompanyView, value); }
        }

        PEnvironmentList selectedEnvironmentItem;

        public PEnvironmentList SelectedEnvironmentItem
        {
            get { return selectedEnvironmentItem; }
            set { SetProperty(ref selectedEnvironmentItem, value); }
        }

        string environmentRequired;

        public string EnvironmentRequired
        {
            get { return environmentRequired; }
            set { SetProperty(ref environmentRequired, value); }
        }

        string searchEnvironmentText;

        public string SearchEnvironmentText
        {
            get { return searchEnvironmentText; }
            set { if (searchEnvironmentText != value) { searchEnvironmentText = value; OnPropertyChanged("SearchEnvironmentText"); } }
        }

        bool searchEnvironmentView;

        public bool SearchEnvironmentView
        {
            get { return searchEnvironmentView; }
            set { SetProperty(ref searchEnvironmentView, value); }
        }

        string selectedUserName;

        public string SelectedUserName
        {
            get { return selectedUserName; }
            set { SetProperty(ref selectedUserName, value); }
        }

        string userName = string.Empty;

        public string UserName
        {
            get
            {
                return userName;
            }
            set
            {
                if (userName != value)
                {
                    userName = value;
                    OnPropertyChanged("UserName");
                }
            }
        }

        string justification = string.Empty;

        public string Justification
        {
            get
            {
                return justification;
            }
            set
            {
                if (justification != value)
                {
                    justification = value;
                    OnPropertyChanged("Justification");
                }
            }
        }

        DateTime? submissionDate = null;

        public DateTime? SubmissionDate
        {
            get { return submissionDate; }
            set { SetProperty(ref submissionDate, value); }
        }

        public new event PropertyChangedEventHandler PropertyChanged;

        protected virtual new void OnPropertyChanged(string propertyName)
        {
            var changed = PropertyChanged;
            if (changed != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        Command _SubmitCommand;

        public Command SubmitCommand => _SubmitCommand ?? (_SubmitCommand = new Command(() => SubmitAlertCommand()));

        Command _SearchApplicationCommand;

        public Command SearchApplicationCommand => _SearchApplicationCommand ?? (_SearchApplicationCommand = new Command(() => ExecuteSearchApplicationCommand()));

        async void ExecuteSearchApplicationCommand()
        {
            SearchApplicationView = !SearchApplicationView;
        }

        Command _SearchResponsibilityCommand;

        public Command SearchResponsibilityCommand => _SearchResponsibilityCommand ?? (_SearchResponsibilityCommand = new Command(() => ExecuteSearchResponsibilityCommand()));

        async void ExecuteSearchResponsibilityCommand()
        {
            SearchResponsibilityView = !SearchResponsibilityView;
        }

        Command _SearchCompanyCommand;

        public Command SearchCompanyCommand => _SearchCompanyCommand ?? (_SearchCompanyCommand = new Command(() => ExecuteSearchCompanyCommand()));

        async void ExecuteSearchCompanyCommand()
        {
            SearchCompanyView = !SearchCompanyView;
        }

        Command _SearchEnvironmentCommand;

        public Command SearchEnvironmentCommand => _SearchEnvironmentCommand ?? (_SearchEnvironmentCommand = new Command(() => ExecuteSearchEnvironmentCommand()));

        async void ExecuteSearchEnvironmentCommand()
        {
            SearchEnvironmentView = !SearchEnvironmentView;
        }

        async void SubmitAlertCommand()
        {
            var alert = await page.DisplayAlert("Alert", "Do you want to confirm to submit?", "OK", "CANCEL");
            if (alert)
                ExecuteSubmitCommand();
            else
                return;
        }

        async void ExecuteSubmitCommand()
        {
            if (UserName == null || UserName == "")
            {
                await page.DisplayAlert("Alert", "Username cannot be empty.", "OK");
                return;
            }

            if (CompanyName == "Select Company")
            {
                await page.DisplayAlert("Alert", "Company name cannot be empty.", "OK");
                return;
            }

            if (EnvironmentRequired == "Select Environment")
            {
                await page.DisplayAlert("Alert", "Environment required cannot be empty.", "OK");
                return;
            }

            if (Justification == null || Justification == "")
            {
                await page.DisplayAlert("Alert", "Justification cannot be empty.", "OK");
                return;
            }

            string showAlert = null;

            if (IsBusy)
                return;

            IsBusy = true;

            ApplicationAccessServiceRequest request = new ApplicationAccessServiceRequest
            {
                p_user_name = Application.Current.Properties["OracleUserName"] as string,
                p_application_access = SelectedApplicationItem.application_id.ToString(),
                p_responsibility_name = ResponsibilityName,
                p_company_name = CompanyName,
                p_environment_required = EnvironmentRequired,
                p_application_user_name = UserName,
                p_justification = Justification,
                p_submission_date = SubmissionDate.Value.ToString(format: "dd-MMM-yyyy")
            };

            try
            {
                var result = await userDetails.PutApplicationAccessRequest(request);
                if (result)
                {
                    if (DataInfo.RequestInformation != null)
                    {
                        if (DataInfo.RequestInformation.p_success_flag == "Y")
                        {
                            showAlert = "Success";
                            await PutAttachmentsRequest();
                        }
                        else
                            showAlert = "Failed";
                    }
                    else
                        showAlert = "Error";
                }
                else
                    showAlert = "Error";
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                showAlert = "Error";
            }
            finally
            {
                IsBusy = false;

                switch (showAlert)
                {
                    case "Error":
                        await page.DisplayAlert("Alert", "Unable to submit leave.", "OK");
                        break;
                    case "Success":
                        await page.DisplayAlert("Alert", DataInfo.RequestInformation.p_error_msg, "OK");
                        await page.Navigation.PopAsync();
                        DataInfo.WorkListInformation = null;
                        break;
                    case "Failed":
                        await page.DisplayAlert("Alert", DataInfo.RequestInformation.p_error_msg, "OK");
                        break;
                }
            }
        }

        public async void GetApplicationAccessDropDown()
        {
            string showAlert = null;

            if (IsListBusy)
                return;

            IsListBusy = true;

            try
            {
                var result = await userDetails.GetApplicationAccessDropDown();
                if (result)
                {
                    ApplicationAccessDropDownInformation = DataInfo.ApplicationAccessDropDownInformation;
                    if (ApplicationAccessDropDownInformation != null && ApplicationAccessDropDownInformation.p_username_list.Count > 0)
                    {
                        showAlert = "Success";
                    }
                    else
                        showAlert = "Failed";
                }
                else
                    showAlert = "Error";
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                showAlert = "Error";
            }
            finally
            {
                IsListBusy = false;

                switch (showAlert)
                {
                    case "Error":
                        break;
                    case "Success":
                        break;
                    case "Failed":
                        break;
                }
            }
        }

        async void GetApplicationAccessRName(string applicationid)
        {
            string showAlert = null;

            if (IsListBusy)
                return;

            IsListBusy = true;

            try
            {
                var result = await userDetails.GetApplicationAccessRName(applicationid);
                if (result)
                {
                    ApplicationAccessRNameInformation = DataInfo.ApplicationAccessRNameInformation;
                    if (ApplicationAccessRNameInformation != null && ApplicationAccessRNameInformation.p_rname_list.Count > 0)
                    {
                        showAlert = "Success";
                    }
                    else
                        showAlert = "Failed";
                }
                else
                    showAlert = "Error";
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                showAlert = "Error";
            }
            finally
            {
                IsListBusy = false;
                //GetReassignUser();

                switch (showAlert)
                {
                    case "Error":
                        break;
                    case "Success":
                        break;
                    case "Failed":
                        break;
                }
            }
        }
    }
}
