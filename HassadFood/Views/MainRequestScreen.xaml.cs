using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace HassadFood.Views
{	
	public partial class MainRequestScreen : ContentPage
	{	
		public MainRequestScreen ()
		{
			InitializeComponent ();
		}
        async void Btn_Click(System.Object sender, System.EventArgs e)
        {

            await Navigation.PushAsync(new EmployeeLetterRequest());
        }
        async void Btn_Click1(System.Object sender, System.EventArgs e)
        {

            await Navigation.PushAsync(new SalaryAdvanceRequest());
        }
        async void Btn_Click2(System.Object sender, System.EventArgs e)
        {

            await Navigation.PushAsync(new CarLoanRequest());
        }
        async void Btn_Click3(System.Object sender, System.EventArgs e)
        {

            await Navigation.PushAsync(new ApplicationAccessRequest());
        }
        
        
    }
}

