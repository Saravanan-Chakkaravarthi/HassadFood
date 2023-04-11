using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HassadFood.Droid;
using HassadFood.Interface;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(Credentials))]
namespace HassadFood.Droid
{
    public class Credentials : IStore
    {
        IDictionary<string, object> properties = Xamarin.Forms.Application.Current.Properties;

        /// <summary>
        /// Deletes the oracle credentials.
        /// </summary>
        public void DeleteOracleCredentials()
        {
            if (properties.ContainsKey("OracleUserName"))
            {
                SecureStorage.Remove(App.AppName + "_" + properties["OracleUserName"] as string + "_oracle");
                SecureStorage.Remove(App.AppName + "_" + "password_oracle");
            }
        }

        /// <summary>
        /// Gets the oracle password.
        /// </summary>
        /// <returns>The oracle password.</returns>
        public async Task<string> GetOraclePassword()
        {
            try
            {
                var oauthToken = await SecureStorage.GetAsync(App.AppName + "_" + "password_oracle");
                return oauthToken;
            }
            catch (Exception ex)
            {
                // Possible that device doesn't support secure storage on device.
            }
            return null;
        }
        /// <summary>
        /// Gets the oracle username.
        /// </summary>
        /// <returns>The oracle username.</returns>
        public async Task<string> GetOracleUsername()
        {
            try
            {
                if (properties.ContainsKey("OracleUserName"))
                {
                    var oauthToken = await SecureStorage.GetAsync(App.AppName + "_" + properties["OracleUserName"] as string + "_oracle");
                    return oauthToken;
                }
                return null;
            }
            catch (Exception ex)
            {
                // Possible that device doesn't support secure storage on device.
            }
            return null;
        }
        /// <summary>
        /// Stores the oracle credentials.
        /// </summary>
        /// <param name="username">Username.</param>
        /// <param name="password">Password.</param>
        public async void StoreOracleCredentials(string username, string password)
        {
            if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
            {
                try
                {
                    await SecureStorage.SetAsync(App.AppName + "_" + username + "_oracle", username);
                    await SecureStorage.SetAsync(App.AppName + "_" + "password_oracle", password);
                }
                catch (Exception ex)
                {
                    // Possible that device doesn't support secure storage on device.
                }
            }
        }

    }
}
