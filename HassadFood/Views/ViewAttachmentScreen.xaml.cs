using System;
using System.Collections.Generic;
using System.Diagnostics;
using HassadFood.Interface;
using Xamarin.Forms;

namespace HassadFood.Views
{
    public partial class ViewAttachmentScreen : ContentPage
    {
        string tempPath = null;
        public ViewAttachmentScreen(string transact_id, string file_id, string filetype)
        {
            InitializeComponent();
            if (Device.RuntimePlatform == Device.Android)
            {
                var is_storagecheck = DependencyService.Get<IDeviceAccess>().GetStoragePermissionAsync();
                if (is_storagecheck == false)
                {
                    PermissionCheck();
                }
                else
                {
                    LoadAttachments(transact_id, file_id, filetype);
                }
            }
            else
            {
                LoadAttachments(transact_id, file_id, filetype);
            }
            OpenBtn.Clicked += (sender, e) =>
            {
                if (tempPath != null)
                    DependencyService.Get<SavePDF>().OpenPDF(tempPath);
            };
        }

        async void PermissionCheck()
        {
            var confirmed = await DisplayAlert("Alert", "Please enable storage permission to view attachment.", "Ok", "Cancel");
            if (confirmed)
            {
                DependencyService.Get<IDeviceAccess>().OpenDeviceSettings();
                await Navigation.PopAsync();
            }
            else
            {
                HtmlWebViewSource source = new HtmlWebViewSource
                {
                    Html = "<html><body>Enable storage permission for the app to view files.</body></html>",
                };
                WebVwAttachments.Source = source;
            }
        }

        async void LoadAttachments(string transact_id, string file_id, string filetype)
        {
            try
            {
                ActivityLayout.IsVisible = true;
                ActivityLoader.IsVisible = true;
                ActivityLoader.IsRunning = true;
                var result = await DependencyService.Get<IUserDetails>().DownloadAttachmentsFile(transact_id, file_id);
                if (filetype.Contains("image"))
                {
                    var path = DependencyService.Get<SavePDF>().SaveImageFile(result, App.AppName + "_Test.jpg");
                    if (path != null)
                        WebVwAttachments.Source = path; //setting the web view source
                    ActivityLayout.IsVisible = false;
                    ActivityLoader.IsVisible = false;
                    ActivityLoader.IsRunning = false;
                }
                else
                {
                    var path = DependencyService.Get<SavePDF>().Show(result, App.AppName + "_Temp.pdf");
                    if (Device.RuntimePlatform == Device.iOS)
                    {
                        WebVwAttachments.Source = path;
                    }
                    else if (Device.RuntimePlatform == Device.Android)
                    {
                        DowloadView.IsVisible = true;
                        OpenBtn.IsVisible = true;
                        OpenBtn.IsEnabled = true;
                        tempPath = path;
                        DownloadPath.Text = "Attachments downloaded:\n" + path;
                    }
                    ActivityLayout.IsVisible = false;
                    ActivityLoader.IsVisible = false;
                    ActivityLoader.IsRunning = false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                ActivityLayout.IsVisible = false;
                ActivityLoader.IsVisible = false;
                ActivityLoader.IsRunning = false;
                await DisplayAlert("Alert", "Files not found.", "OK");
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.NavigationStackCount = Navigation.NavigationStack.Count;
        }
    }
}
