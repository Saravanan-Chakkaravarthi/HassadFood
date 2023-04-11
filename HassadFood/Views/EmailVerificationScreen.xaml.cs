using System;
using System.Collections.Generic;
using HassadFood.Interface;
using HassadFood.ViewModels;
using Xamarin.Forms;

namespace HassadFood.Views
{
    public partial class EmailVerificationScreen : ContentPage
    {
        readonly int _limit = 30;
        ILoginManager _loginManager;
        EmailVerificationModel viewModel;
        public EmailVerificationScreen(ILoginManager loginManager)
        {
            InitializeComponent();
            _loginManager = loginManager;
            VerifyBtn.Clicked += async (sender, e) =>
            {
                await buttonView.ScaleTo(0.95, 50, Easing.CubicOut);
                await buttonView.ScaleTo(1, 50, Easing.CubicIn);
            };
            txtemailID.Completed += (sender, e) =>
            {
                VerifyBtn.Command.Execute(null);
            };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            BindingContext = viewModel = new EmailVerificationModel(this, _loginManager);
            App.NavigationStackCount = Navigation.NavigationStack.Count;
            if (Application.Current.Properties.ContainsKey("E-MAIL"))
            {
                if (Application.Current.Properties["E-MAIL"] != null && Application.Current.Properties["E-MAIL"].ToString().Trim() != "")
                    txtemailID.Text = Application.Current.Properties["E-MAIL"] as string;
            }
        }

        /// <summary>
        /// Limits the entry.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void LimitEntry(object sender, TextChangedEventArgs e)
        {
            string _text = ((Xamarin.Forms.Entry)sender).Text;      //Get Current Text
            if (_text != null && _text.Length > _limit)       //If it is more than your character restriction
            {
                _text = _text.Remove(_text.Length - 1);  // Remove Last character
                ((Xamarin.Forms.Entry)sender).Text = _text;        //Set the Old value
            }
        }

        /// <summary>
        /// Ons the content view size changed.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="args">Arguments.</param>
        void OnContentViewSizeChanged(object sender, EventArgs args)
        {
            ContentView contentView = (ContentView)sender;
            double boardSize = contentView.Height;
            buttonView.WidthRequest = contentView.Width;

            txtemailID.WidthRequest = contentView.Width - 70.0;           
        }
    }
}
