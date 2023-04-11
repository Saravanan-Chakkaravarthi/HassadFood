using System;
using System.ComponentModel;
using System.Diagnostics;
using HassadFood.Interface;
using HassadFood.WebService;
using Xamarin.Forms;

namespace HassadFood.ViewModels
{
    public class WorkListModel : ViewModelBase
    {
        Page _page;
        IUserDetails userDetails;
        public WorkListModel(Page page) : base(page)
        {
            Title = "Work List Page";
            WorkListInformation = DataInfo.WorkListInformation;
            userDetails = DependencyService.Get<IUserDetails>();
            _page = page;
            if (WorkListInformation == null)
                GetWorkList();
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
            GetWorkList();
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
                    if (WorkListInformation != null && WorkListInformation.get_worklist_user.Count > 0)
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
