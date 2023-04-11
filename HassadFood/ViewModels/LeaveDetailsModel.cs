using System;
using System.ComponentModel;
using HassadFood.Interface;
using HassadFood.WebService;
using Xamarin.Forms;

namespace HassadFood.ViewModels
{
    public class LeaveDetailsModel : ViewModelBase
    {
        Page _page;
        IUserDetails userDetails;
        public LeaveDetailsModel(Page page, int position) : base(page)
        {
            Title = "Work List Detail Page";
            LeaveSummaryInformation = DataInfo.LeaveSummaryInformation;
            userDetails = DependencyService.Get<IUserDetails>();
            _page = page;
            SelectedItemLeaveSummary = LeaveSummaryInformation.get_leave_summary[position];
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
    }
}
