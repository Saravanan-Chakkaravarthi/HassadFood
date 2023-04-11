using System;
using HassadFood.WebService;

namespace HassadFood.MVVMHelper
{
	public class BaseViewModel : ExtendedBindableObject
	{
		string title = string.Empty;

		/// <summary>
		/// Gets or sets the title.
		/// </summary>
		/// <value>The title.</value>
		public string Title
		{
			get { return title; }
			set { SetProperty(ref title, value); }
		}

        string subtitle = string.Empty;

        /// <summary>
        /// Gets or sets the subtitle.
        /// </summary>
        /// <value>The subtitle.</value>
        public string Subtitle
        {
            get { return subtitle; }
            set { SetProperty(ref subtitle, value); }
        }

		string engLabelColor = string.Empty;

		/// <summary>
        /// Gets or sets the EngLabelColor.
		/// </summary>
        /// <value>The EngLabelColor.</value>
        public string EngLabelColor
		{
            get { return engLabelColor; }
            set { SetProperty(ref engLabelColor, value); }
		}

		string araLabelColor = string.Empty;

		/// <summary>
        /// Gets or sets the araLabelColor.
		/// </summary>
        /// <value>The araLabelColor.</value>
        public string AraLabelColor
		{
            get { return araLabelColor; }
            set { SetProperty(ref araLabelColor, value); }
		}		
        		
        string loginButton = string.Empty;

        /// <summary>
        /// Gets or sets the loginButton.
        /// </summary>
        /// <value>The loginButton.</value>
        public string LoginButton
        {
            get { return loginButton; }
            set { SetProperty(ref loginButton, value); }
        }

		bool isBusy;

		/// <summary>
		/// Gets or sets a value indicating whether this instance is busy.
		/// </summary>
		/// <value><c>true</c> if this instance is busy; otherwise, <c>false</c>.</value>
		public bool IsBusy
		{
			get { return isBusy; }
			set
			{
                if (SetProperty(ref isBusy, value))
                {
                    IsNotBusy = !isBusy;
                    IsNotRejectBusy = !isBusy;
                }
			}
		}



        //Alert
        string alertHead = string.Empty;

        public string AlertHead
        {
            get { return alertHead; }
            set { SetProperty(ref alertHead, value); }
        }

        //Alert box message
        string alertMessage = string.Empty;

        public string AlertMessage
        {
            get { return alertMessage; }
            set { SetProperty(ref alertMessage, value); }
        }

        //Alert ok button visibility

        bool isOkStackVisible = false;
        public bool IsOkStackVisible
        {
            get { return isOkStackVisible; }
            set { SetProperty(ref isOkStackVisible, value); }
        }

        //Alert submit button visibility

        bool isSubmitStackVisible = false;
        public bool IsSubmitStackVisible
        {
            get { return isSubmitStackVisible; }
            set { SetProperty(ref isSubmitStackVisible, value); }
        }



        bool isNotBusy = true;

		/// <summary>
		/// Gets or sets a value indicating whether this instance is not busy.
		/// </summary>
		/// <value><c>true</c> if this instance is not busy; otherwise, <c>false</c>.</value>
		public bool IsNotBusy
		{
			get { return isNotBusy; }
			private set { SetProperty(ref isNotBusy, value); }
		}

		bool isListBusy;

		/// <summary>
		/// Gets or sets a value indicating whether this instance is busy.
		/// </summary>
		/// <value><c>true</c> if this instance is busy; otherwise, <c>false</c>.</value>
		public bool IsListBusy
		{
			get { return isListBusy; }
			set
			{
				if (SetProperty(ref isListBusy, value))
					IsNotListBusy = !isListBusy;
			}
		}

		bool isNotListBusy = true;

		/// <summary>
		/// Gets or sets a value indicating whether this instance is not busy.
		/// </summary>
		/// <value><c>true</c> if this instance is not busy; otherwise, <c>false</c>.</value>
		public bool IsNotListBusy
		{
			get { return isNotListBusy; }
			private set { SetProperty(ref isNotListBusy, value); }
		}

        bool isRejectBusy;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is busy.
        /// </summary>
        /// <value><c>true</c> if this instance is busy; otherwise, <c>false</c>.</value>
        public bool IsRejectBusy
        {
            get { return isRejectBusy; }
            set
            {
                if (SetProperty(ref isRejectBusy, value))
                {
                    IsNotBusy = !isRejectBusy;
                    IsNotRejectBusy = !isRejectBusy;
                }
            }
        }

        bool isNotRejectBusy = true;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is not busy.
        /// </summary>
        /// <value><c>true</c> if this instance is not busy; otherwise, <c>false</c>.</value>
        public bool IsNotRejectBusy
        {
            get { return isNotRejectBusy; }
            private set { SetProperty(ref isNotRejectBusy, value); }
        }

		LoginResponse userInformation;

        public LoginResponse UserInformation
        {
            get { return userInformation; }
            set { SetProperty(ref userInformation, value); }
        }

        WorkListResponse workListInformation;

        public WorkListResponse WorkListInformation
        {
            get { return workListInformation; }
            set { SetProperty(ref workListInformation, value); }
        }

        GetWorkList selectedItemWorkList;

        public GetWorkList SelectedItemWorkList
        {
            get { return selectedItemWorkList; }
            set { SetProperty(ref selectedItemWorkList, value); }
        }

        LeaveSummaryResponse leaveSummaryInformation;

        public LeaveSummaryResponse LeaveSummaryInformation
        {
            get { return leaveSummaryInformation; }
            set { SetProperty(ref leaveSummaryInformation, value); }
        }

        GetLeaveSummary selectedItemLeaveSummary;

        public GetLeaveSummary SelectedItemLeaveSummary
        {
            get { return selectedItemLeaveSummary; }
            set { SetProperty(ref selectedItemLeaveSummary, value); }
        }

        GetAbsenceResponse getAbsenceTypeInformation;

        public GetAbsenceResponse GetAbsenceTypeInformation
        {
            get { return getAbsenceTypeInformation; }
            set { SetProperty(ref getAbsenceTypeInformation, value); }
        }

        CalculateAbsenceDurationResponse calculateAbsenceDurationInformation;

        public CalculateAbsenceDurationResponse CalculateAbsenceDurationInformation
        {
            get { return calculateAbsenceDurationInformation; }
            set { SetProperty(ref calculateAbsenceDurationInformation, value); }
        }

        DutyVisitPerDiemClassResponse dutyVisitPerDiemClassInformation;

        public DutyVisitPerDiemClassResponse DutyVisitPerDiemClassInformation
        {
            get { return dutyVisitPerDiemClassInformation; }
            set { SetProperty(ref dutyVisitPerDiemClassInformation, value); }
        }

        RequestResponse requestInformation;

        public RequestResponse RequestInformation
        {
            get { return requestInformation; }
            set { SetProperty(ref requestInformation, value); }
        }

        GetAttachmentResponse getAttachmentInformation;

        public GetAttachmentResponse GetAttachmentInformation
        {
            get { return getAttachmentInformation; }
            set { SetProperty(ref getAttachmentInformation, value); }
        }

        LeaveAppliedDatesResponse getLeaveAppliedDatesInformation;

        public LeaveAppliedDatesResponse GetLeaveAppliedDatesInformation
        {
            get { return getLeaveAppliedDatesInformation; }
            set { SetProperty(ref getLeaveAppliedDatesInformation, value); }
        }

        ReassignUserResponse reassignUserInformation;

        public ReassignUserResponse ReassignUserInformation
        {
            get { return reassignUserInformation; }
            set { SetProperty(ref reassignUserInformation, value); }
        }

        GetLeaveSitSummaryResponse getLeaveSitSummaryInformation;

        public GetLeaveSitSummaryResponse GetLeaveSitSummaryInformation
        {
            get { return getLeaveSitSummaryInformation; }
            set { SetProperty(ref getLeaveSitSummaryInformation, value); }
        }

        CarLoanDropDownResponse carLoanDropDownInformation;

        public CarLoanDropDownResponse CarLoanDropDownInformation
        {
            get { return carLoanDropDownInformation; }
            set { SetProperty(ref carLoanDropDownInformation, value); }
        }

        EducationAllowanceDropDownResponse educationAllowanceDropDownInformation;

        public EducationAllowanceDropDownResponse EducationAllowanceDropDownInformation
        {
            get { return educationAllowanceDropDownInformation; }
            set { SetProperty(ref educationAllowanceDropDownInformation, value); }
        }

        EducationAllowanceAccademicYear educationAllowanceAccademicYearInformation;

        public EducationAllowanceAccademicYear EducationAllowanceAccademicYearInformation
        {
            get { return educationAllowanceAccademicYearInformation; }
            set { SetProperty(ref educationAllowanceAccademicYearInformation, value); }
        }

        EducationAllowancePayableTo educationAllowancePayableToInformation;

        public EducationAllowancePayableTo EducationAllowancePayableToInformation
        {
            get { return educationAllowancePayableToInformation; }
            set { SetProperty(ref educationAllowancePayableToInformation, value); }
        }

        EducationAllowanceDDOBResponse educationAllowanceDDOBInformation;

        public EducationAllowanceDDOBResponse EducationAllowanceDDOBInformation
        {
            get { return educationAllowanceDDOBInformation; }
            set { SetProperty(ref educationAllowanceDDOBInformation, value); }
        }

        ApplicationAccessDropDownResponse applicationAccessDropDownInformation;

        public ApplicationAccessDropDownResponse ApplicationAccessDropDownInformation
        {
            get { return applicationAccessDropDownInformation; }
            set { SetProperty(ref applicationAccessDropDownInformation, value); }
        }

        ApplicationAccessRNameResponse applicationAccessRNameInformation;

        public ApplicationAccessRNameResponse ApplicationAccessRNameInformation
        {
            get { return applicationAccessRNameInformation; }
            set { SetProperty(ref applicationAccessRNameInformation, value); }
        }

        ReimbursementDropDownResponse reimbursementDropDownInformation;

        public ReimbursementDropDownResponse ReimbursementDropDownInformation
        {
            get { return reimbursementDropDownInformation; }
            set { SetProperty(ref reimbursementDropDownInformation, value); }
        }

        ReimbursementAmountResponse reimbursementAmountInformation;

        public ReimbursementAmountResponse ReimbursementAmountInformation
        {
            get { return reimbursementAmountInformation; }
            set { SetProperty(ref reimbursementAmountInformation, value); }
        }

        EmployeeLetterDropDownResponse employeeLetterDropDownInformation;

        public EmployeeLetterDropDownResponse EmployeeLetterDropDownInformation
        {
            get { return employeeLetterDropDownInformation; }
            set { SetProperty(ref employeeLetterDropDownInformation, value); }
        }

        GetFutureLeaveSitSummaryResponse getFutureLeaveSitSummaryInformation;

        public GetFutureLeaveSitSummaryResponse GetFutureLeaveSitSummaryInformation
        {
            get { return getFutureLeaveSitSummaryInformation; }
            set { SetProperty(ref getFutureLeaveSitSummaryInformation, value); }
        }

        GetApproverUserListResponse getApproverUserListLeaveBalanceInformation;

        public GetApproverUserListResponse GetApproverUserListLeaveBalanceInformation
        {
            get { return getApproverUserListLeaveBalanceInformation; }
            set { SetProperty(ref getApproverUserListLeaveBalanceInformation, value); }
        }

        //Approvals

        GetWorkList selectedNotification;

        public GetWorkList SelectedNotification
        {
            get { return selectedNotification; }
            set { SetProperty(ref selectedNotification, value); }
        }

        PONotificationDetailResponse pONotificationDetailResponseInformation;
        public PONotificationDetailResponse PONotificationDetailResponseInformation
        {
            get { return pONotificationDetailResponseInformation; }
            set { SetProperty(ref pONotificationDetailResponseInformation, value); }
        }


        RequisitionNotificationDetailResponse requisitionNotificationDetailResponseInformation;
        public RequisitionNotificationDetailResponse RequisitionNotificationDetailResponseInformation
        {
            get { return requisitionNotificationDetailResponseInformation; }
            set { SetProperty(ref requisitionNotificationDetailResponseInformation, value); }
        }


        HassadTransactionWorkFLow hassadTransactionWorkFLowResponseInformation;
        public HassadTransactionWorkFLow HassadTransactionWorkFLowResponseInformation
        {
            get { return hassadTransactionWorkFLowResponseInformation; }
            set { SetProperty(ref hassadTransactionWorkFLowResponseInformation, value); }
        }






    }
}
