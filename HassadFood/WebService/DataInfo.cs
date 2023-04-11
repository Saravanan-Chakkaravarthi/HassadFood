using System;
namespace HassadFood.WebService
{
    public static class DataInfo
    {
        public static EmailPINVerify EmailPINVerifyInformation { get; set; }
        public static LoginResponse UserInformation { get; set; }
        public static WorkListResponse WorkListInformation { get; set; }
        public static NotificationDetailResponse NotificationDetailInformation { get; set; }
        public static LeaveSummaryResponse LeaveSummaryInformation { get; set; }
        public static GetAbsenceResponse GetAbsenceTypeInformation { get; set; }
        public static CalculateAbsenceDurationResponse CalculateAbsenceDurationInformation { get; set; }
        public static RequestResponse RequestInformation { get; set; }
        public static RequestResponse AttachmentsRequestInformation { get; set; }
        public static DutyVisitPerDiemClassResponse DutyVisitPerDiemClassInformation { get; set; }
        public static GetAttachmentResponse GetAttachmentInformation { get; set; }
        public static GetLeaveBalanceResponse GetLeaveBalanceInformation { get; set; }
        public static LeaveAppliedDatesResponse GetLeaveAppliedDatesInformation { get; set; }
        public static ActionHistoryResponse ActionHistoryInformation { get; set; }
        public static ReassignUserResponse ReassignUserInformation { get; set; }
        public static GetLeaveSitSummaryResponse GetLeaveSitSummaryInformation { get; set; }

        //Phase 2
        public static CarLoanDropDownResponse CarLoanDropDownInformation { get; set; }
        public static EducationAllowanceDropDownResponse EducationAllowanceDropDownInformation { get; set; }
        public static EducationAllowanceAccademicYear EducationAllowanceAccademicYearInformation { get; set; }
        public static EducationAllowancePayableTo EducationAllowancePayableToInformation { get; set; }
        public static EducationAllowanceDDOBResponse EducationAllowanceDDOBInformation { get; set; }
        public static ApplicationAccessDropDownResponse ApplicationAccessDropDownInformation { get; set; }
        public static ApplicationAccessRNameResponse ApplicationAccessRNameInformation { get; set; }
        public static EmployeeLetterDropDownResponse EmployeeLetterDropDownInformation { get; set; }
        public static ReimbursementDropDownResponse ReimbursementDropDownInformation { get; set; }
        public static ReimbursementAmountResponse ReimbursementAmountInformation { get; set; }

        public static GetApproverUserListResponse GetApproverUserListInformation { get; set; }
        public static GetApproverUserListResponse GetApproverUserListLeaveBalanceInformation { get; set; }
        public static GetFutureLeaveSitSummaryResponse GetFutureLeaveSitSummaryInformation { get; set; }

        public static LogoutResponse LogoutInformation { get; set; }
        public static AddDeviceResponse AddDeviceInformation { get; set; }
        public static DeleteDeviceResponse DeleteDeviceInformation { get; set; }


        //Approvals
        public static PONotificationDetailResponse PONotificationDetailResponseInformation { get; set; }
        public static RequisitionNotificationDetailResponse RequisitionNotificationDetailResponseInformation { get; set; }

        public static HassadTransactionWorkFLow HassadTransactionWorkFLowResponseInformation { get; set; }
        public static void Clear()
        {
            EmailPINVerifyInformation = null;
            UserInformation = null;
            WorkListInformation = null;
            NotificationDetailInformation = null;
            LeaveSummaryInformation = null;
            GetAbsenceTypeInformation = null;
            CalculateAbsenceDurationInformation = null;
            RequestInformation = null;
            AttachmentsRequestInformation = null;
            DutyVisitPerDiemClassInformation = null;
            GetAttachmentInformation = null;
            GetLeaveBalanceInformation = null;
            GetLeaveAppliedDatesInformation = null;
            ActionHistoryInformation = null;
            ReassignUserInformation = null;
            GetLeaveSitSummaryInformation = null;

            //Phase 2
            CarLoanDropDownInformation = null;
            EducationAllowanceDropDownInformation = null;
            EducationAllowanceAccademicYearInformation = null;
            EducationAllowancePayableToInformation = null;
            EducationAllowanceDDOBInformation = null;
            ApplicationAccessDropDownInformation = null;
            ApplicationAccessRNameInformation = null;
            EmployeeLetterDropDownInformation = null;
            ReimbursementDropDownInformation = null;
            ReimbursementAmountInformation = null;

            GetApproverUserListInformation = null;
            GetFutureLeaveSitSummaryInformation = null;
            GetApproverUserListLeaveBalanceInformation = null;

            LogoutInformation = null;
            AddDeviceInformation = null;
            DeleteDeviceInformation = null;

            //Approvals
            PONotificationDetailResponseInformation = null;
            RequisitionNotificationDetailResponseInformation = null;

            HassadTransactionWorkFLowResponseInformation = null;
        }
    }
}
