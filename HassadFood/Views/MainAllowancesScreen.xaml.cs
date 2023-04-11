using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace HassadFood.Views
{	
	public partial class MainAllowancesScreen : ContentPage
	{	
		public MainAllowancesScreen ()
		{
			InitializeComponent ();
		}
        async void Btn_Clicked(System.Object sender, System.EventArgs e)
        {

            await Navigation.PushAsync(new EducationAllowanceRequest());
        }
        async void Btn_Clicked1(System.Object sender, System.EventArgs e)
        {

            await Navigation.PushAsync(new ClubMembershipFurnitureAllowanceRequest());
        }
        async void Btn_Clicked2(System.Object sender, System.EventArgs e)
        {

            await Navigation.PushAsync(new AirTicketRequest());
        }
        
    }
}

