using System;
using System.ComponentModel;
using System.Diagnostics;
using HassadFood.Interface;
using HassadFood.WebService;
using Xamarin.Forms;

namespace HassadFood.ViewModels
{
    public class LeaveAmendmentModel : ViewModelBase
    {
        Page _page;
        IUserDetails userDetails;
        public LeaveAmendmentModel(Page page) : base(page)
        {
            Title = "Leave Amendment Page";
            userDetails = DependencyService.Get<IUserDetails>();
            _page = page;
            ExitPermit = "No";
            NewLeaveStartDate = DateTime.Now;
            NewLeaveEndDate = DateTime.Now;
            SubmissionDate = DateTime.Now;
            NewTravelDate = DateTime.Now;
            GetLeaveAppliedDates();
        }

        string appliedLeaveDates;

        public string AppliedLeaveDates
        {
            get { return appliedLeaveDates; }
            set { SetProperty(ref appliedLeaveDates, value); }
        }

        DateTime newLeaveStartDate;

        public DateTime NewLeaveStartDate
        {
            get { return newLeaveStartDate; }
            set { SetProperty(ref newLeaveStartDate, value); }
        }

        DateTime newLeaveEndDate;

        public DateTime NewLeaveEndDate
        {
            get { return newLeaveEndDate; }
            set { SetProperty(ref newLeaveEndDate, value); }
        }

        string exitPermit;

        public string ExitPermit
        {
            get { return exitPermit; }
            set { SetProperty(ref exitPermit, value); }
        }

        DateTime newTravelDate;

        public DateTime NewTravelDate
        {
            get { return newTravelDate; }
            set { SetProperty(ref newTravelDate, value); }
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
                await page.DisplayAlert("Alert", "Please select applied leave dates.", "OK");
                return;
            }

            if (Justification == null || Justification == "")
            {
                await page.DisplayAlert("Alert", "Please enter justification.", "OK");
                return;
            }

            string showAlert = null;
            int attendanceID = 0;
            foreach(var item in GetLeaveAppliedDatesInformation.items)
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

            LeaveAmendmentRequest request = new LeaveAmendmentRequest
            {
                p_user_name = Application.Current.Properties["OracleUserName"] as string,
                p_abs_attendance_id = attendanceID,
                p_new_leave_start_date = NewLeaveStartDate.ToString(format: "dd-MMM-yyyy"),
                p_new_leave_end_date = NewLeaveEndDate.ToString(format: "dd-MMM-yyyy"),
                p_exit_permit = ExitPermit,
                p_new_travel_date = NewTravelDate.ToString(format: "dd-MMM-yyyy"),
                p_justification = Justification,
                p_submission_date = SubmissionDate.ToString(format: "dd-MMM-yyyy")
            };

            try
            {
                var result = await userDetails.PutLeaveAmendmentRequest(request);
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
