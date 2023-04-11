using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using HassadFood.Interface;
using HassadFood.Views;
using HassadFood.WebService;
using Xamarin.Forms;

namespace HassadFood.ViewModels
{
    public class LoginModel: ViewModelBase
    {
        Page _page;
        ILoginManager _loginmanager;
        string username,password;
        IUserDetails userDetails;
        ITouchID TouchID;
        public LoginModel(Page page, ILoginManager loginManager) : base(page)
            {
            Title = "Login Page";
            userDetails = DependencyService.Get<IUserDetails>();
            TouchID = DependencyService.Get<ITouchID>();
            _page = page;
            _loginmanager = loginManager;
            if (Application.Current.Properties.ContainsKey("Language") && Application.Current.Properties["Language"].ToString().Equals("ar"))
            {
                AraLabelColor = "#3372A8";
                EngLabelColor = "#0C335A";
            }
            else
            {
                AraLabelColor = "#0C335A";
                EngLabelColor = "#3372A8";
            }
            PopulateUserName();
        }
        public string Username
        {
            get
            {
                return username;
            }
            set
            {
                if (username != value)
                {
                    username = value;
                    OnPropertyChanged("Username");
                }
            }
        }

        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                if (password != value)
                {
                    password = value;
                    OnPropertyChanged("Password");
                }
            }
        }

        Command _LoginCommand, _AraCommand, _EngCommand, _FingerPrintCommand;

        public Command LoginCommand => _LoginCommand ?? (_LoginCommand = new Command(() => ExecuteLoginCommand()));

        public Command EngCommand => _EngCommand ?? (_EngCommand = new Command(() => ExecuteEngCommand()));

        public Command AraCommand => _AraCommand ?? (_AraCommand = new Command(() => ExecuteAraCommand()));

        public Command FingerPrintCommand => _FingerPrintCommand ?? (_FingerPrintCommand = new Command(() => ExecuteFingerPrintCommand()));

        public new event PropertyChangedEventHandler PropertyChanged;

        protected virtual new void OnPropertyChanged(string propertyName)
        {
            var changed = PropertyChanged;
            if (changed != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        async void ExecuteEngCommand()
        {           
            DependencyService.Get<ILocalize>().SetLocale();
            App.CultureCode = "en";
            Resx.AppResources.Culture = DependencyService.Get<ILocalize>().GetCurrentCultureInfo("en");
            Application.Current.Properties["Language"] = "en";
            await Application.Current.SavePropertiesAsync();
            _loginmanager.ShowLoginPage();
        }
       
        async void ExecuteAraCommand()
        {
            DependencyService.Get<ILocalize>().ChangeLocale("ar");
            App.CultureCode = "ar";
            Resx.AppResources.Culture = DependencyService.Get<ILocalize>().GetCurrentCultureInfo("ar");
            Application.Current.Properties["Language"] = "ar";
            await Application.Current.SavePropertiesAsync();
            _loginmanager.ShowLoginPage();
        }

        //Checking whether the device having fingerprint sensor and username for the existing user
        async void PopulateUserName()
        {
            if (Application.Current.Properties.ContainsKey("OracleUserName"))
            {
                try
                {
                    string oracle_username = "";
                    oracle_username = Application.Current.Properties["OracleUserName"] as string;
                    if (!oracle_username.Equals(""))
                    {
                        username = oracle_username;
                        if (TouchID.Permissions())
                        {
                            if (await TouchID.IsTouchIDSupport())
                                IsListBusy = true;
                            else
                                IsListBusy = false;
                        }
                        else
                            Debug.WriteLine("Alert", "Fingerprint permission is disabled.", "OK");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }

        async void ExecuteFingerPrintCommand()
        {
            var resultTouchID = await TouchID.Login();
            IStore store = DependencyService.Get<IStore>();
            switch (resultTouchID)
            {
                case "Success":
                    string showAlert = null;

                    if (IsBusy)
                        return;

                    IsBusy = true;

                    try
                    {
                        Username = Username.ToLower();

                        var result = await userDetails.Authenticate(Application.Current.Properties["OracleUserName"] as string, store.GetOraclePassword().Result);
                        if (result)
                        {
                            UserInformation = DataInfo.UserInformation;
                            if (UserInformation != null && UserInformation.hf_user_auth[0].user_id != -1)
                            {
                                Application.Current.Properties["OracleUserName"] = Username.ToUpper();
                                //Application.Current.Properties["USER_ID"] = UserInformation.hf_user_auth[0].user_id.ToString();
                                //Application.Current.Properties["PERSON_ID"] = UserInformation.hf_user_auth[0].person_id.ToString();
                                //Application.Current.Properties["BUSINESS_GROUP_ID"] = UserInformation.hf_user_auth[0].business_group_id.ToString();
                                //Application.Current.Properties["RESP_FLAG"] = UserInformation.hf_user_auth[0].resp_flag;
                                if (UserInformation.hf_user_auth[0].isverified == "Y")
								{
                                    showAlert = "LoggedIn";

                                    await PushNotification();
								}
                                else
                                    showAlert = "LoggedInWithVerificationError";
                            }
                            else
                                showAlert = "LoggedInWithError";
                        }
                        else
                            showAlert = "Error";
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                        //tempError = ex.Message;
                        showAlert = "Error";
                    }
                    finally
                    {
                        IsBusy = false;

                        switch (showAlert)
                        {
                            case "Error":
                                store.DeleteOracleCredentials();
                                await _page.DisplayAlert("Alert", "Unable to login.", "OK");
                                break;
                            case "LoggedIn":
                                store.StoreOracleCredentials(username.ToUpper(), password);
                                await Application.Current.SavePropertiesAsync();
                                username = "";
                                password = "";
                                _loginmanager.ShowMainPage();
                                break;
                            case "LoggedInWithError":
                                store.DeleteOracleCredentials();
                                await _page.DisplayAlert("Alert", "Invalid Credentials.", "OK");
                                break;
                            case "LoggedInWithVerificationError":
                                store.StoreOracleCredentials(username.ToUpper(), password);
                                await Application.Current.SavePropertiesAsync();
                                username = "";
                                password = "";
                                _loginmanager.ShowPINPage();
                                break;
                        }
                    }
                    break;
                case "Failed":
                    break;
                case "Enroll Fingerprint":
                    await page.DisplayAlert("Alert", "Please enroll fingerprint to login.", "OK");
                    break;
                case "Not Supported":
                    break;
            }
        }

		async Task<bool> PushNotification()
		{
            var token = DependencyService.Get<INotification>().RegisterNotification();
            if (token != null)
            {
                //call web service
                PushNotificationRequest notification = new PushNotificationRequest();
                notification.p_user_id = UserInformation.hf_user_auth[0].user_id;
                notification.p_user_name = Username.ToUpper();
                notification.p_device_id = DependencyService.Get<INotification>().DeviceID();
                notification.p_reg_token = token;
                notification.p_platform = Device.RuntimePlatform.ToString();
                notification.p_isactive = "yes";
                notification.p_creation_date = DateTime.Now.ToString(format: "dd-MMM-yyyy");
                notification.p_created_by = "admin";
                notification.p_last_updated_date = DateTime.Now.ToString(format: "dd-MMM-yyyy");
                notification.p_last_updated_by = "admin";

                var resToken = await userDetails.AddDevice(notification);
                if (resToken)
                {
                    if (DataInfo.AddDeviceInformation.p_success_flag == "Y")
                        Application.Current.Properties["Notification"] = "true";
                    else
                    {
                        Debug.WriteLine("Error registering Notification.");
                        Application.Current.Properties["Notification"] = "false";
                    }
                }
            }
            return true;

		}

        async void ExecuteLoginCommand()
        {


        

            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrEmpty(Username))
            {
                await page.DisplayAlert(Resx.AppResources.AlertText, Resx.AppResources.UsernameErrorMessage, Resx.AppResources.OkText);
                return;
            }

            if (string.IsNullOrWhiteSpace(Password) || string.IsNullOrEmpty(Password))
            {
                await page.DisplayAlert(Resx.AppResources.AlertText, Resx.AppResources.PasswordErrorMessage, Resx.AppResources.OkText);
                return;
            }

            string showAlert = null;

            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Username = Username.ToLower();

                var result = await userDetails.Authenticate(Username, Password);
                if (result)
                {
                    UserInformation = DataInfo.UserInformation;
                    if (UserInformation != null && UserInformation.hf_user_auth[0].user_id != -1)
                    {
                        Application.Current.Properties["OracleUserName"] = Username.ToUpper();

                    
                           if (UserInformation.hf_user_auth[0].isverified == "N")
                            {
                                showAlert = "LoggedIn";

							await PushNotification();
                        }
                        else
                            showAlert = "LoggedInWithVerificationError";
                    }
                    else
                        showAlert = "LoggedInWithError";
                }
                else
                    showAlert = "Error";
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                //tempError = ex.Message;
                showAlert = "Error";
            }

          
            finally
            {
                IsBusy = false;
                IStore store = DependencyService.Get<IStore>();
                switch (showAlert)
                {
                    case "Error":
                        store.DeleteOracleCredentials();
                        await _page.DisplayAlert("Alert", "Unable to login.", "OK");
                       
                       
                        break;
                    case "LoggedIn":
                        store.StoreOracleCredentials(username.ToUpper(), password);
                        await Application.Current.SavePropertiesAsync();
                        username = "";
                        password = "";
                        _loginmanager.ShowMainPage();
                        break;
                    case "LoggedInWithError":
                        store.DeleteOracleCredentials();
                        await _page.DisplayAlert("Alert", "Invalid Credentials.", "OK");
                        break;
                    case "LoggedInWithVerificationError":
                        store.StoreOracleCredentials(username.ToUpper(), password);
                        await Application.Current.SavePropertiesAsync();
                        username = "";
                        password = "";
                        //_loginmanager.ShowPINPage();
                        _loginmanager.ShowMainPage();
                        break;
                }
            }
        }
    }
}
