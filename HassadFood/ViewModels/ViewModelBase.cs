using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using HassadFood.Interface;
using HassadFood.MVVMHelper;
using HassadFood.WebService;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace HassadFood.ViewModels
{
	public abstract class ViewModelBase : BaseViewModel
	{
		protected Page page;
        List<string> YesNo;
        public List<MemoryStream> AttachmentsFiles;
        List<int> Hours, Minutes;
        int _selectedHoursItem, _selectedMinutesItem;
        DateTime _selectedStartDateItem, _selectedEndDateItem;
        IUserDetails userDetails;
		public ViewModelBase(Page page)
		{
			this.page = page;
            userDetails = DependencyService.Get<IUserDetails>();
            YesNo = new List<string>();
            YesNo.Add("No");
            YesNo.Add("Yes");
            AttachmentsFiles = new List<MemoryStream>();
            AddAttachmentsImage = "ic_check_box_outline_blank.png";
            Hours = new List<int>();
            Minutes = new List<int>();
            for (int i = 0; i < 24; i++)
                Hours.Add(i);
            for (int i = 0; i < 60; i++)
                Minutes.Add(i);
            SelectedStartDateItem = DateTime.Now;
            SelectedEndDateItem = DateTime.Now;
            CalculatedLeaveDuration = "0";
		}

        public List<string> YesNoItem
        {
            get { return YesNo; }
            set { SetProperty(ref YesNo, value); }
        }

        public DateTime SelectedStartDateItem
        {
            get { return _selectedStartDateItem; }
            set { SetProperty(ref _selectedStartDateItem, value); }
        }

        bool note = false;

        public bool Note
        {
            get { return note; }
            set { SetProperty(ref note, value); }
        }

        public DateTime SelectedEndDateItem
        {
            get { return _selectedEndDateItem; }
            set { SetProperty(ref _selectedEndDateItem, value); }
        }

        public List<int> ItemsHours
        {
            get { return Hours; }
            set { SetProperty(ref Hours, value); }
        }

        public int SelectedHoursItem
        {
            get { return _selectedHoursItem; }
            set { SetProperty(ref _selectedHoursItem, value); }
        }

        public List<int> ItemsMinutes
        {
            get { return Minutes; }
            set { SetProperty(ref Minutes, value); }
        }

        public int SelectedMinutesItem
        {
            get { return _selectedMinutesItem; }
            set { SetProperty(ref _selectedMinutesItem, value); }
        }

        string attachments1 = string.Empty;

        public string Attachments1
        {
            get { return attachments1; }
            set { SetProperty(ref attachments1, value); }
        }

        string attachments2 = string.Empty;

        public string Attachments2
        {
            get { return attachments2; }
            set { SetProperty(ref attachments2, value); }
        }

        string attachments3 = string.Empty;

        public string Attachments3
        {
            get { return attachments3; }
            set { SetProperty(ref attachments3, value); }
        }

        bool attachments1Visible = false;

        public bool Attachments1Visible
        {
            get { return attachments1Visible; }
            set { SetProperty(ref attachments1Visible, value); }
        }

        bool attachments2Visible = false;

        public bool Attachments2Visible
        {
            get { return attachments2Visible; }
            set { SetProperty(ref attachments2Visible, value); }
        }

        bool attachments3Visible = false;

        public bool Attachments3Visible
        {
            get { return attachments3Visible; }
            set { SetProperty(ref attachments3Visible, value); }
        }

        bool addAttachments = false;

        public bool AddAttachments
        {
            get { return addAttachments; }
            set { SetProperty(ref addAttachments, value); }
        }

        string addAttachmentsImage = string.Empty;

        public string AddAttachmentsImage
        {
            get { return addAttachmentsImage; }
            set { SetProperty(ref addAttachmentsImage, value); }
        }

        string calculatedLeaveDuration;

        public string CalculatedLeaveDuration
        {
            get { return calculatedLeaveDuration; }
            set { SetProperty(ref calculatedLeaveDuration, value); }
        }

     string comments;

      public string Comments
       {
          get { return comments; }
           set { if (comments != value) { comments = value; OnPropertyChanged("Comments"); } }
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

        Command _CameraCommand;

        public Command CameraCommand => _CameraCommand ?? (_CameraCommand = new Command(() => PickImageFromCamera()));

        Command _GalleryCommand;

        public Command GalleryCommand => _GalleryCommand ?? (_GalleryCommand = new Command(() => PickImageFromGallery()));

        Command _Delete1Command;

        public Command Delete1Command => _Delete1Command ?? (_Delete1Command = new Command(() => Delete1Attachments()));

        Command _Delete2Command;

        public Command Delete2Command => _Delete2Command ?? (_Delete2Command = new Command(() => Delete2Attachments()));

        Command _Delete3Command;

        public Command Delete3Command => _Delete3Command ?? (_Delete3Command = new Command(() => Delete3Attachments()));

        Command _AddAttachmentsCommand;

        public Command AddAttachmentsCommand => _AddAttachmentsCommand ?? (_AddAttachmentsCommand = new Command(() => ExecuteAddAttachmentsCommand()));

        Command _CalculateDurationCommand;

        public Command CalculateDurationCommand => _CalculateDurationCommand ?? (_CalculateDurationCommand = new Command(() => ExecuteCalculateDurationCommand()));

        Command _NoteCommand;

        public Command NoteCommand => _NoteCommand ?? (_NoteCommand = new Command(() => ExecuteNoteCommand()));

        void ExecuteNoteCommand()
        {
            Note = !Note;
        }

        async void ExecuteCalculateDurationCommand()
        {
            //if (SelectedStartDateItem < SelectedEndDateItem)
            //{
            //    await page.DisplayAlert("Alert", "Error from date not should be less or greater than to date.", "OK");
            //    CalculatedLeaveDuration = "0";
            //    return;
            //}

            //Debug.WriteLine(SelectedStartDateItem.ToString() + SelectedEndDateItem.ToString() + SelectedHoursItem + SelectedMinutesItem);
            string showAlert = null;

            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                var result = await userDetails.CalculateAbsenceDuration("", SelectedStartDateItem, SelectedEndDateItem);
                if (result)
                {
                    CalculateAbsenceDurationInformation = DataInfo.CalculateAbsenceDurationInformation;
                    if (CalculateAbsenceDurationInformation != null)
                    {
                        CalculatedLeaveDuration = CalculateAbsenceDurationInformation.p_days.ToString();
                        showAlert = "Success";
                        if (CalculateAbsenceDurationInformation.p_days == 0)
                            await page.DisplayAlert("Alert", "You have applied for zero days of leave.", "OK");
                    }
                    else
                        showAlert = "Failed";
                }
                else
                    showAlert = "Error";
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                showAlert = "Error";
            }
            finally
            {
                IsBusy = false;

                switch (showAlert)
                {
                    case "Error":
                        await page.DisplayAlert("Alert", "Unable to calculate days.", "OK");
                        break;
                    case "Success":
                        break;
                    case "Failed":
                        await page.DisplayAlert("Alert", "Unable to calculate days.", "OK");
                        break;
                }
            }
        }

        void ExecuteAddAttachmentsCommand()
        {
            if (AddAttachments)
            {
                AddAttachments = false;
                AddAttachmentsImage = "ic_check_box_outline_blank.png";
            }
            else
            {
                AddAttachments = true;
                AddAttachmentsImage = "ic_check_box.png";
            }
        }

        void Delete1Attachments()
        {
            Attachments1Visible = false;
            AttachmentsFiles.RemoveAt(0);
            Attachments1 = null;
            if (AttachmentsFiles.Count > 0)
            {
                for (int i = 0; i < AttachmentsFiles.Count; i++)
                {
                    switch (i)
                    {
                        case 0:
                            Attachments1 = Attachments2;
                            Attachments1Visible = true;
                            Attachments2Visible = false;
                            Attachments2 = null;
                            break;
                        case 1:
                            Attachments2 = Attachments3;
                            Attachments2Visible = true;
                            Attachments3Visible = false;
                            Attachments3 = null;
                            break;
                    }
                }
            }
        }

        void Delete2Attachments()
        {
            Attachments2Visible = false;
            AttachmentsFiles.RemoveAt(1);
            Attachments2 = null;
            if (AttachmentsFiles.Count > 1)
            {
                Attachments2 = Attachments3;
                Attachments2Visible = true;
                Attachments3Visible = false;
                Attachments3 = null;
            }
        }

        void Delete3Attachments()
        {
            Attachments3Visible = false;
            AttachmentsFiles.RemoveAt(2);
            Attachments3 = null;
        }

        async void PickImageFromGallery()
        {
            try
            {
                if (Device.RuntimePlatform == Device.Android)
                {
                    var b = DependencyService.Get<IDeviceAccess>().GetStoragePermissionAsync();
                    if (b == false)
                    {
                        await page.DisplayAlert("Alert", "Enable storage permission to add attachment.", "OK");
                        DependencyService.Get<IDeviceAccess>().OpenDeviceSettings();
                        return;
                    }
                }

                if (AttachmentsFiles.Count <= 2)
                {
                    string pathOfImage = null;
                    bool FileIsNotExists = false;

                    if (CrossMedia.Current.IsPickPhotoSupported)
                    {
                        MemoryStream stream = new MemoryStream();
                        await CrossMedia.Current.PickPhotoAsync().ContinueWith(t =>
                        {
                            if (t.IsCompleted)
                            {
                                if (t.Result != null)
                                {
                                    MediaFile filex = t.Result;
                                    t.Result.GetStream().CopyTo(stream);
                                    pathOfImage = filex.Path;
                                }
                                else
                                    FileIsNotExists = true;

                            }
                        }, TaskScheduler.FromCurrentSynchronizationContext());

                        if (!FileIsNotExists)
                        {
                            AttachmentsFiles.Add(stream);
                            switch (AttachmentsFiles.Count)
                            {
                                case 1:
                                    Attachments1 = Path.GetFileName(pathOfImage);
                                    Attachments1Visible = true;
                                    break;
                                case 2:
                                    Attachments2 = Path.GetFileName(pathOfImage);
                                    Attachments2Visible = true;
                                    break;
                                case 3:
                                    Attachments3 = Path.GetFileName(pathOfImage);
                                    Attachments3Visible = true;
                                    break;
                            }
                        }
                    }
                }
                else
                    await page.DisplayAlert("Alert", "Attachments should not be more than 3 files.", "OK");

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }

        async void PickImageFromCamera()
        {
            try
            {
                if (Device.RuntimePlatform == Device.Android)
                {
                    var b = DependencyService.Get<IDeviceAccess>().GetCameraPermissionAsync();
                    if (b == false)
                    {
                        await page.DisplayAlert("Alert", "Enable camera permission to add attachment.", "OK");
                        DependencyService.Get<IDeviceAccess>().OpenDeviceSettings();
                        return;
                    }
                }

                if (AttachmentsFiles.Count <= 2)
                {
                    string pathOfImage = null;
                    bool FileIsNotExists = false;

                    if (CrossMedia.Current.IsTakePhotoSupported)
                    {
                        MemoryStream stream = new MemoryStream();
                        await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions() { SaveToAlbum = true }).ContinueWith(t =>
                        {
                         if (t.IsCompleted)
                         {
                             if (t.Result != null)
                             {
                                 MediaFile filex = t.Result;
                                 t.Result.GetStream().CopyTo(stream);
                                 pathOfImage = filex.Path;
                             }
                             else
                                 FileIsNotExists = true;

                         }
                        }, TaskScheduler.FromCurrentSynchronizationContext());

                        if (!FileIsNotExists)
                        {
                            AttachmentsFiles.Add(stream);
                            switch (AttachmentsFiles.Count)
                            {
                                case 1:
                                    Attachments1 = Path.GetFileName(pathOfImage);
                                    Attachments1Visible = true;
                                    break;
                                case 2:
                                    Attachments2 = Path.GetFileName(pathOfImage);
                                    Attachments2Visible = true;
                                    break;
                                case 3:
                                    Attachments3 = Path.GetFileName(pathOfImage);
                                    Attachments3Visible = true;
                                    break;
                            }
                        }
                    }
                }
                else
                    await page.DisplayAlert("Alert", "Attachments should not be more than 3 files.", "OK");

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }

        public async Task<bool> PutAttachmentsRequest()
        {
            if (AttachmentsFiles.Count > 0)
            {
                for (int i = 0; i < AttachmentsFiles.Count; i++)
                {
                    string fileName = null;
                    switch (i)
                    {
                        case 0:
                            fileName = Attachments1;
                            break;
                        case 1:
                            fileName = Attachments2;
                            break;
                        case 2:
                            fileName = Attachments3;
                            break;
                    }
                    try
                    {
                        var resultAttach = await userDetails.PutAttachmentsRequest(AttachmentsFiles[i].ToArray(), fileName, DataInfo.RequestInformation.p_trx_id.ToString());
                        if (resultAttach)
                        {
                            RequestInformation = DataInfo.AttachmentsRequestInformation;
                            if (RequestInformation != null && RequestInformation.sshr.Count> 0)
                                if(RequestInformation.sshr[0].p_success_flag != "Y")
                                    Debug.WriteLine(RequestInformation.p_error_msg);
                        }
                    }
                    catch(Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }
            }
            return true;
        }

	}
}
