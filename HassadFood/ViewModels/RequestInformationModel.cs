using HassadFood.Interface;
using HassadFood.WebService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HassadFood.ViewModels
{
    public class RequestInformationModel : ViewModelBase
    {
        Page _page;
        IUserDetails userDetails;
        public RequestInformationModel(Page page, GetWorkList selected_notification) : base(page)
        {


            Title = "Request Information Page";
            _page = page;
            SelectedNotification = selected_notification;
            //WorkListInformation = DataInfo.WorkListInformation;
            userDetails = DependencyService.Get<IUserDetails>();
            if (DataInfo.ReassignUserInformation != null && DataInfo.ReassignUserInformation.get_reassign_users.Count > 0)
                ReassignUserInformation = DataInfo.ReassignUserInformation;
            else
                //GetReassignUser();
                //SelectedItemWorkList = WorkListInformation.get_worklist_p[position];
                ReassignUser = "Select User";

            //for custom alert - start
            AlertHead = "Alert";
            AlertMessage = "Do you want to confirm to submit ?";
            IsSubmitStackVisible = true;
            IsOkStackVisible = false;
            //for custom alert - end

        }

        GetReassignUser selectedReassignItem;

        public GetReassignUser SelectedReassignItem
        {
            get { return selectedReassignItem; }
            set { SetProperty(ref selectedReassignItem, value); }
        }

        string reassignUser;

        public string ReassignUser
        {
            get { return reassignUser; }
            set { SetProperty(ref reassignUser, value); }
        }

        bool searchView;

        public bool SearchView
        {
            get { return searchView; }
            set { SetProperty(ref searchView, value); }
        }

        string reassignName;

        public string ReassignName
        {
            get { return reassignName; }
            set { SetProperty(ref reassignName, value); }
        }

        string searchText;

        public string SearchText
        {
            get { return searchText; }
            set { if (searchText != value) { searchText = value; OnPropertyChanged("SearchText"); } }
        }

        string comments;

        public string Comments
        {
            get { return comments; }
            set { if (comments != value) { comments = value; OnPropertyChanged("Comments"); } }
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


        //for custom alert - start

        bool IsRequestSuccess = false;

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

            AlertHead = "Alert";
            AlertMessage = "Do you want to confirm to submit ?";
            IsSubmitStackVisible = true;
            IsOkStackVisible = false;


        }


        Command _AlertOkCommand;
        public Command AlertOkCommand => _AlertOkCommand ?? (_AlertOkCommand = new Command(() => ExecuteAlertOkCommand()));

        async void ExecuteAlertOkCommand()
        {
            CustomAlertView = false;

            if (IsRequestSuccess)
            {
                page.Navigation.RemovePage(page.Navigation.NavigationStack[page.Navigation.NavigationStack.Count - 1]);
                await page.Navigation.PopAsync();

            }
        }




        Command _AlertSubmitCommand;
        public Command AlertSubmitCommand => _AlertSubmitCommand ?? (_AlertSubmitCommand = new Command(() => ExecuteAlertSubmitCommand()));

        void ExecuteAlertSubmitCommand()
        {
            ExecuteSubmitCommand();
        }



        Command _AlertCancelCommand;
        public Command AlertCancelCommand => _AlertCancelCommand ?? (_AlertCancelCommand = new Command(() => ExecuteAlertCancelCommand()));

        void ExecuteAlertCancelCommand()
        {
            CustomAlertView = false;
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


        Command _SubmitCommand;

        public Command SubmitCommand => _SubmitCommand ?? (_SubmitCommand = new Command(() => ExecuteSubmitCommand()));

        Command _SearchCommand;

        public Command SearchCommand => _SearchCommand ?? (_SearchCommand = new Command(() => ExecuteSearchCommand()));

        async void ExecuteSearchCommand()
        {
            SearchView = !SearchView;
        }

        async void ExecuteSubmitCommand()
        {
            if (ReassignUser == "Select User")
            {
                // await page.DisplayAlert("Alert", "Select the User.", "OK");
                // return;
                ShowAlert("Select the User.", false, true, true);
                return;
            }

            if (Comments == null || Comments == "")
            {
                //await page.DisplayAlert("Alert", "Enter the inforamtion requested.", "OK");
                //return;
                ShowAlert("Enter the information requested.", false, true, true);
                return;
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
                p_result = "REQUEST_MORE_INFO",
                p_notification_id = SelectedNotification.notification_id.ToString(),
                p_comments = Comments,
                p_fwd_user = ReassignName.ToUpper()
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
                        DataInfo.WorkListInformation = null;
                        IsRequestSuccess = true;
                        ShowAlert("Submitted succesfully.", false, true, true);
                        //await page.DisplayAlert("Alert", "Submitted succesfully.", "OK");
                        //page.Navigation.RemovePage(page.Navigation.NavigationStack[page.Navigation.NavigationStack.Count - 1]);
                        //await page.Navigation.PopAsync();
                        break;
                    case "Failed":
                        //await page.DisplayAlert("Alert", DataInfo.RequestInformation.p_error_msg, "OK");
                        ShowAlert(DataInfo.RequestInformation.p_error_msg, false, true, true);
                        break;
                }
            }
        }

        //    public async void GetReassignUser()
        //    {
        //        string showAlert = null;

        //        if (IsListBusy)
        //            return;

        //        IsListBusy = true;

        //        try
        //        {
        //            var result = await userDetails.GetMoreInfoUsers(SelectedNotification.type, SelectedNotification.item_key);
        //            if (result)
        //            {
        //                if (DataInfo.ReassignUserInformation != null && DataInfo.ReassignUserInformation.get_reassign_users.Count > 0)
        //                {
        //                    showAlert = "Success";
        //                    ReassignUserInformation = DataInfo.ReassignUserInformation;
        //                }
        //                else
        //                    showAlert = "Failed";
        //            }
        //            else
        //                showAlert = "Error";
        //        }
        //        catch (Exception ex)
        //        {

        //            showAlert = "Error";
        //        }
        //        finally
        //        {
        //            IsListBusy = false;
        //            switch (showAlert)
        //            {
        //                case "Error":
        //                    break;
        //                case "Success":
        //                    break;
        //                case "Failed":
        //                    break;
        //            }
        //        }
        //    }
        //}
    }
}
