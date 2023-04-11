using System;
using System.Threading.Tasks;
using HassadFood.WebService;

namespace HassadFood.Interface
{
    public interface IUserDetails
    {
        Task<bool> EMailVerification(string email);
        Task<bool> PINVerification(string pin);
        Task<bool> ResendVerficationPIN();
        Task<bool> Authenticate(string username, string password);
        Task<bool> UserLogout();
        Task<bool> GetWorkList();
        Task<bool> GetNotificationDetailHistory(string itemkey, int selected_person_id);
        Task<bool> GetLeaveSummary();
        Task<bool> GetAbsenceType();
        Task<bool> CalculateAbsenceDuration(string leaveType, DateTime StartDate, DateTime EndDate);
        Task<bool> PutLeaveRequest(LeaveRequest leave);
        Task<bool> PutDutyVisitRequest(DutyVisitRequest request);
        Task<bool> PutExitPermitRequest(ExitPermitRequest request);
        Task<bool> GetDutyVisitPerDiemClass();
        Task<bool> PutAttachmentsRequest(byte[] file, string filename, string transid);
        Task<bool> GetAttachmentsInformation(string transaction_id);
        Task<byte[]> DownloadAttachmentsFile(string transaction_id, string file_id);
        Task<bool> GetLeaveBalance();
        Task<bool> GetLeaveAppliedDates();
        Task<bool> PutLeaveAmendmentRequest(LeaveAmendmentRequest request);
        Task<bool> PutLeaveCancelRequest(LeaveCancelRequest request);
        Task<bool> PutLeaveResumeRequest(LeaveResumeRequest request);
        Task<bool> PutActionRequest(ActionRequest request);
        Task<bool> GetActionHistory(string NotificationID);
        Task<bool> GetReassignUser();
        Task<bool> GetLeaveSitSummary();

        //Phase 2
        Task<bool> GetCarLoanDropDown();
        Task<bool> GetEducationAllowanceDropDown();
        Task<bool> GetEducationAllowanceDDOB(string personid);
        Task<bool> GetApplicationAccessDropDown();
        Task<bool> GetApplicationAccessRName(string applicationid);
        Task<bool> GetEmployeeLetterDropDown();
        Task<bool> GetReimbursementDropDown();
        Task<bool> GetReimbursementAmount(string reimbursement_type);
        Task<bool> PutEducationAllowanceRequest(EducationAllowanceServiceRequest request);
        Task<bool> PutCarLoanRequest(CarLoanServiceRquest request);
        Task<bool> PutSalaryAdvanceRequest(SalaryAdvanceServiceRequest request);
        Task<bool> PutApplicationAccessRequest(ApplicationAccessServiceRequest request);
        Task<bool> PutEmployeeLetterRequest(EmployeeLetterServiceRequest request);
        Task<bool> PutReimbursementRequest(ReimbursementServiceRequest request);

        Task<bool> GetApproverUserList();
        Task<bool> GetFutureLeaveSitSummary(string username, int personid);

        Task<bool> GetApproverUserListLeaveBalance();

        Task<bool> AddDevice(PushNotificationRequest request);
        Task<bool> DeleteDevice();

        Task<bool> PutAirTicketRequest(AirTicketServiceRequest request);


        Task<bool> GetPONotificationDetails(string item_key);

        Task<bool> GetRequisitionNotificationDetails(string item_key);

        Task<bool> GetTransactionWorkFlow(string item_key);
    }
}
