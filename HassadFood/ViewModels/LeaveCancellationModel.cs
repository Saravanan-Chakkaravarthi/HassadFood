using System;
using System.ComponentModel;
using System.Diagnostics;
using HassadFood.Interface;
using HassadFood.WebService;
using Xamarin.Forms;

namespace HassadFood.ViewModels
{
    public class LeaveCancellationModel : ViewModelBase
    {
        Page _page;
        IUserDetails userDetails;
        public LeaveCancellationModel(Page page) : base(page)
        {
            Title = "Leave Cancel Page";
            userDetails = DependencyService.Get<IUserDetails>();
            _page = page;
            SubmissionDate = DateTime.Now;
            GetLeaveAppliedDates();
        }

        string appliedLeaveDates;

        public string AppliedLeaveDates
        {
            get { return appliedLeaveDates; }
            set { SetProperty(ref appliedLeaveDates, value); }
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

        string additionalComments = string.Empty;

        public string AdditionalComments
        {
            get
            {
                return additionalComments;
            }
            set
            {
                if (additionalComments != value)
                {
                    additionalComments = value;
                    OnPropertyChanged("AdditionalComments");
                }
            }
        }

        DateTime submissionDate;

        public DateTime SubmissionDate
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

        public Command SubmitCommand => _SubmitCommand ?? (_SubmitCommand = new Command(() => ExecuteSubmitCommand()));

        async void ExecuteSubmitCommand()
        {
            if (AppliedLeaveDates == null || AppliedLeaveDates == "")
            {
                await page.DisplayAlert("Alert", "Please select leave to be cancelled.", "OK");
                return;
            }

            if (Justification == null || Justification == "")
            {
                await page.DisplayAlert("Alert", "Please enter justification.", "OK");
                return;
            }

            string showAlert = null;
            int attendanceID = 0;
            foreach (var item in GetLeaveAppliedDatesInformation.items)
            {
                if (AppliedLeaveDates == item.date_start_end)
                {
                    attendanceID = item.absence_attendance_id;
                    break;
                }
            }

            if (IsBusy)
                return;

            IsBusy = true;

            LeaveCancelRequest request = new LeaveCancelRequest
            {
                p_user_name = Application.Current.Properties["OracleUserName"] as string,
                p_abs_attendance_id = attendanceID,
                p_justification = Justification,
                p_add_comments = AdditionalComments,
                p_submission_date = SubmissionDate.ToString(format: "dd-MMM-yyyy")
            };

            try
            {
                var result = await userDetails.PutLeaveCancelRequest(request);
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
                        await page.DisplayAlert("Alert", "Unable to submit request.", "OK");
                        break;
                    case "Success":
                        await page.DisplayAlert("Alert", DataInfo.RequestInformation.p_record, "OK");
                        await page.Navigation.PopAsync();
                        DataInfo.WorkListInformation = null;
                        break;
                    case "Failed":
                        await page.DisplayAlert("Alert", "Unable to submit request.", "OK");
                        break;
                }
            }
        }

        async void GetLeaveAppliedDates()
        {
            string showAlert = null;

            if (IsListBusy)
                return;

            IsListBusy = true;

            try
            {
                var result = await userDetails.GetLeaveAppliedDates();
                if (result)
                {
                    GetLeaveAppliedDatesInformation = DataInfo.GetLeaveAppliedDatesInformation;
                    if (GetLeaveAppliedDatesInformation != null && GetLeaveAppliedDatesInformation.items.Count > 0)
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
    }
}
