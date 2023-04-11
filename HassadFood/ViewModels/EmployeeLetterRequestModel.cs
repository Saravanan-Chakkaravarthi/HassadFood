using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using HassadFood.Interface;
using HassadFood.WebService;
using Xamarin.Forms;

namespace HassadFood.ViewModels
{
    public class EmployeeLetterRequestModel : ViewModelBase
    {
        Page _page;
        IUserDetails userDetails;
        public EmployeeLetterRequestModel(Page page) : base(page)
        {
            Title = "Employee Letter Request Page";
            userDetails = DependencyService.Get<IUserDetails>();
            _page = page;
            SubmissionDate = DateTime.Now;
            languageList = new List<string>() { "English", "Arabic" };
            SelectedItemLanguage = "English";
            GetEmployeeLetterDropDown();
        }

        List<string> languageList;

        public List<string> LanguageList
        {
            get { return languageList; }
            set { SetProperty(ref languageList, value); }
        }

        string selectedItemLanguage;

        public string SelectedItemLanguage
        {
            get { return selectedItemLanguage; }
            set { SetProperty(ref selectedItemLanguage, value); }
        }

        bool isLetterVisa;

        public bool IsLetterVisa
        {
            get { return isLetterVisa; }
            set { SetProperty(ref isLetterVisa, value); }
        }

        bool isLetterEmbassy;

        public bool IsLetterEmbassy
        {
            get { return isLetterEmbassy; }
            set { SetProperty(ref isLetterEmbassy, value); }
        }

        PLettertypeList selectedItemLetterType;

        public PLettertypeList SelectedItemLetterType
        {
            get { return selectedItemLetterType; }
            set 
            { 
                SetProperty(ref selectedItemLetterType, value);
                if (selectedItemLetterType != null)
                {
                    switch (selectedItemLetterType.letter_type)
                    {
                        case "Visa Letter":
                            IsLetterVisa = true;
                            IsLetterEmbassy = false;
                            travelDateFrom = null;
                            travelDateTo = null;
                            break;
                        case "Embassy Letter":
                            IsLetterVisa = false;
                            IsLetterEmbassy = true;
                            break;
                        default:
                            IsLetterVisa = false;
                            IsLetterEmbassy = false;
                            travelDateFrom = null;
                            travelDateTo = null;
                            break;
                    }
                }
            }
        }

        PPurposelettertypeList selectedItemPurposeOfLetter;

        public PPurposelettertypeList SelectedItemPurposeOfLetter
        {
            get { return selectedItemPurposeOfLetter; }
            set { SetProperty(ref selectedItemPurposeOfLetter, value); }
        }

        PVisatypeList selectedItemVisaType;

        public PVisatypeList SelectedItemVisaType
        {
            get { return selectedItemVisaType; }
            set { SetProperty(ref selectedItemVisaType, value); }
        }

        PTraveltypeList selectedItemTravelDestination;

        public PTraveltypeList SelectedItemTravelDestination
        {
            get { return selectedItemTravelDestination; }
            set { SetProperty(ref selectedItemTravelDestination, value); }
        }

        PLetterpurposetypeList selectedItemLetterPurpose;

        public PLetterpurposetypeList SelectedItemLetterPurpose
        {
            get { return selectedItemLetterPurpose; }
            set { SetProperty(ref selectedItemLetterPurpose, value); }
        }

        DateTime? travelDateFrom = null;

        public DateTime? TravelDateFrom
        {
            get { return travelDateFrom; }
            set { SetProperty(ref travelDateFrom, value); }
        }

        DateTime? travelDateTo = null;

        public DateTime? TravelDateTo
        {
            get { return travelDateTo; }
            set { SetProperty(ref travelDateTo, value); }
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

            if (SelectedItemLetterType == null)
            {
                await page.DisplayAlert("Alert", "Letter type cannot be empty.", "OK");
                return;
            }

            if (SelectedItemLanguage == null && SelectedItemLanguage == "")
            {
                await page.DisplayAlert("Alert", "Language type cannot be empty.", "OK");
                return;
            }

            string showAlert = null;

            if (IsBusy)
                return;

            IsBusy = true;

            EmployeeLetterServiceRequest request = new EmployeeLetterServiceRequest
            {
                p_user_name = Application.Current.Properties["OracleUserName"] as string,
                p_letter_type = SelectedItemLetterType.letter_type,
                p_purpose_of_letter = (SelectedItemPurposeOfLetter != null) ? SelectedItemPurposeOfLetter.purpose_letter : "",
                p_visa_type = (SelectedItemVisaType != null) ? SelectedItemVisaType.visa_type : "",
                p_travel_destination = (SelectedItemTravelDestination != null) ? SelectedItemTravelDestination.travel_destination : "",
                p_travel_date_from = (TravelDateFrom != null) ? TravelDateFrom.Value.ToString(format: "dd-MMM-yyyy") : "",
                p_justification = Justification,
                p_travel_date_to = (TravelDateTo != null) ? TravelDateTo.Value.ToString(format: "dd-MMM-yyyy") : "",
                p_letter_purpose = (SelectedItemLetterPurpose != null) ? SelectedItemLetterPurpose.letter_purpose : "",
                p_language_of_letter = SelectedItemLanguage,
                p_submission_date = SubmissionDate.Value.ToString(format: "dd-MMM-yyyy")
            };

            try
            {
                var result = await userDetails.PutEmployeeLetterRequest(request);
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

        public async void GetEmployeeLetterDropDown()
        {
            string showAlert = null;

            if (IsListBusy)
                return;

            IsListBusy = true;

            try
            {
                var result = await userDetails.GetEmployeeLetterDropDown();
                if (result)
                {
                    EmployeeLetterDropDownInformation = DataInfo.EmployeeLetterDropDownInformation;
                    if (EmployeeLetterDropDownInformation != null && EmployeeLetterDropDownInformation.p_lettertype_list.Count > 0)
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
