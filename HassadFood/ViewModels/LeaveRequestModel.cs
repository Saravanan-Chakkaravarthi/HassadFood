using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using HassadFood.Interface;
using HassadFood.WebService;
using Xamarin.Forms;

namespace HassadFood.ViewModels
{
    public class LeaveRequestModel : ViewModelBase
    {
        Page _page;
        IUserDetails userDetails;
        public LeaveRequestModel(Page page) : base(page)
        {
            Title = "Leave Request Page";
            userDetails = DependencyService.Get<IUserDetails>();
            _page = page;
            SelectedExitPermit = "No";
            SelectedAirTicket = "No";
            SelectedLeaveAdvance = "No";
            Comments = "";
            ReassignUser = "Select User";
            CalculatedLeaveDuration = "0";
            _selectedLeaveStartDateItem = DateTime.Now;
            _selectedLeaveEndDateItem = DateTime.Now;
            GetAbsenceType();
        }

        DateTime _selectedLeaveStartDateItem, _selectedLeaveEndDateItem;

        public DateTime SelectedLeaveStartDateItem
        {
            get { return _selectedLeaveStartDateItem; }
            set 
            { 
                SetProperty(ref _selectedLeaveStartDateItem, value);
                if (_selectedLeaveStartDateItem != null && SelectedItemAbsenceType != null)
                    ExecuteCalculateDurationCommand();
            }
        }

        public DateTime SelectedLeaveEndDateItem
        {
            get { return _selectedLeaveEndDateItem; }
            set 
            {
                SetProperty(ref _selectedLeaveEndDateItem, value); 
                if (_selectedLeaveEndDateItem != null && SelectedItemAbsenceType != null)
                    ExecuteCalculateDurationCommand();
            }
        }

        GetReassignUser selectedReassignItem;

        public GetReassignUser SelectedReassignItem
        {
            get { return selectedReassignItem; }
            set { SetProperty(ref selectedReassignItem, value); }
        }

        string reassignName;

        public string ReassignName
        {
            get { return reassignName; }
            set { SetProperty(ref reassignName, value); }
        }

        string searchText;

        public string SearchText
        {
            get { return searchText; }
            set { if (searchText != value) { searchText = value; OnPropertyChanged("SearchText"); } }
        }

        string reassignUser;

        public string ReassignUser
        {
            get { return reassignUser; }
            set { SetProperty(ref reassignUser, value); }
        }

        bool searchView;

        public bool SearchView
        {
            get { return searchView; }
            set { SetProperty(ref searchView, value); }
        }

        string selectedExitPermit;

        public string SelectedExitPermit
        {
            get { return selectedExitPermit; }
            set { SetProperty(ref selectedExitPermit, value); }
        }

        string selectedAirTicket;

        public string SelectedAirTicket
        {
            get { return selectedAirTicket; }
            set { SetProperty(ref selectedAirTicket, value); }
        }

        string selectedLeaveAdvance;

        public string SelectedLeaveAdvance
        {
            get { return selectedLeaveAdvance; }
            set { SetProperty(ref selectedLeaveAdvance, value); }
        }

        string selectedItemAbsenceType;

        public string SelectedItemAbsenceType
        {
            get { return selectedItemAbsenceType; }
            set 
            { 
                SetProperty(ref selectedItemAbsenceType, value);
                if(selectedItemAbsenceType != null)
                    ExecuteCalculateDurationCommand();
            }
        }

        DateTime? flightDate = null;

        public DateTime? FlightDate
        {
            get { return flightDate; }
            set { SetProperty(ref flightDate, value); }
        }

        string airlineFlightNumber = string.Empty;

        public string AirlineFlightNumber
        {
            get
            {
                return airlineFlightNumber;
            }
            set
            {
                if (airlineFlightNumber != value)
                {
                    airlineFlightNumber = value;
                    OnPropertyChanged("AirlineFlightNumber");
                }
            }
        }

        string flightTravelTime = string.Empty;

        public string FlightTravelTime
        {
            get
            {
                return flightTravelTime;
            }
            set
            {
                if (flightTravelTime != value)
                {
                    flightTravelTime = value;
                    OnPropertyChanged("FlightTravelTime");
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

        public Command SubmitCommand => _SubmitCommand ?? (_SubmitCommand = new Command(() => SubmitAlertCommand()));

        Command _SearchCommand;

        public Command SearchCommand => _SearchCommand ?? (_SearchCommand = new Command(() => ExecuteSearchCommand()));

        async void ExecuteSearchCommand()
        {
            SearchView = !SearchView;
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
            //if (SelectedEndDateItem < DateTime.Now.AddDays(-1))
            //{
            //    await page.DisplayAlert("Alert", "End date should be greater than current date for future leave.", "OK");
            //    return;
            //}

            //if (CalculatedLeaveDuration == null || CalculatedLeaveDuration == "")
            //{
            //    await page.DisplayAlert("Alert", "Please calculate the days.", "OK");
            //    return;
            //}

            if(CalculatedLeaveDuration == "-1")
            {
                await page.DisplayAlert("Alert", "Please choose valid dates.", "OK");
                return;
            }

            if (CalculatedLeaveDuration == "0")
            {
                if (SelectedItemAbsenceType != "Leave Without Pay")
                {
                    await page.DisplayAlert("Alert", "You have applied for zero day of leave.", "OK");
                    return;
                }
            }

            //if (SelectedItemAbsenceType == "Leave Without Pay")
            //{
            //    if (CalculatedLeaveDuration != "0")
            //    {
            //        await page.DisplayAlert("Alert", "You cannot create Leave Without Pay while there is leave balance.", "OK");
            //        return;
            //    }
            //}

            if (SelectedItemAbsenceType == null)
            {
                await page.DisplayAlert("Alert", "Please choose the absence type.", "OK");
                return;
            }

            if (SelectedItemAbsenceType == "Absent")
            {
                await page.DisplayAlert("Alert", "You cannot create leave Type “Absent”.", "OK");
                return;
            }

            if (SelectedExitPermit == "Yes")
            {
                if (FlightDate == null)
                {
                    await page.DisplayAlert("Alert", "Flight date is mandatory.", "OK");
                    return;
                }
            }

            if (SelectedItemAbsenceType == "Casual Leave")
            {
                if (SelectedExitPermit == "Yes")
                {
                    await page.DisplayAlert("Alert", "You cannot put Exit Permit “Yes” for the Casual Leave.", "OK");
                    return;
                }
            }

            if (SelectedItemAbsenceType == "Sick Leave" || SelectedItemAbsenceType == "Formal Representation Leave" || SelectedItemAbsenceType == "Patient Escort Leave")
            {
                if (AttachmentsFiles.Count == 0)
                {
                    await page.DisplayAlert("Alert", "Attachment is required for the leave.", "OK");
                    return;
                }
            }

            string showAlert = null;

            if (IsBusy)
                return;

            IsBusy = true;

            LeaveRequest leave = new LeaveRequest
            {
                p_user_name = Application.Current.Properties["OracleUserName"] as string,
                p_absence_type = SelectedItemAbsenceType,
                p_start_date = SelectedLeaveStartDateItem.ToString(format: "dd-MMM-yyyy"),
                p_end_date = SelectedLeaveEndDateItem.ToString(format: "dd-MMM-yyyy"),
                p_user_comments = Comments,
                p_file_name = "",
                p_exit_permit_req = (SelectedExitPermit == "Yes") ? "Y" : "N",
                p_leave_advance = (SelectedLeaveAdvance == "Yes") ? "Y" : "N",
                p_replace_user = (ReassignUser == "Select User") ? "" : ReassignUser,
                p_air_ticket = (SelectedAirTicket == "Yes") ? "Y" : "N",
                p_flight_date = (FlightDate != null) ? FlightDate.Value.ToString(format: "dd-MMM-yyyy") : "",
                p_flight_number = AirlineFlightNumber,
                p_flight_time = FlightTravelTime
            };

            try
            {
                var result = await userDetails.PutLeaveRequest(leave);
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
                        await page.DisplayAlert("Alert", DataInfo.RequestInformation.p_record, "OK");
                        await page.Navigation.PopAsync();
                        DataInfo.WorkListInformation = null;
                        break;
                    case "Failed":
                        await page.DisplayAlert("Alert", DataInfo.RequestInformation.p_record, "OK");
                        break;
                }
            }
        }

        async void ExecuteCalculateDurationCommand()
        {
            //if (SelectedStartDateItem < SelectedEndDateItem)
            //{
            //    await page.DisplayAlert("Alert", "Error from date not should be less or greater than to date.", "OK");
            //    CalculatedLeaveDuration = "0";
            //    return;
            //}

            //Debug.WriteLine(SelectedStartDateItem.ToString() + SelectedEndDateItem.ToString() + SelectedHoursItem + SelectedMinutesItem);
            string showAlert = null;

            if (IsListBusy)
                return;

            IsListBusy = true;

            try
            {
                var result = await userDetails.CalculateAbsenceDuration(SelectedItemAbsenceType, SelectedLeaveStartDateItem, SelectedLeaveEndDateItem);
                if (result)
                {
                    CalculateAbsenceDurationInformation = DataInfo.CalculateAbsenceDurationInformation;
                    if (CalculateAbsenceDurationInformation != null)
                    {
                        CalculatedLeaveDuration = CalculateAbsenceDurationInformation.p_days.ToString();
                        showAlert = "Success";
                        if (CalculateAbsenceDurationInformation.p_days == 0)
                            await page.DisplayAlert("Alert", "You have applied for zero days of leave.", "OK");
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
                        await page.DisplayAlert("Alert", "Unable to calculate days.", "OK");
                        break;
                    case "Success":
                        break;
                    case "Failed":
                        await page.DisplayAlert("Alert", "Unable to calculate days.", "OK");
                        break;
                }
            }
        }

        public async void GetReassignUser()
        {
            string showAlert = null;

            if (IsListBusy)
                return;

            IsListBusy = true;

            try
            {
                var result = await userDetails.GetReassignUser();
                if (result)
                {
                    if (DataInfo.ReassignUserInformation != null && DataInfo.ReassignUserInformation.get_reassign_users.Count > 0)
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
