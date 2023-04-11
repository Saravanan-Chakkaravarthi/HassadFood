using System.Threading.Tasks;
using HassadFood.Interface;
using HassadFood.Views;
using Xamarin.Forms;

namespace HassadFood
{
    public partial class App : Application, ILoginManager
    {
        public static string AppName { get { return "HassadFood"; } }
        public static string CultureCode { get; set; }
        public App()
        {
            InitializeComponent();

            if (Device.RuntimePlatform != Device.UWP)
            {
                if (Current.Properties.ContainsKey("Language") && Current.Properties["Language"].ToString().Equals("ar"))
                {
                    DependencyService.Get<ILocalize>().ChangeLocale("ar");
                    CultureCode = "ar";
                    Resx.AppResources.Culture = DependencyService.Get<ILocalize>().GetCurrentCultureInfo("ar");
                }
                else
                {                   
                    DependencyService.Get<ILocalize>().SetLocale();                   
                    CultureCode = "en";
                    Resx.AppResources.Culture = DependencyService.Get<ILocalize>().GetCurrentCultureInfo("en");
                }
            }
            ShowLoginPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        public void ShowLoginPage()
        {
            MainPage = new LoginPage(this);
        }

        public void ShowMainPage()
        {
            MainPage = new NavigationPage(new MasterDetailScreen(this)) { BarBackgroundColor = Color.White, BarTextColor = Color.Black };
        }

        public void Logout()
        {
            MainPage = new LoginPage(this);
        }
    }
}
