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
    public class PurchaseNotificationsDetailsModel : ViewModelBase
    {
        Page _page;
        IUserDetails userDetails;
        public PurchaseNotificationsDetailsModel(Page page) : base(page)
        {
            Title = "Purchase Requisition Notifications Page";
            userDetails = DependencyService.Get<IUserDetails>();
            _page = page;
            RequisitionNotificationDetailResponseInformation = DataInfo.RequisitionNotificationDetailResponseInformation;
            
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
