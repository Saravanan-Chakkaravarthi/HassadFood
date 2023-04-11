using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using HassadFood.Interface;
using HassadFood.Resx;
using HassadFood.Views;
using HassadFood.WebService;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace HassadFood.ViewModels
{
    public class SideMenuModel : ViewModelBase
    {
        Page _page;
        ObservableCollection<SideMenuList> sideMenuList;
        Command<SideMenuList> _cellSelectedCommand;
        SideMenuList _selectedItem;
        public SideMenuModel(Page page) : base(page)
        {
            Title = "Home";
            UserInformation = DataInfo.UserInformation;
            sideMenuList = new ObservableCollection<SideMenuList>();
           
            //sideMenuList.Add(new SideMenuList() { IconName = "ic_action_settings.png", Title = AppResources.WorkList });
            sideMenuList.Add(new SideMenuList() { MainView = true, MainTitle = "Leave Management", SubView = false, IconName = "", Title = "" });
            sideMenuList.Add(new SideMenuList() { MainView = false, MainTitle = "", SubView = true, IconName = "ic_action_leave.png", Title = AppResources.Leave });
            sideMenuList.Add(new SideMenuList() { MainView = false, MainTitle = "", SubView = true, IconName = "ic_amend.png", Title = AppResources.LeaveAmendment });
            sideMenuList.Add(new SideMenuList() { MainView = false, MainTitle = "", SubView = true, IconName = "ic_cancel.png", Title = AppResources.LeaveCancel });
            sideMenuList.Add(new SideMenuList() { MainView = false, MainTitle = "", SubView = true, IconName = "ic_resume.png", Title = AppResources.LeaveResume });
            sideMenuList.Add(new SideMenuList() { MainView = false, MainTitle = "", SubView = true, IconName = "ic_exit_permit.png", Title = AppResources.ExitPermit });

            //Phase II
            sideMenuList.Add(new SideMenuList() { MainView = false, MainTitle = "", SubView = true, IconName = "ic_air.png", Title = "Air Ticket" });

            sideMenuList.Add(new SideMenuList() { MainView = true, MainTitle = "Forms", SubView = false, IconName = "", Title = "" });
            sideMenuList.Add(new SideMenuList() { MainView = false, MainTitle = "", SubView = true, IconName = "ic_duty_visit.png", Title = AppResources.DutyVisit });

            //Phase II

            sideMenuList.Add(new SideMenuList() { MainView = false, MainTitle = "", SubView = true, IconName = "ic_emp_letter_request.png", Title = "Employee Letter Request" });
            sideMenuList.Add(new SideMenuList() { MainView = false, MainTitle = "", SubView = true, IconName = "ic_educationallowance.png", Title = "Education Allowance Request" });
            sideMenuList.Add(new SideMenuList() { MainView = false, MainTitle = "", SubView = true, IconName = "ic_carloan.png", Title = "Car Loan Request" });
            sideMenuList.Add(new SideMenuList() { MainView = false, MainTitle = "", SubView = true, IconName = "ic_salary_advance.png", Title = "Salary Advance Request" });
            sideMenuList.Add(new SideMenuList() { MainView = false, MainTitle = "", SubView = true, IconName = "ic_appliaction_access_request.png", Title = "Application Access Request" });
            sideMenuList.Add(new SideMenuList() { MainView = false, MainTitle = "", SubView = true, IconName = "ic_clubmembership.png", Title = "Reimbursement Request" });

            sideMenuList.Add(new SideMenuList() { MainView = true, MainTitle = "Extras", SubView = false, IconName = "", Title = "" });
            sideMenuList.Add(new SideMenuList() { MainView = false, MainTitle = "", SubView = true, IconName = "ic_request_status.png", Title = "Request Status" });
            if (DataInfo.UserInformation.hf_user_auth[0].resp_flag == "Y")
            {
                sideMenuList.Add(new SideMenuList() { MainView = false, MainTitle = "", SubView = true, IconName = "ic_subordinate.png", Title = "Subordinates Leave" });
                sideMenuList.Add(new SideMenuList() { MainView = false, MainTitle = "", SubView = true, IconName = "ic_leave_balances.png", Title = "Subordinates Leave Balance" });
            }

            sideMenuList.Add(new SideMenuList() { MainView = false, MainTitle = "", SubView = true, IconName = "ic_action_logout.png", Title = AppResources.Logout });

            _page = page;  
        }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        public ObservableCollection<SideMenuList> Items
        {
            get
            {
                return sideMenuList;
            }
            set
            {
                SetProperty(ref sideMenuList, value);
            }
        }

        /// <summary>
        /// Gets the selected cell command.
        /// </summary>
        /// <value>
        /// The selected cell command.
        /// </value>
        public Command<SideMenuList> CellSelectedCommand
        {
            get
            {
                return _cellSelectedCommand ?? (_cellSelectedCommand = new Command<SideMenuList>(parameter => Debug.WriteLine(parameter.Title)));
            }
        }

        /// <summary>
        /// Gets or sets the selected item.
        /// </summary>
        /// <value>The selected item.</value>
        public SideMenuList SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                SetProperty(ref _selectedItem, value);
                if (_selectedItem == null)
                    return;
                else
                {
                    PushCommand(_selectedItem.Title);
                    SelectedItem = null;
                }
            }
        }
        /// <summary>
        /// Closing the side menu
        /// </summary>
        private void OnToggleRequest()
        {
            App app = Application.Current as App;
            MasterDetailScreen md = (MasterDetailScreen)app.MainPage;
            md.IsPresented = !md.IsPresented;
        }
        /// <summary>
        /// Pushs the command for navigating the page.
        /// </summary>
        /// <param name="title">Title.</param>
        async void PushCommand(string title)
        {
            App app = Application.Current as App;
            ((NavigationPage)app.MainPage).BarTextColor = Color.White;
            ((NavigationPage)app.MainPage).BarBackgroundColor = Color.FromHex(AppResources.HeaderButtonColor);
            MasterDetailScreen md = (MasterDetailScreen)((NavigationPage)app.MainPage).CurrentPage;
            md.Title = "";
            if (Application.Current.Properties.ContainsKey("Language") && Application.Current.Properties["Language"].ToString().Equals("ar"))
            {
                switch (title)
                {
                    case "Home":
                        md.IsPresented = false;
                        break;
                    case "تطبيق الإجازة":
                        md.IsPresented = false;
                        await _page.Navigation.PushAsync(new LeaveRequestScreen());
                        break;
                    case "تعديل":
                        md.IsPresented = false;
                        await _page.Navigation.PushAsync(new LeaveAmendmentRequestScreen());
                        break;
                    case "استئناف":
                        md.IsPresented = false;
                        await _page.Navigation.PushAsync(new LeaveResumeRequestScreen());
                        break;
                    case "إلغاء":
                        md.IsPresented = false;
                        await _page.Navigation.PushAsync(new LeaveCancellationRequestScreen());
                        break;
                    case "زيارة العمل":
                        md.IsPresented = false;
                        await _page.Navigation.PushAsync(new DutyVisitEmployeeRequestScreen());
                        break;
                    case "تصريح خروج":
                        md.IsPresented = false;
                        await _page.Navigation.PushAsync(new ExitPermitRequestScreen());
                        break;                    
                    case "الخروج":
                        md.IsPresented = false;
                        app.Logout();
                        var fields = typeof(DataInfo).GetFields();
                        foreach (var field in fields)
                        {
                            var type = field.GetType();
                            field.SetValue(null, null);
                        }
                        break;
                }
            }
            else
            {
                switch (title)
                {
                    case "Home":
                        md.IsPresented = false;
                        break;
                    case "Apply Leave":
                        md.IsPresented = false;
                        await _page.Navigation.PushAsync(new LeaveRequestScreen());
                        break;
                    case "Amendment":
                        md.IsPresented = false;
                        await _page.Navigation.PushAsync(new LeaveAmendmentRequestScreen());
                        break;
                    case "Resumption":
                        md.IsPresented = false;
                        await _page.Navigation.PushAsync(new LeaveResumeRequestScreen());
                        break;
                    case "Cancellation":
                        md.IsPresented = false;
                        await _page.Navigation.PushAsync(new LeaveCancellationRequestScreen());
                        break;
                    case "Duty Visit":
                        md.IsPresented = false;
                        await _page.Navigation.PushAsync(new DutyVisitEmployeeRequestScreen());
                        break;
                    case "Exit Permit":
                        md.IsPresented = false;
                        await _page.Navigation.PushAsync(new ExitPermitRequestScreen());
                        break;
                    case "Air Ticket":
                        md.IsPresented = false;
                        await _page.Navigation.PushAsync(new AirTicketRequest());
                        break;
                    case "Request Status":
                        md.IsPresented = false;
                        await _page.Navigation.PushAsync(new LeaveSummaryScreen());
                        break;
                    case "Subordinates Leave":
                        md.IsPresented = false;
                        await _page.Navigation.PushAsync(new FutureLeaveSummaryScreen());
                        break;
                    case "Subordinates Leave Balance":
                        md.IsPresented = false;
                        await _page.Navigation.PushAsync(new FutureLeaveBalanceScreen());
                        break;
                    case "Employee Letter Request":
                        md.IsPresented = false;
                        await _page.Navigation.PushAsync(new EmployeeLetterRequest());
                        break;
                    case "Education Allowance Request":
                        md.IsPresented = false;
                        await _page.Navigation.PushAsync(new EducationAllowanceRequest());
                        break;
                    case "Car Loan Request":
                        md.IsPresented = false;
                        await _page.Navigation.PushAsync(new CarLoanRequest());
                        break;
                    case "Salary Advance Request":
                        md.IsPresented = false;
                        await _page.Navigation.PushAsync(new SalaryAdvanceRequest());
                        break;
                    case "Application Access Request":
                        md.IsPresented = false;
                        await _page.Navigation.PushAsync(new ApplicationAccessRequest());
                        break; 
                    case "Reimbursement Request":
                        md.IsPresented = false;
                        await _page.Navigation.PushAsync(new ClubMembershipFurnitureAllowanceRequest());
                        break;
                    case "Logout":
                        md.IsPresented = false;
                        app.Logout();
                        break;
                }
            }
        }

        public new event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Ons the property changed.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        protected virtual new void OnPropertyChanged(string propertyName)
        {
            var changed = PropertyChanged;
            if (changed != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public class SideMenuList
        {
            public bool MainView { get; set; }
            public string MainTitle { get; set; }
            public bool SubView { get; set; }
            public string IconName { get; set; }
            public string Title { get; set; }
        }

    }
}
