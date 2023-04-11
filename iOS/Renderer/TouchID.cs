using System;
using System.Threading.Tasks;
using Foundation;
using LocalAuthentication;
using HassadFood.Interface;
using HassadFood.iOS.Renderer;

[assembly: Xamarin.Forms.Dependency(typeof(TouchID))]
namespace HassadFood.iOS.Renderer
{
    public class TouchID : ITouchID
    {
        public async Task<bool> IsTouchIDSupport()
        {
            LAContext context = new LAContext();
            NSError AuthError;
            if (context.CanEvaluatePolicy(LAPolicy.DeviceOwnerAuthenticationWithBiometrics, out AuthError))
                return true;
            return false;
        }

        public async Task<string> Login()
        {
            LAContext context = new LAContext();
            NSError AuthError;
            var myReason = context.BiometryType == LABiometryType.None ? new NSString("Touch your finger to login.") : new NSString("Show your face to login.");
            if (context.CanEvaluatePolicy(LAPolicy.DeviceOwnerAuthenticationWithBiometrics, out AuthError))
            {
                var authenticated = await context.EvaluatePolicyAsync(LAPolicy.DeviceOwnerAuthenticationWithBiometrics, myReason);
                if (authenticated.Item1)
                {
                    return "Success";
                }
                return "Failed";
            }
            return "Not Supported";
        }

        public bool Permissions()
        {
            return true;
        }
    }
}
