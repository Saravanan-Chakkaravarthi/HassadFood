using System;
namespace HassadFood.Interface
{
    public interface INotification
    {
        /// <summary>
        /// Registers the notification.
        /// </summary>
        /// <returns>The notification.</returns>
        string RegisterNotification();
        /// <summary>
        /// Unregisters the notification.
        /// </summary>
        void UnRegisterNotification();
        /// <summary>
        /// Get Device ID.
        /// </summary>
        string DeviceID();
        /// <summary>
        /// Updates the app.
        /// </summary>
        void UpdateApp();
    }
}
