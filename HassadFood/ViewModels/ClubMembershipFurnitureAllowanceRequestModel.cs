using System;
using System.ComponentModel;
using System.Diagnostics;
using HassadFood.Interface;
using HassadFood.WebService;
using Xamarin.Forms;

namespace HassadFood.ViewModels
{
    public class ClubMembershipFurnitureAllowanceRequestModel : ViewModelBase
    {
        Page _page;
        IUserDetails userDetails;
        public ClubMembershipFurnitureAllowanceRequestModel(Page page) : base(page)
        {
            Title = "Reimbursement Request Page";
            userDetails = DependencyService.Get<IUserDetails>();
            _page = page;
            SubmissionDate = DateTime.Now;
            MembershipStartDate = DateTime.Now;
            MembershipEndDate = DateTime.Now;
            GetReimbursementDropDown();
        }

        bool isClubMembership;

        public bool IsClubMembership
        {
            get { return isClubMembership; }
            set { SetProperty(ref isClubMembership, value); }
        }

        bool isFurnitureAllowance;

        public bool IsFurnitureAllowance
        {
            get { return isFurnitureAllowance; }
            set { SetProperty(ref isFurnitureAllowance, value); }
        }


        PReimbursementtypeList selectedItemReimbursementType;

        public PReimbursementtypeList SelectedItemReimbursementType
        {
            get { return selectedItemReimbursementType; }
            set 
            { 
                SetProperty(ref selectedItemReimbursementType, value);
                if (selectedItemReimbursementType != null)
                {
                    switch(selectedItemReimbursementType.reimbursement_type)
                    {
                        case "Club Membership":
                            IsClubMembership = true;
                            IsFurnitureAllowance = false;
                            break;
                        case "Furniture Allowance":
                            IsClubMembership = false;
                            IsFurnitureAllowance = true;
                            break;
                    }
                    GetReimbursementAmount(selectedItemReimbursementType.reimbursement_type);
                }
            }
        }

        string maritalStatus;

        public string MaritalStatus
        {
            get { return maritalStatus; }
            set { SetProperty(ref maritalStatus, value); }
        }

        string allowanceAmount;

        public string AllowanceAmount
        {
            get { return allowanceAmount; }
            set { SetProperty(ref allowanceAmount, value); }
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

        string amountPayable = string.Empty;

        public string AmountPayable
        {
            get
            {
                return amountPayable;
            }
            set
            {
                if (amountPayable != value)
                {
                    amountPayable = value;
                    OnPropertyChanged("AmountPayable");
                }
            }
        }

        string billAmount = string.Empty;

        public string BillAmount
        {
            get
            {
                return billAmount;
            }
            set
            {
                if (billAmount != value)
                {
                    billAmount = value;
                    OnPropertyChanged("BillAmount");
                }
            }
        }

        string clubName = string.Empty;

        public string ClubName
        {
            get
            {
                return clubName;
            }
            set
            {
                if (clubName != value)
                {
                    clubName = value;
                    OnPropertyChanged("ClubName");
                }
            }
        }

        DateTime? membershipStartDate = null;

        public DateTime? MembershipStartDate
        {
            get { return membershipStartDate; }
            set { SetProperty(ref membershipStartDate, value); }
        }

        DateTime? membershipEndDate = null;

        public DateTime? MembershipEndDate
        {
            get { return membershipEndDate; }
            set { SetProperty(ref membershipEndDate, value); }
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
            if (SelectedItemReimbursementType == null)
            {
                await page.DisplayAlert("Alert", "Reimbursement type cannot be empty.", "OK");
                return;
            }

            if(selectedItemReimbursementType.reimbursement_type == "Club Membership")
            {
                if (AttachmentsFiles == null || AttachmentsFiles.Count == 0)
                {
                    await page.DisplayAlert("Alert", "Attachments is required.", "OK");
                    return;
                }

                if (ClubName == null && ClubName == "")
                {
                    await page.DisplayAlert("Alert", "Club name cannot be empty.", "OK");
                    return;
                }

                if (BillAmount == null && BillAmount == "")
                {
                    await page.DisplayAlert("Alert", "Bill amount cannot be empty.", "OK");
                    return;
                }

                if (AmountPayable == null && AmountPayable == "")
                {
                    await page.DisplayAlert("Alert", "Amount payable cannot be empty.", "OK");
                    return;
                }
            }

            string showAlert = null;

            if (IsBusy)
                return;

            IsBusy = true;

            ReimbursementServiceRequest request = new ReimbursementServiceRequest
            {
                p_user_name = Application.Current.Properties["OracleUserName"] as string,
                p_reimbursement_type = SelectedItemReimbursementType.reimbursement_type,
                p_marital_status = MaritalStatus,
                p_club_name = ClubName,
                p_membership_start_date = MembershipStartDate.Value.ToString(format: "dd-MMM-yyyy"),
                p_membership_end_date = MembershipEndDate.Value.ToString(format: "dd-MMM-yyyy"),
                p_bill_amount = BillAmount,
                p_amount_payable = AmountPayable,
                p_justification = Justification,
                p_allowance_amount = AllowanceAmount,
                p_submission_date = SubmissionDate.Value.ToString(format: "dd-MMM-yyyy")
            };

            try
            {
                var result = await userDetails.PutReimbursementRequest(request);
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

        async void GetReimbursementDropDown()
        {
            string showAlert = null;

            if (IsListBusy)
                return;

            IsListBusy = true;

            try
            {
                var result = await userDetails.GetReimbursementDropDown();
                if (result)
                {
                    ReimbursementDropDownInformation = DataInfo.ReimbursementDropDownInformation;
                    if (ReimbursementDropDownInformation != null && ReimbursementDropDownInformation.p_reimbursementtype_list.Count > 0)
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

        async void GetReimbursementAmount(string reimbursement_type)
        {
            string showAlert = null;

            if (IsListBusy)
                return;

            IsListBusy = true;

            try
            {
                var result = await userDetails.GetReimbursementAmount(reimbursement_type);
                if (result)
                {
                    ReimbursementAmountInformation = DataInfo.ReimbursementAmountInformation;
                    if (ReimbursementAmountInformation != null && ReimbursementAmountInformation.p_elligibleamount.Count > 0)
                    {
                        showAlert = "Success";
                        AllowanceAmount = ReimbursementAmountInformation.p_elligibleamount[0].val;
                        MaritalStatus = ReimbursementAmountInformation.p_maritalstatus[0].marital_status;
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
