using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace HassadFood.Views
{	
	public partial class MainLeaveRequestScreen : ContentPage
	{	
		public MainLeaveRequestScreen ()
		{
			InitializeComponent ();
		}

        async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            
            await Navigation.PushAsync(new LeaveRequestScreen());
        }
        async void Button_Clicked1(System.Object sender, System.EventArgs e)
        {
           
            await Navigation.PushAsync(new LeaveAmendmentRequestScreen());
        }
        async void Button_Clicked2(System.Object sender, System.EventArgs e)
        {
            
            await Navigation.PushAsync(new LeaveCancellationRequestScreen());
        }
        async void Button_Clicked3(System.Object sender, System.EventArgs e)
        {
           
            await Navigation.PushAsync(new LeaveResumeRequestScreen());
        }
        async void Button_Clicked4(System.Object sender, System.EventArgs e)
        {
           
            await Navigation.PushAsync(new ExitPermitRequestScreen());
        }
    }
}

