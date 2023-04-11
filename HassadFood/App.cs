using System;
using System.Threading.Tasks;
using HassadFood.Interface;
using HassadFood.Views;
using HassadFood.WebService;
using Xamarin.Forms;

namespace HassadFood
{
    public partial class App : Application, ILoginManager
    {
        public static string AppName { get { return "HassadFood"; } }
        public static string CultureCode { get; set; }
        public static int NavigationStackCount { get; set; }
        DateTime timer;
        public App()
        {
            InitializeComponent();

            NavigationStackCount = 1;
            if (Device.RuntimePlatform != Device.UWP)
            {
                //if (Current.Properties.ContainsKey("Language") && Current.Properties["Language"].ToString().Equals("ar"))
                //{
                //    DependencyService.Get<ILocalize>().ChangeLocale("ar");
                //    CultureCode = "ar";
                //    Resx.AppResources.Culture = DependencyService.Get<ILocalize>().GetCurrentCultureInfo("ar");
                //    Application.Current.Properties["Language"] = "ar";
                //}
                //else
                //{
                    DependencyService.Get<ILocalize>().ChangeLocale("en");
                    CultureCode = "en";
                    Resx.AppResources.Culture = DependencyService.Get<ILocalize>().GetCurrentCultureInfo("en");
                    Application.Current.Properties["Language"] = "en";
                //}
                //HPUTHIYOTTIL
            }
            if (Current.Properties.ContainsKey("E-MAIL"))
            {
                if (Current.Properties["E-MAIL"] == null)
                    ShowEMailPage();
                
                else if (Current.Properties.ContainsKey("PIN_VERIFIED"))
                {
                    if (Current.Properties["PIN_VERIFIED"] as string == "false")
                        ShowPINPage();

                        
                    else
                        ShowLoginPage();
                }
                else
                    ShowPINPage();
            }
            else
                //ShowEMailPage();
            ShowLoginPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            timer = DateTime.Now;
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
            if (DateTime.Now > timer.AddMinutes(3))
                Logout();
        }

        public void ShowLoginPage()
        {
            MainPage = new LoginPage(this);
        }

        public void ShowMainPage()
        {
            MainPage = new NavigationPage(new MasterDetailScreen(this)) { BarBackgroundColor = Color.White, BarTextColor = Color.Black };
        }

        public void ShowEMailPage()
        {
            MainPage = new EmailVerificationScreen(this);
        }

        public void ShowPINPage()
        {
            MainPage = new LoginPINScreen(this);
        }
        public void Logout()
        {
            DependencyService.Get<IUserDetails>().UserLogout();
            DataInfo.Clear();
            MainPage = new LoginPage(this);
        }
    }
}
