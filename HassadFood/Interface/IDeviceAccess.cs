using System;
namespace HassadFood.Interface
{
    public interface IDeviceAccess
    {
        /// <summary>
        /// Opens the device settings.
        /// </summary>
        void OpenDeviceSettings();
        /// <summary>
        /// Gets the camera permission async.
        /// </summary>
        /// <returns><c>true</c>, if camera permission async was gotten, <c>false</c> otherwise.</returns>
        bool GetCameraPermissionAsync();
        /// <summary>
        /// Gets the storage permission async.
        /// </summary>
        /// <returns><c>true</c>, if storage permission async was gotten, <c>false</c> otherwise.</returns>
        bool GetStoragePermissionAsync();
    }
}
