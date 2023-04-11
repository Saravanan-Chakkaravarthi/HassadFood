using System;
using System.Collections.Generic;
using HassadFood.Interface;
using HassadFood.ViewModels;
using HassadFood.WebService;
using Xamarin.Forms;

namespace HassadFood.Views
{
    public partial class MainPage : ContentPage
    {
        MainModel viewModel;
        ILoginManager _loginManager;
        public MainPage(ILoginManager loginManager)
        {
            InitializeComponent();

            FlowDirection = Resx.AppResources.Culture.Parent.Name.ToString().Equals("ar") ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;

            NavigationPage.SetHasNavigationBar(this, false);
            _loginManager = loginManager;

            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += async (s, e) =>
            {
                if (!side_menu.AnimationIsRunning("ScaleTo"))
                {
                    await side_menu.ScaleTo(0.5, 50, Easing.CubicOut);
                    await side_menu.ScaleTo(1, 50, Easing.CubicIn);
                }
                App app = Application.Current as App;
                ((MasterDetailScreen)((NavigationPage)app.MainPage).CurrentPage).IsPresented = !((MasterDetailScreen)((NavigationPage)app.MainPage).CurrentPage).IsPresented;
            };
            side_menu.GestureRecognizers.Add(tapGestureRecognizer);

  //  var tapGestureRecognizerlogout = new TapGestureRecognizer();
   //     tapGestureRecognizerlogout.Tapped += async (s, e) =>
   //     {
  //          _loginManager.Logout();
   //     };
     //   logout_menu.GestureRecognizers.Add(tapGestureRecognizerlogout);

            var tapGestureRecognizerviewlbl = new TapGestureRecognizer();
           tapGestureRecognizerviewlbl.Tapped += async (s, e) =>
            {
                if (NotificationList.IsVisible)
                {
                    viewlbl.Text = "View More";
                    NotificationList.IsVisible = false;
                }
                else
                {
                    viewlbl.Text = "Hide";
                    NotificationList.IsVisible = true;
                }
               
           };
           viewlbl.GestureRecognizers.Add(tapGestureRecognizerviewlbl);




            NotificationList.ItemTapped += async (sender, e) =>
            {
                var selecteditem = ((ListView)sender).SelectedItem;
                
                var selected_object = (GetWorkList)selecteditem;
                var action_type = selected_object.action;

                switch (action_type)
                {
                    case "R":
                        var position = viewModel.WorkListInformation.get_work_list.IndexOf((GetWorkList)((ListView)sender).SelectedItem);
                        App app = Application.Current as App;
                        ((NavigationPage)app.MainPage).Title = "";
                        ((NavigationPage)app.MainPage).BarTextColor = Color.White;
                        ((NavigationPage)app.MainPage).BarBackgroundColor = Color.FromHex(Resx.AppResources.HeaderButtonColor);
                        await ((NavigationPage)app.MainPage).Navigation.PushAsync(new WorkListDetailScreen(position));
                        ((ListView)sender).SelectedItem = null;
                        break;
                    case "N":
                        var position1 = viewModel.WorkListInformation.get_work_list.IndexOf((GetWorkList)((ListView)sender).SelectedItem);
                        App app1 = Application.Current as App;
                        ((NavigationPage)app1.MainPage).Title = "";
                        ((NavigationPage)app1.MainPage).BarTextColor = Color.White;
                        ((NavigationPage)app1.MainPage).BarBackgroundColor = Color.FromHex(Resx.AppResources.HeaderButtonColor);
                        await ((NavigationPage)app1.MainPage).Navigation.PushAsync(new ApprovalScreen((GetWorkList)selecteditem, viewModel.SelectedType));
                        ((ListView)sender).SelectedItem = null;
                        break;

                }

              
            };
        }

        async void Handle_Clicked(object sender, System.EventArgs e)
        {
            //App app = Application.Current as App;
            //((NavigationPage)app.MainPage).Title = "";
            //((NavigationPage)app.MainPage).BarTextColor = Color.White;
            //((NavigationPage)app.MainPage).BarBackgroundColor = Color.FromHex(Resx.AppResources.HeaderButtonColor);
           // await ((NavigationPage)app.MainPage).Navigation.PushAsync(new WorkListScreen());
        }

        async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
           
            await Navigation.PushAsync(new MainLeaveRequestScreen());
        }
        async void Btn_Clicked(System.Object sender, System.EventArgs e)
        {
            
            await Navigation.PushAsync(new MainAllowancesScreen());
        }
        async void Btn_Click(System.Object sender, System.EventArgs e)
        {
            
            await Navigation.PushAsync(new MainRequestScreen());
        }
        async void Button_Click(System.Object sender, System.EventArgs e)
        {
            
            await Navigation.PushAsync(new DutyVisitEmployeeRequestScreen());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();   
            BindingContext = viewModel = new MainModel(this, _loginManager);
            App.NavigationStackCount = 1;
        }

        void TapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
        {
            if(viewlbl.Text== "View More")
            {
                viewbtn.IsVisible = false;
                //notify_lbl.IsVisible = false;
            }
            else
            {
                viewbtn.IsVisible = true;
            }
            
        }
    }
}
