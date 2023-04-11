using System;
using System.Collections.Generic;

namespace HassadFood.WebService
{
    public class Utils
    {
    }

    public class EmailPINVerify
    {
        public string p_record { get; set; }
        public string p_success_flag { get; set; }
        public int p_trx_id { get; set; }
    }

    public class HfUserAuth
    {
        public int user_id { get; set; }
        public int person_id { get; set; }
        public int business_group_id { get; set; }
        public string resp_flag { get; set; }
        public string disp_name { get; set; }
        public string sub_inventory { get; set; }
        public string location { get; set; }
        public string isverified { get; set; }
        public string sqno { get; set; }
        //logout response
        public string p_success_flag { get; set; }
    }

    public class LogoutResponse
    {
        public string p_success_flag { get; set; }
    }

    public class AddDeviceResponse
    {
        public string p_success_flag { get; set; }
    }

    public class DeleteDeviceResponse
    {
        public string p_success_flag { get; set; }
    }

    public class LoginResponse
    {
        public List<HfUserAuth> hf_user_auth { get; set; }
    }

    //public class GetWorklistUser
    //{
    //    public string requestor_user_name { get; set; }
    //    public string requestor_name { get; set; }
    //    public string approver_user_name { get; set; }
    //    public string approver_name { get; set; }
    //    public string full_name_english { get; set; }
    //    public string sit_name { get; set; }
    //    public string image_Name { get; set; }
    //    public string xml_tag { get; set; }
    //    public string subject { get; set; }
    //    public string date_of_submission { get; set; }
    //    public int transaction_id { get; set; }
    //    public string item_type { get; set; }
    //    public string item_key { get; set; }
    //    public int notification_id { get; set; }
    //    public string action { get; set; }       
    //}

    //public class WorkListResponse
    //{
    //    public List<GetWorklistUser> get_worklist_user { get; set; }
    //}

    public class NotificationDetailResponse
    {
        public string p_notification_det { get; set; }
    }

    public class GetWorkList
    {
        public int notification_id { get; set; }
        public string from_user { get; set; }
        public string to_user { get; set; }
        public string subject { get; set; }
        public string language { get; set; }
        public string image_Name { get; set; }
        public string begin_date { get; set; }
        public string type { get; set; }
        public string from_role { get; set; }
        public string item_key { get; set; }
        public string process_name { get; set; }
        public string action { get; set; }
        public int selected_person_id { get; set; }

        public string item_type { get; set; }
        public string transaction_id { get; set; }
    }

    public class WorkListResponse
    {
        public List<GetWorkList> get_work_list { get; set; }
    }

    public class GetLeaveSummary
    {
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public string absence_type { get; set; }
        public string absence_category { get; set; }
        public string approval_status { get; set; }
        public string absence_status { get; set; }
        public int absence_days { get; set; }
        public string full_name { get; set; }
        public string pk1_value { get; set; }
        public string entity_name { get; set; }
        public object comments { get; set; }
        public string resume_flag { get; set; }
        public int? transaction_id { get; set; }
    }

    public class LeaveSummaryResponse
    {
        public List<GetLeaveSummary> get_leave_summary { get; set; }
    }


    public class LeaveSummaryRequest
    {
        public string p_user_name { get; set; }
        public int p_person_id { get; set; }
        public string p_start_date { get; set; }
        public string p_end_date { get; set; }
        public string p_approval { get; set; }
        public string p_absence_status { get; set; }
        public string p_absence_type { get; set; }
        public string p_absence_category { get; set; }
    }

    public class GetAbsenceTypesP
    {
        public int absence_attendance_type_id { get; set; }
        public string name { get; set; }
    }

    public class GetAbsenceResponse
    {
        public List<GetAbsenceTypesP> get_absence_types_p { get; set; }
        public List<string> NewAbsenceType { get; set; }
    }

    public class CalculateAbsenceDurationResponse
    {
        public int p_days { get; set; }
    }

    public class LeaveRequest
    {
        public string p_user_name { get; set; }
        public string p_absence_type { get; set; }
        public string p_start_date { get; set; }
        public string p_end_date { get; set; }
        public string p_user_comments { get; set; }
        public string p_file_name { get; set; }
        public string p_exit_permit_req { get; set; }
        public string p_leave_advance { get; set; }
        public string p_replace_user { get; set; }
        public string p_air_ticket { get; set; }
        public string p_flight_date { get; set; }
        public string p_flight_number { get; set; }
        public string p_flight_time { get; set; }
    }

    public class Sshr
    {
        public string p_success_flag { get; set; }
        public string p_error_msg { get; set; }
    }

    public class RequestResponse
    {
        public List<Sshr> sshr { get; set; }
        public string p_error_msg { get; set; }
        public string p_record { get; set; }
        public string p_success_flag { get; set; }
        public int p_trx_id { get; set; }
    }

    public class DutyVisitRequest
    {
        public string p_user_name { get; set; }
        public string p_perdiem_amt { get; set; }
        public string p_class_of_travel { get; set; }
        public string p_date_of_travel { get; set; }
        public string p_fday_visit { get; set; }
        public string p_lday_visit { get; set; }
        public string p_arrival_date { get; set; }
        public string p_exit_permit { get; set; }
        public string p_destination { get; set; }
        public string p_budget { get; set; }
        public string p_purpose { get; set; }
        public string p_accomodation { get; set; }
        public string p_ticket { get; set; }
        public string p_perdiem_amt_host { get; set; }
        public string p_submission_date { get; set; }
    }

    public class ExitPermitRequest
    {
        public string p_user_name { get; set; }
        public string p_flight_date { get; set; }
        public string p_return_date { get; set; }
        public string p_airline_flight_no { get; set; }
        public string p_flight_travel_time { get; set; }
        public string p_justification { get; set; }
        public string p_submission_date { get; set; }
    }

    public class DutyVisitPerDiemClassResponse
    {
        public double p_per_diam { get; set; }
        public string p_class { get; set; }
    }

    public class GetAttachment
    {
        public string transaction_id { get; set; }
        public string file_name { get; set; }
        public string file_content_type { get; set; }
        public int file_id { get; set; }
    }

    public class GetAttachmentResponse
    {
        public List<GetAttachment> get_attachment { get; set; }
    }

    public class GetLeaveBalanceResponse
    {
        public string p_entitlement { get; set; }
    }

    public class Item
    {
        public int absence_attendance_id { get; set; }
        public string date_start_end { get; set; }
        public DateTime date_start { get; set; }
        public DateTime date_end { get; set; }
        public string description { get; set; }
    }

    public class LeaveAppliedDatesResponse
    {
        public List<Item> items { get; set; }
        public List<string> NewAppliedLeave { get; set; }
    }

    public class LeaveAmendmentRequest
    {
        public string p_user_name { get; set; }
        public int p_abs_attendance_id { get; set; }
        public string p_new_leave_start_date { get; set; }
        public string p_new_leave_end_date { get; set; }
        public string p_exit_permit { get; set; }
        public string p_new_travel_date { get; set; }
        public string p_justification { get; set; }
        public string p_submission_date { get; set; }
    }

    public class LeaveCancelRequest
    {
        public string p_user_name { get; set; }
        public int p_abs_attendance_id { get; set; }
        public string p_justification { get; set; }
        public string p_add_comments { get; set; }
        public string p_submission_date { get; set; }
    }

    public class LeaveResumeRequest
    {
        public string p_user_name { get; set; }
        public string p_abs_attendance_id { get; set; }
        public string p_justification { get; set; }
        public string p_remarks { get; set; }
        public string p_submission_date { get; set; }
    }

    public class ActionRequest
    {
        public string p_user_name { get; set; }
        public string p_itemtype { get; set; }
        public string p_item_key { get; set; }
        public string p_result { get; set; }
        public string p_notification_id { get; set; }
        public string p_comments { get; set; }
        public string p_fwd_user { get; set; }
    }

    public class GetActionHist
    {
        public int seq_no { get; set; }
        public int notification_id { get; set; }
        public string role { get; set; }
        public string action { get; set; }
        public string comments { get; set; }
        public string action_date { get; set; }
    }

    public class ActionHistoryResponse
    {
        public List<GetActionHist> get_action_hist { get; set; }
    }

    public class GetReassignUser
    {
        public string user_name { get; set; }
        public string display_name { get; set; }
        public string email_address { get; set; }
    }

    public class ReassignUserResponse
    {
        public List<GetReassignUser> get_reassign_users { get; set; }
    }

    public class GetLeaveSitSummary
    {
        public string leave_type { get; set; }
        public string leave_category { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }
        public string approval_status { get; set; }
        public string absence_status { get; set; }
        public int absence_days { get; set; }
        public string full_name { get; set; }
        public string approver_name { get; set; }
        public string date_of_submission { get; set; }       
    }

    public class GetLeaveSitSummaryResponse
    {
        public List<GetLeaveSitSummary> get_leave_sit_summary { get; set; }
    }

    //Phase 2

    public class CarLoanDropDownResponse
    {
        public string p_addresses { get; set; }
        public string p_phone_number { get; set; }
    }

    public class PDependentList
    {
        public string name { get; set; }
        public int contact_person_id { get; set; }
        public string dob { get; set; }
        public int age { get; set; }
    }

    public class PHlMeaning
    {
        public string terms_type { get; set; }
    }

    public class PAccademicYearList
    {
        public string year { get; set; }
    }

    public class PayableTo
    {
        public string type { get; set; }
    }

    public class EducationAllowanceAccademicYear
    {
        public List<PAccademicYearList> p_accademic_year_list { get; set; }
    }

    public class EducationAllowancePayableTo
    {
        public List<PayableTo> p_payable_to { get; set; }
    }

    public class EducationAllowanceDropDownResponse
    {
        public string p_accademic_year_list { get; set; }
        public string p_payable_to { get; set; }
        public List<PDependentList> p_dependent_list { get; set; }
        public List<PHlMeaning> p_hl_meaning { get; set; }
    }

    public class EducationAllowanceDDOBResponse
    {
        public string p_dob { get; set; }
    }

    public class PUsernameList
    {
        public string username { get; set; }
    }

    public class PApplicationaccessList
    {
        public string application_name { get; set; }
        public int application_id { get; set; }
    }

    public class PCompanyList
    {
        public string company_name { get; set; }
    }

    public class PEnvironmentList
    {
        public string evinronment_type { get; set; }
    }

    public class ApplicationAccessDropDownResponse
    {
        public List<PUsernameList> p_username_list { get; set; }
        public List<PApplicationaccessList> p_applicationaccess_list { get; set; }
        public List<PCompanyList> p_company_list { get; set; }
        public List<PEnvironmentList> p_environment_list { get; set; }
    }

    public class PRnameList
    {
        public string responsibility_name { get; set; }
        public string description { get; set; }
    }

    public class ApplicationAccessRNameResponse
    {
        public List<PRnameList> p_rname_list { get; set; }
    }

    public class PLettertypeList
    {
        public string letter_type { get; set; }
    }

    public class PPurposelettertypeList
    {
        public string purpose_letter { get; set; }
    }

    public class PVisatypeList
    {
        public string visa_type { get; set; }
    }

    public class PTraveltypeList
    {
        public string travel_destination { get; set; }
    }

    public class PLetterpurposetypeList
    {
        public string letter_purpose { get; set; }
    }

    public class EmployeeLetterDropDownResponse
    {
        public List<PLettertypeList> p_lettertype_list { get; set; }
        public List<PPurposelettertypeList> p_purposelettertype_list { get; set; }
        public List<PVisatypeList> p_visatype_list { get; set; }
        public List<PTraveltypeList> p_traveltype_list { get; set; }
        public List<PLetterpurposetypeList> p_letterpurposetype_list { get; set; }
    }

    public class PReimbursementtypeList
    {
        public string reimbursement_type { get; set; }
    }

    public class ReimbursementDropDownResponse
    {
        public List<PReimbursementtypeList> p_reimbursementtype_list { get; set; }
    }

    public class PMaritalstatu
    {
        public string marital_status { get; set; }
    }

    public class PElligibleamount
    {
        public string val { get; set; }
    }

    public class ReimbursementAmountResponse
    {
        public List<PMaritalstatu> p_maritalstatus { get; set; }
        public List<PElligibleamount> p_elligibleamount { get; set; }
    }

    public class EducationAllowanceServiceRequest
    {
        public string p_user_name { get; set; }
        public int p_contact_person_id { get; set; }
        public string p_dob { get; set; }
        public string p_school { get; set; }
        public string p_term { get; set; }
        public string p_academic_year { get; set; }
        public int p_amount { get; set; }
        public string p_submission_date { get; set; }
        public string p_payable_to { get; set; }
    }

    public class CarLoanServiceRquest
    {
        public string p_user_name { get; set; }
        public string p_person_id { get; set; }
        public string p_mobile_number { get; set; }
        public string p_permanent_residence_address { get; set; }
        public string p_post_box_number { get; set; }
        public string p_justification { get; set; }
        public string p_submission_date { get; set; }
    }

    public class SalaryAdvanceServiceRequest
    {
        public string p_user_name { get; set; }
        public string p_justification { get; set; }
        public string p_submission_date { get; set; }
    }

    public class ApplicationAccessServiceRequest
    {
        public string p_user_name { get; set; }
        public string p_application_access { get; set; }
        public string p_company_name { get; set; }
        public string p_environment_required { get; set; }
        public string p_application_user_name { get; set; }
        public string p_responsibility_name { get; set; }
        public string p_justification { get; set; }
        public string p_submission_date { get; set; }
    }

    public class EmployeeLetterServiceRequest
    {
        public string p_user_name { get; set; }
        public string p_letter_type { get; set; }
        public string p_purpose_of_letter { get; set; }
        public string p_visa_type { get; set; }
        public string p_travel_destination { get; set; }
        public string p_travel_date_from { get; set; }
        public string p_justification { get; set; }
        public string p_travel_date_to { get; set; }
        public string p_letter_purpose { get; set; }
        public string p_language_of_letter { get; set; }
        public string p_submission_date { get; set; }
    }

    public class ReimbursementServiceRequest
    {
        public string p_user_name { get; set; }
        public string p_reimbursement_type { get; set; }
        public string p_marital_status { get; set; }
        public string p_club_name { get; set; }
        public string p_membership_start_date { get; set; }
        public string p_membership_end_date { get; set; }
        public string p_bill_amount { get; set; }
        public string p_amount_payable { get; set; }
        public string p_justification { get; set; }
        public string p_allowance_amount { get; set; }
        public string p_submission_date { get; set; }
    }

    public class PushNotificationRequest
    {
        public int p_user_id { get; set; }
        public string p_user_name { get; set; }
        public string p_device_id { get; set; }
        public string p_reg_token { get; set; }
        public string p_platform { get; set; }
        public string p_isactive { get; set; }
        public string p_creation_date { get; set; }
        public string p_created_by { get; set; }
        public string p_last_updated_date { get; set; }
        public string p_last_updated_by { get; set; }
    }

    public class AirTicketServiceRequest
    {
        public string p_user_name { get; set; }
        public string p_justification { get; set; }
        public string p_submission_date { get; set; }
    }

    public class GetApproverUserList
    {
        public int person_id { get; set; }
        public string user_name { get; set; }
        public string full_name { get; set; }
        public string leave_balance { get; set; }
    }

    public class GetApproverUserListResponse
    {
        public List<GetApproverUserList> get_approver_user_list { get; set; }
    }

    public class GetFutureLeaveSitSummary
    {
        public string leave_type { get; set; }
        public string leave_category { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }
        public string approval_status { get; set; }
        public string absence_status { get; set; }
        public int absence_days { get; set; }
        public string full_name { get; set; }
        public string approver_name { get; set; }
        public string date_of_submission { get; set; }
        //public DateTime? sent_date { get; set; }
    }

    public class GetFutureLeaveSitSummaryResponse
    {
        public List<GetFutureLeaveSitSummary> get_future_leave_sit_summary { get; set; }
    }


    //Approvals

    public class PLineDetail
    {
        //PO
        public int line_number { get; set; }
        public int item_number { get; set; }
        public object revision { get; set; }
        public string item_description { get; set; }
        public string uom { get; set; }
        public int quantity { get; set; }
        public object unit_price { get; set; }
        public double line_amount { get; set; }
        public string charge_account { get; set; }
        public string need_by_date { get; set; }
        public object project { get; set; }
        public object task { get; set; }

        //Invoice
        public int distribution_number { get; set; }
        public string gl_account { get; set; }
        public string charge_description { get; set; }
        public string line_description { get; set; }
        public DateTime accounting_date { get; set; }
        public double amount { get; set; }
        public object po_number { get; set; }
        public object receipt_number { get; set; }
        public object quantity_invoiced { get; set; }
        //public object unit_price { get; set; }

        //Requisition
        //public int line_number { get; set; }
        //public int distribution_number { get; set; }
        //public string gl_account { get; set; }
        public int line_quantity { get; set; }
        //public string charge_description { get; set; }
        public int percent_allocation_value { get; set; }
    }

    public class PONotificationDetailResponse
    {
        public string P_HEADER_DETAILS { get; set; }
        public List<PLINEDETAIL> P_LINE_DETAILS { get; set; }

        public string P_APPROVERS { get; set; }
    }




  

    public class PREQLINE
    {
        public int line_number { get; set; }
        public string description { get; set; }
        public string supplier { get; set; }
        public string cost_center { get; set; }
        public string unit { get; set; }
        public string quantity { get; set; }
        public string price { get; set; }
        public string amount { get; set; }
    }

    public class PLINEDETAIL
    {
        public int line_number { get; set; }
        public string revision { get; set; }
        public string item_description { get; set; }
        public string uom { get; set; }
        public string quantity { get; set; }
        public string unit_price { get; set; }
        public string line_amount { get; set; }

        //public int line_number { get; set; }
        public int distribution_number { get; set; }
        public string gl_account { get; set; }
        public int line_quantity { get; set; }
        public string percent_allocation_value { get; set; }
    }

    public class RequisitionNotificationDetailResponse
    {
        public string P_HEADER_DETAILS { get; set; }
        public string P_REQ_ADD_DETAILS { get; set; }
        public List<PREQLINE> P_REQ_LINE { get; set; }
        public List<PLINEDETAIL> P_LINE_DETAILS { get; set; }
    }


    public class HassadTransactionWorkFLow
    {
        public string P_LINE_DETAILS { get; set; }
        public string P_APPROVERS { get; set; }
    }









}
