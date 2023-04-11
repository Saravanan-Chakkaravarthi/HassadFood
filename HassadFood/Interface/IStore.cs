using System;
using System.Threading.Tasks;

namespace HassadFood.Interface
{
    public interface IStore
    {
        void StoreOracleCredentials(string username, string password);
        void DeleteOracleCredentials();
        Task<string> GetOracleUsername();
        Task<string> GetOraclePassword();
    }
}
