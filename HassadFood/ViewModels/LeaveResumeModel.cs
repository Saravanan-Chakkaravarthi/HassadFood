using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;
using HassadFood.Interface;
using HassadFood.WebService;
using Xamarin.Forms;

namespace HassadFood.ViewModels
{
    public class LeaveResumeModel : ViewModelBase
    {
        Page _page;
        IUserDetails userDetails;
        List<string> RemarksRqItem;
        public LeaveResumeModel(Page page) : base(page)
        {
            Title = "Leave Resume Page";
            userDetails = DependencyService.Get<IUserDetails>();
            _page = page;
            RemarksRqItem = new List<string>();
            RemarksRqItem.Add("As Planned");
            RemarksRqItem.Add("Earlier than Planned");
            RemarksRqItem.Add("Later than Planned");
            ResumptionDate = DateTime.Now;
            GetLeaveAppliedDates();
        }

        public List<string> RemarksItem
        {
            get { return RemarksRqItem; }
            set { SetProperty(ref RemarksRqItem, value); }
        }

        string appliedLeaveDates;

        public string AppliedLeaveDates
        {
            get { return appliedLeaveDates; }
            set { SetProperty(ref appliedLeaveDates, value); }
        }

        string remarksRq;

        public string RemarksRq
        {
            get { return remarksRq; }
            set { SetProperty(ref remarksRq, value); }
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

        DateTime resumptionDate;

        public DateTime ResumptionDate
        {
            get { return resumptionDate; }
            set 
            { 
                SetProperty(ref resumptionDate, value);
                if (AppliedLeaveDates != null)
                {
                    string[] dates = Regex.Split(AppliedLeaveDates.ToLower(), " to ");
                    if (DateTime.ParseExact(dates[0], "dd-MMM-yyyy", CultureInfo.InvariantCulture) > ResumptionDate)
                    {
                        page.DisplayAlert("Alert", "Resumption date should not be prior to Leave Start Date.", "OK");
                    }
                    else if (ResumptionDate > DateTime.Now)
                    {
                        page.DisplayAlert("Alert", "Resumption date should not be greater than Current Date.", "OK");
                    }
                }
            }
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

            if (RemarksRq == null || RemarksRq == "")
            {
                await page.DisplayAlert("Alert", "Please select remarks.", "OK");
                return;
            }

            if (Justification == null || Justification == "")
            {
                await page.DisplayAlert("Alert", "Please enter justification.", "OK");
                return;
            }

            string[] dates = Regex.Split(AppliedLeaveDates.ToLower(), " to ");
            if (DateTime.ParseExact(dates[0], "dd-MMM-yyyy", CultureInfo.InvariantCulture) > ResumptionDate)
            {
                await page.DisplayAlert("Alert", "Resumption date should not be prior to Leave Start Date.", "OK");
                return;
            }
            if (ResumptionDate > DateTime.Now)
            {
                await page.DisplayAlert("Alert", "Resumption date should not be greater than Current Date.", "OK");
                return;
            }


            string showAlert = null;
            string attendanceID = "";
            foreach (var item in GetLeaveAppliedDatesInformation.items)
            {
                if (AppliedLeaveDates == item.date_start_end)
                {
                    attendanceID = item.absence_attendance_id.ToString();
                    break;
                }
            }

            if (IsBusy)
                return;

            IsBusy = true;

            LeaveResumeRequest request = new LeaveResumeRequest
            {
                p_user_name = Application.Current.Properties["OracleUserName"] as string,
                p_abs_attendance_id = attendanceID,
                p_remarks = RemarksRq,
                p_justification = Justification,
                p_submission_date = ResumptionDate.ToString(format: "dd-MMM-yyyy")
            };

            try
            {
                var result = await userDetails.PutLeaveResumeRequest(request);
                if (result)
                {
                    if (DataInfo.RequestInformation != null)
                    {
                        if (DataInfo.RequestInformation.p_success_flag == "Y")
                        {
                            showAlert = "Success";
                            PutAttachmentsRequest();
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
