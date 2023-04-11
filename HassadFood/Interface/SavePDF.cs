using System;
namespace HassadFood.Interface
{
    public interface SavePDF
    {
        /// <summary>
        /// Show the specified file and fileName.
        /// </summary>
        /// <returns>The show.</returns>
        /// <param name="file">File.</param>
        /// <param name="fileName">File name.</param>
        string Show(byte[] file, string fileName);
        /// <summary>
        /// Opens the pdf.
        /// </summary>
        /// <param name="filePath">File path.</param>
        void OpenPDF(string filePath);
        /// <summary>
        /// Saves the image file.
        /// </summary>
        /// <param name="file">File.</param>
        /// <param name="fileName">File name.</param>
        string SaveImageFile(byte[] file, string fileName);

        void TrustCA();
    }
}
