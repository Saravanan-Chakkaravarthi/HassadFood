using System;
using System.ComponentModel;
using System.Diagnostics;
using HassadFood.Interface;
using HassadFood.Views;
using HassadFood.WebService;
using Xamarin.Forms;

namespace HassadFood.ViewModels
{
    public class MainModel : ViewModelBase
    {
        Page _page;
        ILoginManager _loginmanager;
        IUserDetails userDetails;
        public MainModel(Page page, ILoginManager loginManager) : base(page)
        {
            Title = "Main Page";
            userDetails = DependencyService.Get<IUserDetails>();
            _page = page;
            _loginmanager = loginManager;
            SelectedType = "Open";
            if (DataInfo.WorkListInformation == null)
                GetWorkList();
            else
            {
                WorkListInformation = DataInfo.WorkListInformation;
                if(DataInfo.GetLeaveBalanceInformation != null && DataInfo.GetLeaveBalanceInformation.p_entitlement != null)
                    LeaveBalance = DataInfo.GetLeaveBalanceInformation.p_entitlement;
            }
        }

        string selectedType;

        public string SelectedType
        {
            get { return selectedType; }
            set
            {
                SetProperty(ref selectedType, value);
                //GetNotifications();
            }
        }


        bool emptyList;

        public bool EmptyList
        {
            
            get { return emptyList = false; }
            set { SetProperty(ref emptyList, value); }
        }

        string leaveBalance;

        public string LeaveBalance
        {
            get { return leaveBalance; }
            set { SetProperty(ref leaveBalance, value); }
        }

        async void GetLeaveBalance()
        {
            string showAlert = null;

            if (IsListBusy)
                return;

            IsListBusy = true;

            try
            {
                var result = await userDetails.GetLeaveBalance();
                if (result)
                {
                    LeaveBalance = DataInfo.GetLeaveBalanceInformation.p_entitlement;
                    if (LeaveBalance != null)
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

        async void GetWorkList()
        {
            string showAlert = null;

            if (IsListBusy)
                return;

            IsListBusy = true;

            try
            {
                var result = await userDetails.GetWorkList();
                if (result)
                {
                    WorkListInformation = DataInfo.WorkListInformation;
                    if (WorkListInformation != null && WorkListInformation.get_work_list.Count > 0)
                    {
                        showAlert = "Success";
                    }
                    else
                    {
                        EmptyList = true;
                        showAlert = "Failed";
                    }
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
                GetLeaveBalance();
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

        public new event PropertyChangedEventHandler PropertyChanged;

        protected virtual new void OnPropertyChanged(string propertyName)
        {
            var changed = PropertyChanged;
            if (changed != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        Command _LogoutCommand;

        public Command LogoutCommand => _LogoutCommand ?? (_LogoutCommand = new Command(() => ExecuteLogoutCommand()));

        Command _RefreshCommand;

        public Command RefreshCommand => _RefreshCommand ?? (_RefreshCommand = new Command(() => ExecuteRefreshCommand()));

      

        void ExecuteLogoutCommand()
        {
            _loginmanager.ShowLoginPage();
        }

        public void ExecuteRefreshCommand()
        {
            GetWorkList();
        }
    }
}
