using System;
using System.ComponentModel;
using System.Diagnostics;
using HassadFood.Interface;
using HassadFood.Views;
using HassadFood.WebService;
using Xamarin.Forms;

namespace HassadFood.ViewModels
{
    public class CarLoanRequestModel : ViewModelBase
    {
        Page _page;
        IUserDetails userDetails;
        public CarLoanRequestModel(Page page) : base(page)
        {
            Title = "Car Loan Request Page";
            userDetails = DependencyService.Get<IUserDetails>();
            _page = page;
            SubmissionDate = DateTime.Now;
            GetCarLoanDropDown();
        }

        string mobileNumber = string.Empty;

        public string MobileNumber
        {
            get
            {
                return mobileNumber;
            }
            set
            {
                if (mobileNumber != value)
                {
                    mobileNumber = value;
                    OnPropertyChanged("MobileNumber");
                }
            }
        }

        string permanentResidenceAddress = string.Empty;

        public string PermanentResidenceAddress
        {
            get
            {
                return permanentResidenceAddress;
            }
            set
            {
                if (permanentResidenceAddress != value)
                {
                    permanentResidenceAddress = value;
                    OnPropertyChanged("PermanentResidenceAddress");
                }
            }
        }

        string postBoxNumber = string.Empty;

        public string PostBoxNumber
        {
            get
            {
                return postBoxNumber;
            }
            set
            {
                if (postBoxNumber != value)
                {
                    postBoxNumber = value;
                    OnPropertyChanged("PostBoxNumber");
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
            if (MobileNumber == null || MobileNumber == "")
            {
                await page.DisplayAlert("Alert", "Mobile number cannot be empty.", "OK");
                return;
            }

            if (PermanentResidenceAddress == null || PermanentResidenceAddress == "")
            {
                await page.DisplayAlert("Alert", "Permanent residence address cannot be empty.", "OK");
                return;
            }

            if (PostBoxNumber == null || PostBoxNumber == "")
            {
                await page.DisplayAlert("Alert", "Post box number cannot be empty.", "OK");
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

            CarLoanServiceRquest request = new CarLoanServiceRquest
            {
                p_user_name = Application.Current.Properties["OracleUserName"] as string,
                p_person_id = DataInfo.UserInformation.hf_user_auth[0].person_id.ToString(),
                p_mobile_number = MobileNumber,
                p_permanent_residence_address = PermanentResidenceAddress,
                p_post_box_number = PostBoxNumber,
                p_justification = Justification,
                p_submission_date = SubmissionDate.Value.ToString(format: "dd-MMM-yyyy")
            };

            try
            {
                var result = await userDetails.PutCarLoanRequest(request);
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

        async void GetCarLoanDropDown()
        {
            string showAlert = null;

            if (IsListBusy)
                return;

            IsListBusy = true;

            try
            {
                var result = await userDetails.GetCarLoanDropDown();
                if (result)
                {
                    CarLoanDropDownInformation = DataInfo.CarLoanDropDownInformation;
                    if (CarLoanDropDownInformation != null)
                    {
                        showAlert = "Success";
                        ((CarLoanRequest)_page).ChangeText(CarLoanDropDownInformation.p_phone_number, CarLoanDropDownInformation.p_addresses);
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
