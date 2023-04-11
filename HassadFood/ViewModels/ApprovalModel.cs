using HassadFood.Interface;
using HassadFood.Views;
using HassadFood.WebService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xamarin.Forms;

namespace HassadFood.ViewModels
{
    public class ApprovalModel : ViewModelBase
    {
        Page _page;
        IUserDetails userDetails;
        List<XML_Tag> NotificationDetails;
        int _position;
        public GetWorkList selected_notifications1;



       
        public ApprovalModel(Page page, GetWorkList selected_notifications) : base(page)
        {
            int position = 0;

            Title = "Work List Detail Page";
            WorkListInformation = DataInfo.WorkListInformation;
            userDetails = DependencyService.Get<IUserDetails>();
            _page = page;
            _position = position;
            NotificationDetails = new List<XML_Tag>();
            SelectedNotification = selected_notifications;
            selected_notifications1 = selected_notifications;


            switch (selected_notifications.type)
            {
                case "PO Approval":
                    GetPONotificationDetail();
                    
                    break;
                case "Requisition":
                    GetReqNotificationDetails();
                    break;
        
                case "Hassad Transactions Workflow":
                    GetTransactionWorkFlow();
                    break;



            }

            if (selected_notifications.action == "Y")
                isCommentBox = !isCommentBox;



            AlertHead = "Alert";
            AlertMessage = "Do you want to confirm to submit ?";
            IsSubmitStackVisible = true;
            IsOkStackVisible = false;


        }

        private async void GetTransactionWorkFlow()
        {
            string showAlert = null;

            if (IsListBusy)
                return;

            IsListBusy = true;

            try
            {
                var result = await userDetails.GetTransactionWorkFlow(SelectedNotification.item_key.ToString());
                if (result)
                {
                    HassadTransactionWorkFLowResponseInformation = DataInfo.HassadTransactionWorkFLowResponseInformation;
                    if (HassadTransactionWorkFLowResponseInformation != null)
                    {
                        showAlert = "Success";
                        //POLineView = !POLineView;
                    }
                    else
                        showAlert = "Failed";
                }
                else
                    showAlert = "Error";
            }
            catch (Exception ex)
            {

                showAlert = "Error";
            }
            finally
            {
                GetActionHistory();
                IsListBusy = false;


                switch (showAlert)
                {
                    case "Error":
                        ((ApprovalScreen)page).ShowStackLineButtons();
                        break;
                    case "Success":
                        ((ApprovalScreen)page).ShowStackLineButtons();
                        if (HassadTransactionWorkFLowResponseInformation != null)
                            //    XML_Tag(PONotificationDetailResponseInformation.P_HEADER_DETAILS.Replace("&", "and"));
                            //XML_ApproverTag(PONotificationDetailResponseInformation.P_APPROVERS.Replace("&", "and"));
                            XML_Tag(HassadTransactionWorkFLowResponseInformation.P_LINE_DETAILS.Replace("&", "and"));
                        XML_ApproverTag(HassadTransactionWorkFLowResponseInformation.P_APPROVERS.Replace("&", "and"));
                        break;
                    case "Failed":
                        ((ApprovalScreen)page).ShowStackLineButtons();
                        break;
                }
            }
        }

        bool customAlertView;

        public bool CustomAlertView
        {
            get { return customAlertView; }
            set { SetProperty(ref customAlertView, value); }
        }

        Command _ShowAlertCommand;
        public Command ShowAlertCommand => _ShowAlertCommand ?? (_ShowAlertCommand = new Command(() => ExecuteShowAlertCommand()));

        void ExecuteShowAlertCommand()
        {

            CustomAlertView = !CustomAlertView;

            //for custom alert - start
            AlertHead = "Alert";
            AlertMessage = "Do you want to confirm to submit ?";
            IsSubmitStackVisible = true;
            IsOkStackVisible = false;
            //for custom alert - end
        }


        bool IsRequestSuccess = false;

        Command _AlertOkCommand;
        public Command AlertOkCommand => _AlertOkCommand ?? (_AlertOkCommand = new Command(() => ExecuteAlertOkCommand()));

        async void ExecuteAlertOkCommand()
        {
            CustomAlertView = false;

            if (IsRequestSuccess)
                await page.Navigation.PopAsync();
        }




        Command _AlertSubmitCommand;
        public Command AlertSubmitCommand => _AlertSubmitCommand ?? (_AlertSubmitCommand = new Command(() => ExecuteAlertSubmitCommand()));

        void ExecuteAlertSubmitCommand()
        {
            ExecuteSubmitCommand();
        }

        Command _SubmitCommand;

        public Command SubmitCommand => _SubmitCommand ?? (_SubmitCommand = new Command(() => ExecuteSubmitCommand()));



        Command _AlertCancelCommand;
        public Command AlertCancelCommand => _AlertCancelCommand ?? (_AlertCancelCommand = new Command(() => ExecuteAlertCancelCommand()));

        void ExecuteAlertCancelCommand()
        {
            CustomAlertView = false;
        }


        async void ExecuteSubmitCommand()
        {
            string temp = string.Empty;
            switch (SelectedNotification.action)
            {
                case "N":
                    temp = "APPROVED";
                    break;
                case "Y":
                    temp = "RESPOND";
                    break;
                default:
                    temp = "OK";
                    break;
            }
            string showAlert = null;

            if (IsBusy)
                return;

            IsBusy = true;

            ActionRequest request = new ActionRequest
            {
                p_user_name = Application.Current.Properties["OracleUserName"] as string,
                p_itemtype = SelectedNotification.type,
                p_item_key = SelectedNotification.item_key,
                p_result = temp,
                p_notification_id = SelectedNotification.notification_id.ToString(),
                p_comments = Comments,
                p_fwd_user = ""
            };

            try
            {
                var result = await userDetails.PutActionRequest(request);
                if (result)
                {
                    if (DataInfo.RequestInformation != null)
                    {
                        if (DataInfo.RequestInformation.p_success_flag == "Y")
                        {
                            showAlert = "Success";
                            PutAttachmentsRequest();
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

                showAlert = "Error";
            }
            finally
            {
                IsBusy = false;
                switch (showAlert)
                {
                    case "Error":
                        //await page.DisplayAlert("Alert", "Unable to submit request.", "OK");
                        ShowAlert("Unable to submit request.", false, true, true);
                        break;
                    case "Success":
                        DataInfo.WorkListInformation = null;
                        //await page.DisplayAlert("Alert", "Submitted succesfully.", "OK");
                        //await page.Navigation.PopAsync();
                        IsRequestSuccess = true;
                        ShowAlert("Submitted succesfully.", false, true, true);
                        break;
                    case "Failed":
                        // await page.DisplayAlert("Alert", DataInfo.RequestInformation.p_error_msg, "OK");
                        ShowAlert(DataInfo.RequestInformation.p_error_msg, false, true, true);
                        break;
                }
            }
        }


        async void SubmitAlertCommand()
        {
            CustomAlertView = !CustomAlertView;
            AlertHead = "Alert";
            AlertMessage = "Do you want to confirm to submit ?";
            IsSubmitStackVisible = true;
            IsOkStackVisible = false;

        }

        async void ShowAlert(string message, bool submit_stack_visibility, bool ok_stack_visibility, bool alert_view_visibility)
        {
            AlertHead = "Alert";
            AlertMessage = message;
            IsSubmitStackVisible = submit_stack_visibility;
            IsOkStackVisible = ok_stack_visibility;
            CustomAlertView = alert_view_visibility;


        }

        async void GetActionHistory()
        {
            string showAlert = null;

            if (IsListBusy)
                return;

            IsListBusy = true;

            try
            {
                var result = await userDetails.GetActionHistory(SelectedNotification.notification_id.ToString());
                if (result)
                {
                    if (DataInfo.ActionHistoryInformation != null && DataInfo.ActionHistoryInformation.get_action_hist.Count > 0)
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

                showAlert = "Error";
            }
            finally
            {
                IsListBusy = false;
                if (SelectedNotification.action == "Y")
                    //GetMoreInfo();
                switch (showAlert)
                {
                    case "Error":
                        break;
                    case "Success":
                        bool isAction = false;


                        //case "PO Approval":
                        //    GetPONotificationDetail();

                        //    break;
                        //case "Requisition":
                        //    GetReqNotificationDetails();
                        //    break;

                            if (SelectedNotification.type.Equals("PO Approval") || SelectedNotification.type.Equals("Requisition"))
                        {
                            foreach (var item in DataInfo.ActionHistoryInformation.get_action_hist)
                            {


                                if (item.action.Equals("Pending"))
                                {
                                    isAction = true;
                                    break;
                                }

                            }
                        }
                        else
                        {
                            isAction = false;
                        }

                        //  ((NotificationsDetailsScreen)page).ShowStackLineButtons();

                        if (isAction)
                        {
                            ((ApprovalScreen)page).AddStackActionLine();
                            ((ApprovalScreen)page).AddStackAction();
                            foreach (var item in DataInfo.ActionHistoryInformation.get_action_hist)
                            {
                                if (item.role != null)
                                {
                                    //if (item.action.Equals("Pending"))//if (item.action != null || !item.action.Equals(" "))
                                    // {
                                    // ((NotificationsDetailsScreen)page).AddStackSq(item.seq_no);
                                    ((ApprovalScreen)page).AddStackBold("Name", item.role);
                                    ((ApprovalScreen)page).AddStack("Action", item.action);
                                    if (!(item.action_date.Equals(" ")))
                                        ((ApprovalScreen)page).AddStack("Date", item.action_date);
                                    if (!(item.comments.Equals(" ")))
                                        ((ApprovalScreen)page).AddStack("Notes", item.comments);
                                    ((ApprovalScreen)page).AddStackActionSeparatorLine();
                                    // }
                                }
                            }
                        }

                        else
                        {
                            ((ApprovalScreen)page).AddStackActionLine();
                            ((ApprovalScreen)page).AddStackAction();
                            foreach (var item1 in DataInfo.ActionHistoryInformation.get_action_hist)
                            {
                                if (item1.role != null)
                                {
                                    // if (item.action.Equals("Pending"))//if (item.action != null || !item.action.Equals(" "))
                                    // {
                                    // ((NotificationsDetailsScreen)page).AddStackSq(item.seq_no);
                                    ((ApprovalScreen)page).AddStackBold("Name", item1.role);
                                    ((ApprovalScreen)page).AddStack("Action", item1.action);
                                    if (!(item1.action_date.Equals(" ")))
                                        ((ApprovalScreen)page).AddStack("Date", item1.action_date);
                                    if (!(item1.comments.Equals(" ")))
                                        ((ApprovalScreen)page).AddStack("Notes", item1.comments);
                                    ((ApprovalScreen)page).AddStackActionSeparatorLine();
                                    // }
                                }
                            }
                        }





                        break;
                    case "Failed":
                        break;
                }
            }
        }


        Command _RejectCommand;

        public Command RejectCommand => _RejectCommand ?? (_RejectCommand = new Command(() => ExecuteRejectCommand()));

        async void ExecuteRejectCommand()
        {
            string showAlert = null;

            if (IsRejectBusy)
                return;

            IsRejectBusy = true;

            ActionRequest request = new ActionRequest
            {
                p_user_name = Application.Current.Properties["OracleUserName"] as string,
                p_itemtype = SelectedNotification.type,
                p_item_key = SelectedNotification.item_key,
                p_result = "REJECTED",
                p_notification_id = SelectedNotification.notification_id.ToString(),
                p_comments = Comments,
                p_fwd_user = ""
            };

            try
            {
                var result = await userDetails.PutActionRequest(request);
                if (result)
                {
                    if (DataInfo.RequestInformation != null)
                    {
                        if (DataInfo.RequestInformation.p_success_flag == "Y")
                        {
                            showAlert = "Success";
                            PutAttachmentsRequest();
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

                showAlert = "Error";
            }
            finally
            {
                IsRejectBusy = false;
                switch (showAlert)
                {
                    case "Error":
                        //await page.DisplayAlert("Alert", "Unable to submit request.", "OK");
                        ShowAlert("Unable to submit request.", false, true, true);
                        break;
                    case "Success":
                        DataInfo.WorkListInformation = null;
                        // await page.DisplayAlert("Alert", "Submitted succesfully.", "OK");
                        // await page.Navigation.PopAsync();
                        IsRequestSuccess = true;
                        ShowAlert("Submitted succesfully.", false, true, true);
                        break;
                    case "Failed":
                        //await page.DisplayAlert("Alert", DataInfo.RequestInformation.p_error_msg, "OK");
                        ShowAlert(DataInfo.RequestInformation.p_error_msg, false, true, true);
                        break;
                }
            }
        }

        private async void GetReqNotificationDetails()
        {
            string showAlert = null;

            if (IsListBusy)
                return;

            IsListBusy = true;

            try
            {
                var result = await userDetails.GetRequisitionNotificationDetails(SelectedNotification.item_key.ToString());
                if (result)
                {
                    RequisitionNotificationDetailResponseInformation = DataInfo.RequisitionNotificationDetailResponseInformation;
                    if (RequisitionNotificationDetailResponseInformation != null)
                    {
                        showAlert = "Success";
                        if (RequisitionNotificationDetailResponseInformation.P_REQ_LINE.Count > 0)
                            RequisitionLineView = !RequisitionLineView;
                        if (RequisitionNotificationDetailResponseInformation.P_LINE_DETAILS.Count > 0)
                            RequisitionDistributionView = !RequisitionDistributionView;
                    }
                    else
                        showAlert = "Failed";
                }
                else
                    showAlert = "Error";
            }
            catch (Exception ex)
            {

                showAlert = "Error";
            }
            finally
            {
                IsListBusy = false;
                GetActionHistory();
                // ((NotificationsDetailsScreen)page).ShowStackLineButtons();
                //GetAttachmentsInformation();
                switch (showAlert)
                {
                    case "Error":
                        ((ApprovalScreen)page).ShowStackLineButtons();
                        break;
                    case "Success":
                        ((ApprovalScreen)page).ShowStackLineButtons();
                        if (RequisitionNotificationDetailResponseInformation.P_HEADER_DETAILS != null)
                            XML_Tag(RequisitionNotificationDetailResponseInformation.P_HEADER_DETAILS.Replace("&", "and"));
                        if (RequisitionNotificationDetailResponseInformation.P_REQ_ADD_DETAILS != null)
                            XML_Tag(RequisitionNotificationDetailResponseInformation.P_REQ_ADD_DETAILS.Replace("&", "and"));
                        break;
                    case "Failed":
                        ((ApprovalScreen)page).ShowStackLineButtons();
                        break;
                }
            }


        }

        bool pOLineView;
        public bool POLineView
        {
            get { return pOLineView; }
            set { SetProperty(ref pOLineView, value); }
        }

        bool invoiceAmountView;
        public bool InvoiceAmountView
        {
            get { return invoiceAmountView; }
            set { SetProperty(ref invoiceAmountView, value); }
        }

        bool invoiceLineView;
        public bool InvoiceLineView
        {
            get { return invoiceLineView; }
            set { SetProperty(ref invoiceLineView, value); }
        }

        bool invoiceDistributionView;
        public bool InvoiceDistributionView
        {
            get { return invoiceDistributionView; }
            set { SetProperty(ref invoiceDistributionView, value); }
        }

        bool requisitionLineView;
        public bool RequisitionLineView
        {
            get { return requisitionLineView; }
            set { SetProperty(ref requisitionLineView, value); }
        }

        bool requisitionDistributionView;
        public bool RequisitionDistributionView
        {
            get { return requisitionDistributionView; }
            set { SetProperty(ref requisitionDistributionView, value); }
        }

        bool journalBadgeView;
        public bool JournalBadgeView
        {
            get { return journalBadgeView; }
            set { SetProperty(ref journalBadgeView, value); }
        }

        bool isCommentBox;
        public bool IsCommentBox
        {
            get { return isCommentBox; }
            set { SetProperty(ref isCommentBox, value); }
        }

        async void GetPONotificationDetail()
        {
            string showAlert = null;

            if (IsListBusy)
                return;

            IsListBusy = true;

            try
            {
                var result = await userDetails.GetPONotificationDetails(SelectedNotification.item_key.ToString());
                if (result)
                {
                    PONotificationDetailResponseInformation = DataInfo.PONotificationDetailResponseInformation;
                    if (PONotificationDetailResponseInformation != null && PONotificationDetailResponseInformation.P_LINE_DETAILS.Count > 0)
                    {
                        showAlert = "Success";
                        POLineView = !POLineView;
                    }
                    else
                        showAlert = "Failed";
                }
                else
                    showAlert = "Error";
            }
            catch (Exception ex)
            {

                showAlert = "Error";
            }
            finally
            {
                GetActionHistory();
                IsListBusy = false;

               
                switch (showAlert)
                {
                    case "Error":
                        ((ApprovalScreen)page).ShowStackLineButtons();
                        break;
                    case "Success":
                        ((ApprovalScreen)page).ShowStackLineButtons();
                        if (PONotificationDetailResponseInformation.P_HEADER_DETAILS != null)
                            XML_Tag(PONotificationDetailResponseInformation.P_HEADER_DETAILS.Replace("&", "and"));
                            XML_ApproverTag(PONotificationDetailResponseInformation.P_APPROVERS.Replace("&", "and"));
                        break;
                    case "Failed":
                        ((ApprovalScreen)page).ShowStackLineButtons();
                        break;
                }
            }
        }

        //requestinfo

        //Command _RequestCommand;

        //public Command RequestCommand => _RequestCommand ?? (_RequestCommand = new Command(() => ExecuteRequestCommand()));

        //async void ExecuteRequestCommand()
        //{
        //    page.Title = "";
        //    MoreView = false;
        //    await page.Navigation.PushAsync(new RequestInformationScreen(selected_notifications1));
        //}

        void XML_Tag(string xml)
        {
            try
            {
                var xmlData = xml;
                byte[] utf8Bytes = System.Text.Encoding.UTF8.GetBytes(xmlData);
                string s_unicode2 = System.Text.Encoding.UTF8.GetString(utf8Bytes, 0, utf8Bytes.Length);
                s_unicode2 = s_unicode2.Trim();
                var xdoc = XDocument.Parse(s_unicode2);
                var re = xdoc.DescendantNodes();
                var de = xdoc.Descendants("WORKLIST");

                foreach (var element in de.Elements())
                {
                    string _seqno = string.Empty;
                    if (element.Elements("SEQNO").Any())
                        _seqno = element.Element("SEQNO").Value;
                    string _columnname = string.Empty;
                    if (element.Elements("COLUMN_NAME").Any())
                        _columnname = element.Element("COLUMN_NAME").Value;
                    string _columnvalue = string.Empty;
                    if (element.Elements("COLUMN_VALUE").Any())
                        _columnvalue = element.Element("COLUMN_VALUE").Value;
                    if (string.IsNullOrEmpty(_columnvalue))
                        _columnvalue = "";
                    if (!string.IsNullOrEmpty(_columnname))
                        ((ApprovalScreen)page).AddStack(_columnname, _columnvalue);
                   

                }


            }
            catch (Exception ex)
            {

            }

        }

        void XML_ApproverTag(string xml)
        {
            try
            {
                var xmlData = xml;
                byte[] utf8Bytes = System.Text.Encoding.UTF8.GetBytes(xmlData);
                string s_unicode2 = System.Text.Encoding.UTF8.GetString(utf8Bytes, 0, utf8Bytes.Length);
                s_unicode2 = s_unicode2.Trim();
                var xdoc = XDocument.Parse(s_unicode2);
                var re = xdoc.DescendantNodes();
                var de = xdoc.Descendants("WORKLIST");

                foreach (var element in de.Elements())
                {
                    string _seqno = string.Empty;
                    if (element.Elements("SEQNO").Any())
                        _seqno = element.Element("SEQNO").Value;
                    string _columnname = string.Empty;
                    if (element.Elements("COLUMN_NAME").Any())
                        _columnname = element.Element("COLUMN_NAME").Value;
                    string _columnvalue = string.Empty;
                    if (element.Elements("COLUMN_VALUE").Any())
                        _columnvalue = element.Element("COLUMN_VALUE").Value;
                    if (string.IsNullOrEmpty(_columnvalue))
                        _columnvalue = "";
                    if (!string.IsNullOrEmpty(_columnname))
                        ((ApprovalScreen)page).AddApproverStack(_columnname, _columnvalue);


                }


            }
            catch (Exception ex)
            {

            }

        }

      // string comments;

      //  public string Comments
     //   {
      //      get { return comments; }
      //      set { if (comments != value) { comments = value; OnPropertyChanged("Comments"); } }
      //  }

        public List<XML_Tag> ItemNotificationDetails
        {
            get { return NotificationDetails; }
            set { SetProperty(ref NotificationDetails, value); }
        }

        //bool isRejectBusy;

        ///// <summary>
        ///// Gets or sets a value indicating whether this instance is busy.
        ///// </summary>
        ///// <value><c>true</c> if this instance is busy; otherwise, <c>false</c>.</value>
        //public bool IsRejectBusy
        //{
        //    get { return isRejectBusy; }
        //    set
        //    {
        //        if (SetProperty(ref isRejectBusy, value))
        //            IsNotBusy = !isRejectBusy;
        //    }
        //}
        bool isRejectBusy;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is busy.
        /// </summary>
        /// <value><c>true</c> if this instance is busy; otherwise, <c>false</c>.</value>
        


        bool moreView;
        public bool MoreView
        {
            get { return moreView; }
            set { SetProperty(ref moreView, value); }
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

        //Command _SubmitCommand;

        //public Command SubmitCommand => _SubmitCommand ?? (_SubmitCommand = new Command(() => ExecuteSubmitCommand()));

        //Command _RejectCommand;

        //public Command RejectCommand => _RejectCommand ?? (_RejectCommand = new Command(() => ExecuteRejectCommand()));

        //Command _ReassignCommand;

        //public Command ReassignCommand => _ReassignCommand ?? (_ReassignCommand = new Command(() => ExecuteReassignCommand()));

        //Command _RequestCommand;

        //public Command RequestCommand => _RequestCommand ?? (_RequestCommand = new Command(() => ExecuteRequestCommand()));

        //Command _ForwardCommand;

        //public Command ForwardCommand => _ForwardCommand ?? (_ForwardCommand = new Command(() => ExecuteForwardCommand()));

        //Command _ApproveForwardCommand;

        //public Command ApproveForwardCommand => _ApproveForwardCommand ?? (_ApproveForwardCommand = new Command(() => ExecuteApproveForwardCommand()));

        Command _MoreCommand;

        public Command MoreCommand => _MoreCommand ?? (_MoreCommand = new Command(() => ExecuteMoreCommand()));

        Command _SubmitPOLineCommand;

        public Command SubmitPOLineCommand => _SubmitPOLineCommand ?? (_SubmitPOLineCommand = new Command(() => ExecuteSubmitPOLineCommand()));

        Command _ForwardCommand;

        public Command ForwardCommand => _ForwardCommand ?? (_ForwardCommand = new Command(() => ExecuteForwardCommand()));

        async void ExecuteForwardCommand()
        {
            page.Title = "";
            await page.Navigation.PushAsync(new ForwardNotificationScreen(selected_notifications1));
        }



        async void ExecuteSubmitPOLineCommand()
        {
            await page.Navigation.PushAsync(new PONotificationsDetailsScreen());
        }


        Command _SubmitRequisitionLineCommand;

        public Command SubmitRequisitionLineCommand => _SubmitRequisitionLineCommand ?? (_SubmitRequisitionLineCommand = new Command(() => ExecuteSubmitRequisitionLineCommand()));

        async void ExecuteSubmitRequisitionLineCommand()
        {
            await page.Navigation.PushAsync(new PurchaseRequisitionNotificationsDetailsScreen());
        }


        //async void ExecuteSubmitInvoiceLineCommand()
        //{
        //    await page.Navigation.PushAsync(new InvoiceLinesNotificationsDetailsScreen());
        //}

        //async void ExecuteSubmitInvoiceAmountCommand()
        //{
        //    await page.Navigation.PushAsync(new InvoiceAccountNotificationsDetailsScreen());
        //}

        //async void ExecuteSubmitInvoiceDistributionCommand()
        //{
        //    await page.Navigation.PushAsync(new InvoiceDistributionNotificationsDetailsScreen());
        //}

        //async void ExecuteSubmitRequisitionLineCommand()
        //{
        //    await page.Navigation.PushAsync(new PurchaseRequisitionNotificationsDetailsScreen());
        //}
        Command _SubmitRequisitionDistributionCommand;

        public Command SubmitRequisitionDistributionCommand => _SubmitRequisitionDistributionCommand ?? (_SubmitRequisitionDistributionCommand = new Command(() => ExecuteSubmitRequisitionDistributionCommand()));

        async void ExecuteSubmitRequisitionDistributionCommand()
        {
            await page.Navigation.PushAsync(new PurchaseDistributionNotificationsDetailsScreen());
        }


        void ExecuteMoreCommand()
        {
            MoreView = !MoreView;
        }















    }

    public class XML_Tag
    {
        public string Attribute1 { get; set; }
        public string Attribute2 { get; set; }
        public string Attribute3 { get; set; }
    }



   

  



}
