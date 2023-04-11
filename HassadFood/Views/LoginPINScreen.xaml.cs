using System;
using System.Collections.Generic;
using HassadFood.Interface;
using HassadFood.ViewModels;
using Xamarin.Forms;

namespace HassadFood.Views
{
    public partial class LoginPINScreen : ContentPage
    {
        readonly int _limit = 7;
        ILoginManager _loginManager;
        LoginPINModel viewModel;
        public LoginPINScreen(ILoginManager loginManager)
        {
            InitializeComponent();
            _loginManager = loginManager;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            BindingContext = viewModel = new LoginPINModel(this, _loginManager);
            App.NavigationStackCount = Navigation.NavigationStack.Count;
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
    }
}
