using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using HassadFood.Interface;
using HassadFood.Views;
using HassadFood.WebService;
using Xamarin.Forms;

namespace HassadFood.ViewModels
{
    public class WorkListDetailModel : ViewModelBase
    {
        Page _page;
        IUserDetails userDetails;
        public WorkListDetailModel(Page page, int position) : base(page)
        {
            Title = "Work List Detail Page";
            WorkListInformation = DataInfo.WorkListInformation;
            userDetails = DependencyService.Get<IUserDetails>();
            _page = page;
            SelectedItemWorkList = WorkListInformation.get_work_list[position];
            if (SelectedItemWorkList.action == "N")
            {
                if (SelectedItemWorkList.selected_person_id > 0)
                    GetNotificationDetailHistory();
            }
        }

        void XML_Tag(string obj)
        {
            try
            {
                var xmlData = obj;
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
                        ((WorkListDetailScreen)page).AddStack(_columnname + ":" , _columnvalue);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

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
                p_user_name = (Application.Current.Properties["OracleUserName"] as string).ToUpper(),
                p_itemtype = SelectedItemWorkList.item_type,
                p_item_key = SelectedItemWorkList.item_key,
                p_result = "REJECTED",
                p_notification_id = SelectedItemWorkList.notification_id.ToString(),
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
                Debug.WriteLine(ex.Message);
                showAlert = "Error";
            }
            finally
            {
                IsRejectBusy = false;
                switch (showAlert)
                {
                    case "Error":
                        await page.DisplayAlert("Alert", "Unable to submit request.", "OK");
                        break;
                    case "Success":
                        DataInfo.WorkListInformation = null;
                        await page.DisplayAlert("Alert", "Submitted succesfully.", "OK");
                        await page.Navigation.PopAsync();
                        break;
                    case "Failed":
                        await page.DisplayAlert("Alert", DataInfo.RequestInformation.p_error_msg, "OK");
                        break;
                }
            }
        }

        async void ExecuteSubmitCommand()
        {
            string showAlert = null;

            if (IsBusy)
                return;

            IsBusy = true;

            ActionRequest request = new ActionRequest
            {
                p_user_name = (Application.Current.Properties["OracleUserName"] as string).ToUpper(),
                p_itemtype = SelectedItemWorkList.item_type,
                p_item_key = SelectedItemWorkList.item_key,
                p_result = (SelectedItemWorkList.action != "N") ? "OK" : (SelectedItemWorkList.selected_person_id > 0) ? "APPROVED" : "OK",
                p_notification_id = SelectedItemWorkList.notification_id.ToString(),
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
                        DataInfo.WorkListInformation = null;
                        await page.DisplayAlert("Alert", "Submitted succesfully.", "OK");
                        await page.Navigation.PopAsync();
                        break;
                    case "Failed":
                        await page.DisplayAlert("Alert", DataInfo.RequestInformation.p_error_msg, "OK");
                        break;
                }
            }
        }

        async void GetNotificationDetailHistory()
        {
            string showAlert = null;

            if (IsListBusy)
                return;

            IsListBusy = true;

            try
            {
                var result = await userDetails.GetNotificationDetailHistory(SelectedItemWorkList.item_key.ToString(), SelectedItemWorkList.selected_person_id);
                if (result)
                {
                    if (DataInfo.NotificationDetailInformation != null)
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
                GetAttachmentsInformation();
                switch (showAlert)
                {
                    case "Error":
                        break;
                    case "Success":
                        if (DataInfo.NotificationDetailInformation.p_notification_det != null)
                            XML_Tag(DataInfo.NotificationDetailInformation.p_notification_det);
                        break;
                    case "Failed":
                        break;
                }
            }
        }

        async void GetAttachmentsInformation()
        {
            string showAlert = null;

            if (IsListBusy)
                return;

            IsListBusy = true;

            try
            {
                var result = await userDetails.GetAttachmentsInformation(SelectedItemWorkList.transaction_id.ToString());
                if (result)
                {
                    GetAttachmentInformation = DataInfo.GetAttachmentInformation;
                    if (GetAttachmentInformation != null && GetAttachmentInformation.get_attachment.Count > 0)
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
                //GetActionHistory();
                switch (showAlert)
                {
                    case "Error":
                        break;
                    case "Success":
                        ((WorkListDetailScreen)page).AddStackActionLine();
                        ((WorkListDetailScreen)page).AddStackAttachments();
                        if (GetAttachmentInformation.get_attachment.Count > 0)
                        {
                            foreach (var item in GetAttachmentInformation.get_attachment)
                            {
                                ((WorkListDetailScreen)page).AddStackAttachments(item);
                            }
                        }
                        break;
                    case "Failed":
                        break;
                }
            }
        }

        async void GetActionHistory()
        {
            string showAlert = null;

            if (IsListBusy)
                return;

            IsListBusy = true;

            try
            {
                var result = await userDetails.GetActionHistory(SelectedItemWorkList.notification_id.ToString());
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
                        ((WorkListDetailScreen)page).AddStackActionLine();
                        //((WorkListDetailScreen)page).AddStackAction();
                        foreach (var item in DataInfo.ActionHistoryInformation.get_action_hist)
                        {
                            ((WorkListDetailScreen)page).AddStack("Approver:", item.seq_no.ToString());
                            ((WorkListDetailScreen)page).AddStack("Name:", item.role);
                            ((WorkListDetailScreen)page).AddStack("Action:", item.action);
                            ((WorkListDetailScreen)page).AddStack("Date:", item.action_date);
                            ((WorkListDetailScreen)page).AddStack("Notes:", item.comments);
                            ((WorkListDetailScreen)page).AddStackActionSeparatorLine();
                        }
                        break;
                    case "Failed":
                        break;
                }
            }
        }
    }
}
