using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using HassadFood.Interface;
using HassadFood.WebService;
using Xamarin.Forms;

namespace HassadFood.ViewModels
{
    public class EmailVerificationModel : ViewModelBase
    {
        Page _page;
        ILoginManager _loginManager;
        string email;
        IUserDetails userDetails;
        public EmailVerificationModel(Page page, ILoginManager loginManager) : base(page)
        {
            Title = "Login";
            userDetails = DependencyService.Get<IUserDetails>();
            _page = page;
            _loginManager = loginManager;
            if (Application.Current.Properties.ContainsKey("E-MAIL"))
                EMail = Application.Current.Properties["E-MAIL"] as string;
        }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        public string EMail
        {
            get
            {
                return email;
            }
            set
            {
                if (email != value)
                {
                    email = value;
                    OnPropertyChanged("EMail");
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

        Command _VerifyCommand;

        public Command VerifyCommand => _VerifyCommand ?? (_VerifyCommand = new Command(() => ExecuteVerifyCommand()));

        //Verify the user entered E-Mail
        async void ExecuteVerifyCommand()
        {
            string showAlert = null;
            string message = null;
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrEmpty(email))
            {
                await page.DisplayAlert("Alert", "E-Mail cannot be empty.", "OK");
                return;
            }

            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(email.Trim());

            if (!match.Success)
            {
                await page.DisplayAlert("Alert", email + " is Invalid Email Address", "OK");
                return;
            }

            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                var res = await userDetails.EMailVerification(email.Trim());
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
                        Application.Current.Properties["E-MAIL"] = EMail.Trim();
                        await Application.Current.SavePropertiesAsync();
                        await page.DisplayAlert("Alert", "Verification PIN sent to " + email.Trim() + ".", "OK");
                        _loginManager.ShowPINPage();
                        break;
                }
            }
        }

    }
}
