using System;
using System.ComponentModel;
using System.Diagnostics;
using HassadFood.Interface;
using HassadFood.WebService;
using Xamarin.Forms;

namespace HassadFood.ViewModels
{
    public class FutureLeaveSummaryModel : ViewModelBase
    {
        Page _page;
        IUserDetails userDetails;
        public FutureLeaveSummaryModel(Page page) : base(page)
        {
            Title = "Future Leave Summary Page";
            userDetails = DependencyService.Get<IUserDetails>();
            _page = page;
            GetFutureLeaveSitSummaryInformation = new GetFutureLeaveSitSummaryResponse();
            GetFutureLeaveSitSummaryInformation.get_future_leave_sit_summary = new System.Collections.Generic.List<GetFutureLeaveSitSummary>();
            GetFutureLeaveSitSummaryInformation.get_future_leave_sit_summary.Clear();
            DataInfo.GetFutureLeaveSitSummaryInformation = null;
            if (DataInfo.GetFutureLeaveSitSummaryInformation == null)
                GetApproverUserList();
            else
                GetFutureLeaveSitSummaryInformation = DataInfo.GetFutureLeaveSitSummaryInformation;
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
            GetApproverUserList();
        }

        async void GetApproverUserList()
        {
            string showAlert = null;

            if (IsListBusy)
                return;

            IsListBusy = true;

            try
            {
                var result = await userDetails.GetApproverUserList();
                if (result)
                {
                    GetFutureLeaveSitSummaryInformation = DataInfo.GetFutureLeaveSitSummaryInformation;
                    if (GetFutureLeaveSitSummaryInformation != null && GetFutureLeaveSitSummaryInformation.get_future_leave_sit_summary.Count > 0)
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
