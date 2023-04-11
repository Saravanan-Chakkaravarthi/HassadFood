using System;
using System.ComponentModel;
using System.Diagnostics;
using HassadFood.Interface;
using HassadFood.WebService;
using Xamarin.Forms;

namespace HassadFood.ViewModels
{
    public class SalaryAdvanceRequestModel : ViewModelBase
    {
        Page _page;
        IUserDetails userDetails;
        public SalaryAdvanceRequestModel(Page page) : base(page)
        {
            Title = "Salary Advance Request Page";
            userDetails = DependencyService.Get<IUserDetails>();
            _page = page;
            SubmissionDate = DateTime.Now;
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
            if (Justification == null || Justification == "")
            {
                await page.DisplayAlert("Alert", "Justification cannot be empty.", "OK");
                return;
            }


            string showAlert = null;

            if (IsBusy)
                return;

            IsBusy = true;

            SalaryAdvanceServiceRequest request = new SalaryAdvanceServiceRequest
            {
                p_user_name = Application.Current.Properties["OracleUserName"] as string,
                p_justification = Justification,
                p_submission_date = SubmissionDate.Value.ToString(format: "dd-MMM-yyyy")
            };

            try
            {
                var result = await userDetails.PutSalaryAdvanceRequest(request);
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

        async void GetAbsenceType()
        {
            string showAlert = null;

            if (IsListBusy)
                return;

            IsListBusy = true;

            try
            {
                var result = await userDetails.GetAbsenceType();
                if (result)
                {
                    GetAbsenceTypeInformation = DataInfo.GetAbsenceTypeInformation;
                    if (GetAbsenceTypeInformation != null && GetAbsenceTypeInformation.get_absence_types_p.Count > 0)
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
