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
    public class PONotificationsDetailsModel : ViewModelBase
    {
        Page _page;
        IUserDetails userDetails;
        public PONotificationsDetailsModel(Page page) : base(page)
        {
            Title = "PO Notifications Details Page";
            userDetails = DependencyService.Get<IUserDetails>();
            _page = page;
            PONotificationDetailResponseInformation = DataInfo.PONotificationDetailResponseInformation;
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
