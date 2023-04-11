using System;
using System.ComponentModel;
using System.Diagnostics;
using HassadFood.Interface;
using HassadFood.WebService;
using Xamarin.Forms;

namespace HassadFood.ViewModels
{
    public class LeaveSummaryModel : ViewModelBase
    {
        Page _page;
        IUserDetails userDetails;
        public LeaveSummaryModel(Page page) : base(page)
        {
            Title = "Leave Summary Page";
            userDetails = DependencyService.Get<IUserDetails>();
            _page = page;
            GetLeaveSitSummaryInformation = new GetLeaveSitSummaryResponse();
            GetLeaveSitSummaryInformation.get_leave_sit_summary = new System.Collections.Generic.List<GetLeaveSitSummary>();
            GetLeaveSitSummaryInformation.get_leave_sit_summary.Clear();
            if (DataInfo.GetLeaveSitSummaryInformation == null)
                GetLeaveSummary();
            else
                GetLeaveSitSummaryInformation = DataInfo.GetLeaveSitSummaryInformation;
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
            GetLeaveSummary();
        }

        async void GetLeaveSummary()
        {
            string showAlert = null;

            if (IsListBusy)
                return;

            IsListBusy = true;

            try
            {
                var result = await userDetails.GetLeaveSitSummary();
                if (result)
                {
                    GetLeaveSitSummaryInformation = DataInfo.GetLeaveSitSummaryInformation;
                    if (GetLeaveSitSummaryInformation != null && GetLeaveSitSummaryInformation.get_leave_sit_summary.Count > 0)
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
