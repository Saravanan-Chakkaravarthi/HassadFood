using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using HassadFood.Interface;
using HassadFood.WebService;
using Xamarin.Forms;

namespace HassadFood.ViewModels
{
    public class DutyVisitEmployeeModel : ViewModelBase
    {
        Page _page;
        IUserDetails userDetails;
        List<string> AccommodationList, TicketList;
        public DutyVisitEmployeeModel(Page page) : base(page)
        {
            Title = "Duty Visit Employee Page";
            userDetails = DependencyService.Get<IUserDetails>();
            _page = page;
            DateofTravel = DateTime.Now;
            FirstDayoftheDutyVisit = DateTime.Now;
            LastDayoftheDutyVisit = DateTime.Now;
            ArrivalDate = DateTime.Now;
            AccommodationList = new List<string>();
            AccommodationList.Add("Own Arrangement");
            AccommodationList.Add("Provided by the Host");
            AccommodationList.Add("To be arranged by Hassad");
            TicketList = new List<string>();
            TicketList.Add("Own Arrangement");
            TicketList.Add("Provided by the Host");
            TicketList.Add("To be arranged by Hassad");
            SelectedPerdiemPaidbyHost = "No";
            SelectedExitPermit = "No";
            SelectedBudget = "No";
            SubmissionDate = DateTime.Now;
            PerdiemAmount = "2000";
            ClassofTravel = "E";
            GetDutyVisitPerDiemClass();
        }

        public List<string> AccommodationListItem
        {
            get { return AccommodationList; }
            set { SetProperty(ref AccommodationList, value); }
        }

        public List<string> TicketListItem
        {
            get { return TicketList; }
            set { SetProperty(ref TicketList, value); }
        }

        string perdiemAmount;

        public string PerdiemAmount
        {
            get { return perdiemAmount; }
            set { SetProperty(ref perdiemAmount, value); }
        }

        string classofTravel;

        public string ClassofTravel
        {
            get { return classofTravel; }
            set { SetProperty(ref classofTravel, value); }
        }

        DateTime dateofTravel;

        public DateTime DateofTravel
        {
            get { return dateofTravel; }
            set { SetProperty(ref dateofTravel, value); }
        }

        DateTime firstDayoftheDutyVisit;

        public DateTime FirstDayoftheDutyVisit
        {
            get { return firstDayoftheDutyVisit; }
            set { SetProperty(ref firstDayoftheDutyVisit, value); }
        }

        DateTime lastDayoftheDutyVisit;

        public DateTime LastDayoftheDutyVisit
        {
            get { return lastDayoftheDutyVisit; }
            set { SetProperty(ref lastDayoftheDutyVisit, value); }
        }

        DateTime arrivalDate;

        public DateTime ArrivalDate
        {
            get { return arrivalDate; }
            set { SetProperty(ref arrivalDate, value); }
        }

        string selectedExitPermit;

        public string SelectedExitPermit
        {
            get { return selectedExitPermit; }
            set { SetProperty(ref selectedExitPermit, value); }
        }

        string destinationRoute = string.Empty;

        public string DestinationRoute
        {
            get
            {
                return destinationRoute;
            }
            set
            {
                if (destinationRoute != value)
                {
                    destinationRoute = value;
                    OnPropertyChanged("DestinationRoute");
                }
            }
        }

        string selectedBudget;

        public string SelectedBudget
        {
            get { return selectedBudget; }
            set { SetProperty(ref selectedBudget, value); }
        }

        string purposeofVisitRemarks = string.Empty;

        public string PurposeofVisitRemarks
        {
            get
            {
                return purposeofVisitRemarks;
            }
            set
            {
                if (purposeofVisitRemarks != value)
                {
                    purposeofVisitRemarks = value;
                    OnPropertyChanged("PurposeofVisitRemarks");
                }
            }
        }

        string selectedAccomodation;

        public string SelectedAccomodation
        {
            get { return selectedAccomodation; }
            set { SetProperty(ref selectedAccomodation, value); }
        }

        string selectedTicket;

        public string SelectedTicket
        {
            get { return selectedTicket; }
            set { SetProperty(ref selectedTicket, value); }
        }

        string selectedPerdiemPaidbyHost;

        public string SelectedPerdiemPaidbyHost
        {
            get { return selectedPerdiemPaidbyHost; }
            set { SetProperty(ref selectedPerdiemPaidbyHost, value); }
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
            if (PurposeofVisitRemarks == null || PurposeofVisitRemarks == "")
            {
                await page.DisplayAlert("Alert", "Please enter purpose of visit / remarks.", "OK");
                return;
            }

            if (SelectedAccomodation == null || SelectedAccomodation == "")
            {
                await page.DisplayAlert("Alert", "Please select accomodation.", "OK");
                return;
            }

            if (SelectedTicket == null || SelectedTicket == "")
            {
                await page.DisplayAlert("Alert", "Please select ticket.", "OK");
                return;
            }

            string showAlert = null;

            if (IsBusy)
                return;

            IsBusy = true;

            DutyVisitRequest request = new DutyVisitRequest
            {
                p_user_name = Application.Current.Properties["OracleUserName"] as string,
                p_perdiem_amt = PerdiemAmount,
                p_class_of_travel = ClassofTravel,
                p_date_of_travel = DateofTravel.ToString(format: "dd-MMM-yyyy"),
                p_fday_visit = FirstDayoftheDutyVisit.ToString(format: "dd-MMM-yyyy"),
                p_lday_visit = LastDayoftheDutyVisit.ToString(format: "dd-MMM-yyyy"),
                p_arrival_date = ArrivalDate.ToString(format: "dd-MMM-yyyy"),
                p_exit_permit = SelectedExitPermit,
                p_destination = DestinationRoute,
                p_budget = SelectedBudget,
                p_purpose = PurposeofVisitRemarks,
                p_accomodation = SelectedAccomodation,
                p_ticket = SelectedTicket,
                p_perdiem_amt_host = SelectedPerdiemPaidbyHost,
                p_submission_date = SubmissionDate.ToString(format: "dd-MMM-yyyy")
            };

            try
            {
                var result = await userDetails.PutDutyVisitRequest(request);
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

        async void GetDutyVisitPerDiemClass()
        {
            string showAlert = null;

            if (IsListBusy)
                return;

            IsListBusy = true;

            try
            {
                var result = await userDetails.GetDutyVisitPerDiemClass();
                if (result)
                {
                    DutyVisitPerDiemClassInformation = DataInfo.DutyVisitPerDiemClassInformation;
                    if (DutyVisitPerDiemClassInformation != null)
                    {
                        showAlert = "Success";
                        PerdiemAmount = DutyVisitPerDiemClassInformation.p_per_diam.ToString();
                        ClassofTravel = DutyVisitPerDiemClassInformation.p_class;
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
