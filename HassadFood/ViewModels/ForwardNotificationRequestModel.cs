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
    public class ForwardNotificationRequestModel : ViewModelBase
    {
        Page _page;
        IUserDetails userDetails;
        public ForwardNotificationRequestModel(Page page, GetWorkList selected_notification) : base(page)
        {
            Title = "Forward Notification Page";
            _page = page;
            SelectedNotification = selected_notification;
            // WorkListInformation = DataInfo.WorkListInformation;
            userDetails = DependencyService.Get<IUserDetails>();
            //if (DataInfo.ReassignUserInformation != null && DataInfo.ReassignUserInformation.p_return.Count > 0)
            //    ReassignUserInformation = DataInfo.ReassignUserInformation;
            //else
                //GetReassignUser();
           
            ReassignUser = "Select User";
        }

        //PReturn selectedReassignItem;

        //public PReturn SelectedReassignItem
        //{
        //    get { return selectedReassignItem; }
        //    set { SetProperty(ref selectedReassignItem, value); }
        //}

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

        //Command _SubmitCommand;

        //public Command SubmitCommand => _SubmitCommand ?? (_SubmitCommand = new Command(() => ExecuteSubmitCommand()));

        Command _SearchCommand;

        public Command SearchCommand => _SearchCommand ?? (_SearchCommand = new Command(() => ExecuteSearchCommand()));

        async void ExecuteSearchCommand()
        {
            SearchView = !SearchView;
        }

        //async void ExecuteSubmitCommand()
        //{
        //    if (ReassignUser == "Select User")
        //    {
        //        await page.DisplayAlert("Alert", "Select the User.", "OK");
        //        return;
        //    }

        //    string showAlert = null;

        //    if (IsBusy)
        //        return;

        //    IsBusy = true;

        //    var obj = SelectedNotification;

        //    ActionRequest request = new ActionRequest
        //    {
        //        p_user_name = DataInfo.UserInformation.get_user_auth_p[0].OracleUserName.ToString().ToUpper(),
        //        p_itemtype = "",
        //        p_item_key = SelectedNotification.item_key,
        //        p_result = "REASSIGN",
        //        p_notification_id = SelectedNotification.notification_id,
        //        p_comments = Comments,
        //        p_fwd_user = ReassignName
        //    };

        //    try
        //    {
        //        var result = await userDetails.PutActionRequest(request);
        //        if (result)
        //        {
        //            if (DataInfo.RequestInformation != null)
        //            {
        //                if (DataInfo.RequestInformation.p_success_flag == "Y")
        //                {
        //                    showAlert = "Success";
        //                    PutAttachmentsRequest();
        //                }
        //                else
        //                    showAlert = "Failed";
        //            }
        //            else
        //                showAlert = "Error";
        //        }
        //        else
        //            showAlert = "Error";
        //    }
        //    catch (Exception ex)
        //    {

        //        showAlert = "Error";
        //    }
        //    finally
        //    {
        //        IsBusy = false;
        //        switch (showAlert)
        //        {
        //            case "Error":
        //                await page.DisplayAlert("Alert", "Unable to submit request.", "OK");
        //                break;
        //            case "Success":
        //                DataInfo.WorkListInformation = null;
        //                await page.DisplayAlert("Alert", "Submitted succesfully.", "OK");
        //                page.Navigation.RemovePage(page.Navigation.NavigationStack[page.Navigation.NavigationStack.Count - 1]);
        //                await page.Navigation.PopAsync();
        //                break;
        //            case "Failed":
        //                await page.DisplayAlert("Alert", DataInfo.RequestInformation.p_error_msg, "OK");
        //                break;
        //        }
        //    }
        //}

        //public async void GetReassignUser()
        //{
        //    string showAlert = null;

        //    if (IsListBusy)
        //        return;

        //    IsListBusy = true;

        //    try
        //    {
        //        var result = await userDetails.GetReassignUser("abcd");
        //        if (result)
        //        {
        //            if (DataInfo.ReassignUserInformation != null && DataInfo.ReassignUserInformation.p_return.Count > 0)
        //            {
        //                showAlert = "Success";
        //                ReassignUserInformation = DataInfo.ReassignUserInformation;
        //            }
        //            else
        //                showAlert = "Failed";
        //        }
        //        else
        //            showAlert = "Error";
        //    }
        //    catch (Exception ex)
        //    {

        //        showAlert = "Error";
        //    }
        //    finally
        //    {
        //        IsListBusy = false;
        //        switch (showAlert)
        //        {
        //            case "Error":
        //                break;
        //            case "Success":
        //                break;
        //            case "Failed":
        //                break;
        //        }
        //    }
        //}
    }

}
