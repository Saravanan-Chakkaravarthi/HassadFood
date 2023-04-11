using System;
using System.IO;
using System.Net;
using HassadFood.Interface;
using HassadFood.iOS.Renderer;

[assembly: Xamarin.Forms.Dependency(typeof(SavePDFRenderer))]
namespace HassadFood.iOS.Renderer
{
    public class SavePDFRenderer : SavePDF
    {
        /// <summary>
        /// Opens the pdf.
        /// </summary>
        /// <param name="filePath">File path.</param>
        public void OpenPDF(string filePath)
        {
        }

        public string SaveImageFile(byte[] file, string fileName)
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var filePath = Path.Combine(documentsPath, fileName);
            File.WriteAllBytes(filePath, file);
            return filePath;
        }

        /// <summary>
        /// Show the specified file and fileName.
        /// </summary>
        /// <returns>The show.</returns>
        /// <param name="file">File.</param>
        /// <param name="fileName">File name.</param>
        public string Show(byte[] file, string fileName)
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var filePath = Path.Combine(documentsPath, fileName);
            File.WriteAllBytes(filePath, file);
            return filePath;
        }

        public void TrustCA()
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
        }
    }
}
