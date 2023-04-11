using System;
using System.Threading.Tasks;

namespace HassadFood.Interface
{
    public interface ITouchID
    {
        /// <summary>
        /// Login this instance.
        /// </summary>
        /// <returns>The login.</returns>
        Task<string> Login();
        /// <summary>
        /// Ises the touch identifiers upport.
        /// </summary>
        /// <returns>The touch identifiers upport.</returns>
        Task<bool> IsTouchIDSupport();
        /// <summary>
        /// Permissions this instance.
        /// </summary>
        /// <returns>The permissions.</returns>
        bool Permissions();
    }
}
