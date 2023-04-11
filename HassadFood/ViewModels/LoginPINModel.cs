using System;
using System.ComponentModel;
using System.Diagnostics;
using HassadFood.Interface;
using HassadFood.WebService;
using Xamarin.Forms;

namespace HassadFood.ViewModels
{
    public class LoginPINModel : ViewModelBase
    {
        Page _page;
        ILoginManager _loginManager;
        string pin;
        IUserDetails userDetails;
        public LoginPINModel(Page page, ILoginManager loginManager) : base(page)
        {
            Title = "Login";
            userDetails = DependencyService.Get<IUserDetails>();
            _page = page;
            _loginManager = loginManager;
        }

        public string PIN
        {
            get
            {
                return pin;
            }
            set
            {
                if (pin != value)
                {
                    pin = value;
                    OnPropertyChanged("PIN");
                }
            }
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

        Command _ResendPINCommand;

        public Command ResendPINCommand => _ResendPINCommand ?? (_ResendPINCommand = new Command(() => ExecuteResendCommand()));

        //Verify the user entered E-Mail
        async void ExecuteResendCommand()
        {
            //_loginManager.ShowEMailPage();
            string showAlert = null;
            string message = null;

            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                var res = await userDetails.ResendVerficationPIN();
                if (res)
                {
                    if (DataInfo.EmailPINVerifyInformation.p_success_flag == "Y")
                        showAlert = "Sent";
                    else
                    {
                        showAlert = "Error";
                        message = DataInfo.EmailPINVerifyInformation.p_record;
                    }
                }
                else
                    showAlert = "Error";
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                message = "Unable to send.";
                showAlert = "Error";
            }
            finally
            {
                IsBusy = false;
                switch (showAlert)
                {
                    case "Error":
                        await page.DisplayAlert("Alert", message, "OK");
                        break;
                    case "Sent":
                        await page.DisplayAlert("Alert", "Sent Successfully.", "OK");
                        break;
                }
            }
        }

        Command _VerifyCommand;

        public Command VerifyCommand => _VerifyCommand ?? (_VerifyCommand = new Command(() => ExecuteVerifyCommand()));

        //Verify the user entered E-Mail
        async void ExecuteVerifyCommand()
        {
            string showAlert = null;
            string message = null;
            if (string.IsNullOrWhiteSpace(PIN) || string.IsNullOrEmpty(PIN))
            {
                await page.DisplayAlert("Alert", "PIN cannot be empty", "OK");
                return;
            }

            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                var res = await userDetails.PINVerification(pin);
                if (res)
                {
                    if (DataInfo.EmailPINVerifyInformation.p_success_flag == "Y")
                        showAlert = "Verified";
                    else
                    {
                        showAlert = "Error";
                        message = DataInfo.EmailPINVerifyInformation.p_record;
                    }
                }
                else
                    showAlert = "Error";
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                message = "Unable to verify";
                showAlert = "Error";
            }
            finally
            {
                IsBusy = false;
                switch (showAlert)
                {
                    case "Error":
                        await page.DisplayAlert("Alert", message, "OK");
                        break;
                    case "Verified":
                        Application.Current.Properties["PIN_VERIFIED"] = "true";
                        await Application.Current.SavePropertiesAsync();
                        _loginManager.ShowMainPage();
                        break;
                }
            }
        }

    }
}
