using System;
using System.ComponentModel;
using System.Diagnostics;
using HassadFood.Interface;
using HassadFood.WebService;
using Xamarin.Forms;

namespace HassadFood.ViewModels
{
    public class ExitPermitRequestModel : ViewModelBase
    {
        Page _page;
        IUserDetails userDetails;
        public ExitPermitRequestModel(Page page) : base(page)
        {
            Title = "Exit Permit Request Page";
            userDetails = DependencyService.Get<IUserDetails>();
            _page = page;
            FlightDateRq = DateTime.Now;
            ReturnDate = DateTime.Now;
            SubmissionDate = DateTime.Now;
        }

        DateTime flightDateRq;

        public DateTime FlightDateRq
        {
            get { return flightDateRq; }
            set { SetProperty(ref flightDateRq, value); }
        }

        DateTime returnDate;

        public DateTime ReturnDate
        {
            get { return returnDate; }
            set { SetProperty(ref returnDate, value); }
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
            string showAlert = null;

            if (IsBusy)
                return;

            IsBusy = true;

            ExitPermitRequest request = new ExitPermitRequest
            {
                p_user_name = Application.Current.Properties["OracleUserName"] as string,
                p_flight_date = FlightDateRq.ToString(format: "dd-MMM-yyyy"),
                p_return_date = ReturnDate.ToString(format: "dd-MMM-yyyy"),
                p_airline_flight_no = AirlineFlightNumber,
                p_flight_travel_time = FlightTravelTime,
                p_justification = Justification, 
                p_submission_date = SubmissionDate.ToString(format: "dd-MMM-yyyy")
            };

            try
            {
                var result = await userDetails.PutExitPermitRequest(request);
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
    }
}
