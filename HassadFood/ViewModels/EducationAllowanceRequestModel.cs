using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using HassadFood.Interface;
using HassadFood.WebService;
using Xamarin.Forms;

namespace HassadFood.ViewModels
{
    public class EducationAllowanceRequestModel : ViewModelBase
    {
        Page _page;
        IUserDetails userDetails;
        List<string> academicYear;
        public EducationAllowanceRequestModel(Page page) : base(page)
        {
            Title = "Education Allowance Request Page";
            userDetails = DependencyService.Get<IUserDetails>();
            _page = page;
            SubmissionDate = DateTime.Now;
            GetEducationAllowanceDropDown();
            academicYear = new List<string>() { "2016 - 2017", "2017 - 2018", "2018 - 2019" };
        }

        PDependentList selectedItemDependentName;

        public PDependentList SelectedItemDependentName
        {
            get { return selectedItemDependentName; }
            set 
            { 
                SetProperty(ref selectedItemDependentName, value);
                if(selectedItemDependentName != null)
                    GetEducationAllowanceDDOB();
            }
        }

        string dOB;

        public string DOB
        {
            get { return dOB; }
            set { SetProperty(ref dOB, value); }
        }

        string school = string.Empty;

        public string School
        {
            get
            {
                return school;
            }
            set
            {
                if (school != value)
                {
                    school = value;
                    OnPropertyChanged("School");
                }
            }
        }

        PHlMeaning selectedItemTerm;

        public PHlMeaning SelectedItemTerm
        {
            get { return selectedItemTerm; }
            set { SetProperty(ref selectedItemTerm, value); }
        }

        string amount;

        public string Amount
        {
            get
            {
                return amount;
            }
            set
            {
                if (amount != value)
                {
                    amount = value;
                    OnPropertyChanged("Amount");
                }
            }
        }

        public List<string> AcademicYear
        {
            get { return academicYear; }
            set { SetProperty(ref academicYear, value); }
        }

        PAccademicYearList selectedItemAcademicYear;

        public PAccademicYearList SelectedItemAcademicYear
        {
            get { return selectedItemAcademicYear; }
            set { SetProperty(ref selectedItemAcademicYear, value); }
        }

        PayableTo selectedItemPayableTo;

        public PayableTo SelectedItemPayableTo
        {
            get { return selectedItemPayableTo; }
            set { SetProperty(ref selectedItemPayableTo, value); }
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
            if (SelectedItemDependentName == null)
            {
                await page.DisplayAlert("Alert", "Dependent name cannot be empty.", "OK");
                return;
            }

            if (School == null && School == "")
            {
                await page.DisplayAlert("Alert", "School cannot be empty.", "OK");
                return;
            }

            if (SelectedItemTerm == null)
            {
                await page.DisplayAlert("Alert", "Term cannot be empty.", "OK");
                return;
            }

            if (Amount == null && Amount == "")
            {
                await page.DisplayAlert("Alert", "Amount cannot be empty.", "OK");
                return;
            }

            if (SelectedItemAcademicYear == null)
            {
                await page.DisplayAlert("Alert", "Academic year cannot be empty.", "OK");
                return;
            }

            if (SelectedItemPayableTo == null)
            {
                await page.DisplayAlert("Alert", "Payable to cannot be empty.", "OK");
                return;
            }

            if (AttachmentsFiles == null || AttachmentsFiles.Count == 0)
            {
                await page.DisplayAlert("Alert", "Attachments is required.", "OK");
                return;
            }

            string showAlert = null;

            if (IsBusy)
                return;

            IsBusy = true;

            EducationAllowanceServiceRequest request = new EducationAllowanceServiceRequest
            {
                p_user_name = Application.Current.Properties["OracleUserName"] as string,
                p_contact_person_id = SelectedItemDependentName.contact_person_id,
                p_dob = DOB,
                p_school = School,
                p_term = SelectedItemTerm.terms_type,
                p_academic_year = SelectedItemAcademicYear.year,
                p_amount = Int32.Parse(Amount),
                p_payable_to = SelectedItemPayableTo.type,
                p_submission_date = SubmissionDate.Value.ToString(format: "dd-MMM-yyyy")
            };

            try
            {
                var result = await userDetails.PutEducationAllowanceRequest(request);
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

        async void GetEducationAllowanceDropDown()
        {
            string showAlert = null;

            if (IsListBusy)
                return;

            IsListBusy = true;

            try
            {
                var result = await userDetails.GetEducationAllowanceDropDown();
                if (result)
                {
                    EducationAllowanceDropDownInformation = DataInfo.EducationAllowanceDropDownInformation;
                    EducationAllowanceAccademicYearInformation = DataInfo.EducationAllowanceAccademicYearInformation;
                    EducationAllowancePayableToInformation = DataInfo.EducationAllowancePayableToInformation;
                    if (EducationAllowanceDropDownInformation != null && EducationAllowanceDropDownInformation.p_dependent_list.Count > 0)
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

        async void GetEducationAllowanceDDOB()
        {
            string showAlert = null;

            if (IsListBusy)
                return;

            IsListBusy = true;

            try
            {
                var result = await userDetails.GetEducationAllowanceDDOB(selectedItemDependentName.contact_person_id.ToString());
                if (result)
                {
                    EducationAllowanceDDOBInformation = DataInfo.EducationAllowanceDDOBInformation;
                    if (EducationAllowanceDDOBInformation != null)
                    {
                        showAlert = "Success";
                        DOB = EducationAllowanceDDOBInformation.p_dob;
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
