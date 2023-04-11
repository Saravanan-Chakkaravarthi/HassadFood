using System;
using System.ComponentModel;
using System.Diagnostics;
using HassadFood.Interface;
using HassadFood.WebService;
using Xamarin.Forms;

namespace HassadFood.ViewModels
{
    public class FutureLeaveBalanceModel : ViewModelBase
    {
        Page _page;
        IUserDetails userDetails;
        public FutureLeaveBalanceModel(Page page) : base(page)
        {
            Title = "Future Leave Summary Page";
            userDetails = DependencyService.Get<IUserDetails>();
            _page = page;            
            if (DataInfo.GetApproverUserListLeaveBalanceInformation == null)
                GetApproverUserListLeaveBalance();
            else
                GetApproverUserListLeaveBalanceInformation = DataInfo.GetApproverUserListLeaveBalanceInformation;
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

        Command _RefreshCommand;

        public Command RefreshCommand => _RefreshCommand ?? (_RefreshCommand = new Command(() => ExecuteRefreshCommand()));

        public void ExecuteRefreshCommand()
        {
            GetApproverUserListLeaveBalance();
        }

        async void GetApproverUserListLeaveBalance()
        {
            string showAlert = null;

            if (IsListBusy)
                return;

            IsListBusy = true;

            try
            {
                var result = await userDetails.GetApproverUserListLeaveBalance();
                if (result)
                {
                    GetApproverUserListLeaveBalanceInformation = DataInfo.GetApproverUserListLeaveBalanceInformation;
                    if (GetApproverUserListLeaveBalanceInformation != null && GetApproverUserListLeaveBalanceInformation.get_approver_user_list.Count > 0)
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
    }
}
