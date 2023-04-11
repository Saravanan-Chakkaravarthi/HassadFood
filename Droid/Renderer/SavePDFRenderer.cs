using System;
using System.IO;
using System.Net;
using Android.Content;
using Android.Widget;
using HassadFood.Droid.Renderer;
using HassadFood.Interface;

[assembly: Xamarin.Forms.Dependency(typeof(SavePDFRenderer))]
namespace HassadFood.Droid.Renderer
{
    public class SavePDFRenderer : SavePDF
    {
        string SavePDF.Show(byte[] file, string fileName)
        {
            string documentsPath = Path.Combine((Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads)).Path);
            var filePath = Path.Combine(documentsPath, fileName);
            File.WriteAllBytes(filePath, file);
            if (File.Exists(filePath))
            {
                Java.IO.File files = new Java.IO.File(filePath);
                Intent intent = new Intent(Intent.ActionView);
                intent.SetDataAndType(Android.Net.Uri.FromFile(files), "application/pdf");
                intent.SetFlags(ActivityFlags.ClearWhenTaskReset | ActivityFlags.NewTask);
                try
                {
                    Xamarin.Forms.Forms.Context.StartActivity(intent);
                }
                catch (Exception)
                {
                    Toast.MakeText(Xamarin.Forms.Forms.Context, "No Application Available to View PDF", ToastLength.Short).Show();
                }
                return filePath;
            }
            return null;
        }

        void SavePDF.OpenPDF(string filePath)
        {
            if (File.Exists(filePath))
            {
                Java.IO.File files = new Java.IO.File(filePath);
                Intent intent = new Intent(Intent.ActionView);
                intent.SetDataAndType(Android.Net.Uri.FromFile(files), "application/pdf");
                intent.SetFlags(ActivityFlags.ClearWhenTaskReset | ActivityFlags.NewTask);
                try
                {
                    Xamarin.Forms.Forms.Context.StartActivity(intent);
                }
                catch (Exception)
                {
                    Toast.MakeText(Xamarin.Forms.Forms.Context, "No Application Available to View PDF", ToastLength.Short).Show();
                }
            }
        }

        public string SaveImageFile(byte[] file, string fileName)
        {
            string documentsPath = Path.Combine((Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads)).Path);
            var filePath = Path.Combine(documentsPath, fileName);
            File.WriteAllBytes(filePath, file);
            if (File.Exists(filePath))
                return "file://localhost" + filePath;
            return null;
        }

        public void TrustCA()
        {
           ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
        }
    }
}
