using System;

namespace HassadFood.Interface
{
	public interface ILoginManager
	{
        void ShowEMailPage();
        void ShowPINPage();
        void ShowLoginPage();
		void ShowMainPage();
		void Logout();
	}
}
