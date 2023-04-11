using System;

using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Hardware.Fingerprints;
using Android.OS;
using Android.Runtime;
using Android.Security.Keystore;
using Android.Support.V4.Content;
using Android.Support.V4.Hardware.Fingerprint;
using Android.Util;

using HassadFood.Droid.Renderer;
using HassadFood.Interface;
using Java.Lang;
using Java.Security;
using Javax.Crypto;
using Plugin.CurrentActivity;
using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;

[assembly: Xamarin.Forms.Dependency(typeof(TouchID))]
namespace HassadFood.Droid.Renderer
{
    public class TouchID : ITouchID
    {
        public async Task<bool> IsTouchIDSupport()
        {

            var Isavailable = await CrossFingerprint.Current.IsAvailableAsync();

            if (Isavailable == true)
                return Isavailable;
            else
                return Isavailable;

            //FingerprintManagerCompat fingerprintManager = FingerprintManagerCompat.From(Xamarin.Forms.Forms.Context);
            //if (fingerprintManager.IsHardwareDetected)
            //{
            //  return true;
            //}
            //return false;
        }

        public async Task<string> Login()
        {

            //Success
            //Failed
            //Enroll Fingerprint
            //Not Supported
            // or use dependency injection and inject IFingerprint
            string resultstatus = string.Empty;
            var request = new AuthenticationRequestConfiguration("Prove you have mvx fingers!", "Because without it you can't have access");
            var result = await CrossFingerprint.Current.AuthenticateAsync(request);
            if (result.Authenticated)
            {
                resultstatus = "Success";
                // do secret stuff :)
            }
            else
            {
                resultstatus = "failed";
                // not allowed to do secret stuff :(
            }
            return resultstatus;

            //FingerprintManagerCompat fingerprintManager = FingerprintManagerCompat.From(Xamarin.Forms.Forms.Context);
            //if (fingerprintManager.IsHardwareDetected)
            //{
            //  if (!fingerprintManager.HasEnrolledFingerprints)
            //  {
            //      // Can't use fingerprint authentication - notify the user that they need to 
            //      // enroll at least one fingerprint with the device.
            //      return "Enroll Fingerprint";
            //  }
            //  const int flags = 0; /* always zero (0) */

            //  // CryptoObjectHelper is described in the previous section.
            //  CryptoObjectHelper cryptoHelper = new CryptoObjectHelper();

            //  // cancellationSignal can be used to manually stop the fingerprint scanner. 
            //  CancellationSignal cancellationSignal = new Android.Support.V4.OS.CancellationSignal();

            //  FingerprintManagerCompat fingerPrintManager = FingerprintManagerCompat.From(Xamarin.Forms.Forms.Context);

            //  // AuthenticationCallback is a base class that will be covered later on in this guide.
            //  FingerprintManagerCompat.AuthenticationCallback authenticationCallback = new SimpleAuthCallbacks();

            //  // Start the fingerprint scanner.
            //  fingerprintManager.Authenticate(cryptoHelper.BuildCryptoObject(), flags, cancellationSignal, authenticationCallback, null);
            //}
            //return "Not Supported";
        }

        [Obsolete]
        public bool Permissions()
        {
            // The context is typically a reference to the current activity.
            Android.Content.PM.Permission permissionResult = ContextCompat.CheckSelfPermission(Xamarin.Forms.Forms.Context, Manifest.Permission.UseFingerprint);
            if (permissionResult == Android.Content.PM.Permission.Granted)
            {
                // Permission granted - go ahead and start the fingerprint scanner.
                return true;
            }
            // No permission. Go and ask for permissions and don't start the scanner. See
            // http://developer.android.com/training/permissions/requesting.html
            return false;
        }
    }

    /// <summary>
    ///     This class encapsulates the creation of a CryptoObject based on a javax.crypto.Cipher.
    /// </summary>
    /// <remarks>Each invocation of BuildCryptoObject will instantiate a new CryptoObjet. 
    /// If necessary a key for the cipher will be created.</remarks>
    public class CryptoObjectHelper
    {
        // ReSharper disable InconsistentNaming
        static readonly string TAG = "X:" + typeof(CryptoObjectHelper).Name;

        // This can be key name you want. Should be unique for the app.
        static readonly string KEY_NAME = "BasicFingerPrintSample.FingerprintManagerAPISample.sample_fingerprint_key";

        // We always use this keystore on Android.
        static readonly string KEYSTORE_NAME = "AndroidKeyStore";

        // Should be no need to change these values.
        static readonly string KEY_ALGORITHM = KeyProperties.KeyAlgorithmAes;
        static readonly string BLOCK_MODE = KeyProperties.BlockModeCbc;
        static readonly string ENCRYPTION_PADDING = KeyProperties.EncryptionPaddingPkcs7;

        static readonly string TRANSFORMATION = KEY_ALGORITHM + "/" +
                                                BLOCK_MODE + "/" +
                                                ENCRYPTION_PADDING;
        // ReSharper restore InconsistentNaming

        readonly KeyStore _keystore;

        public CryptoObjectHelper()
        {
            _keystore = KeyStore.GetInstance(KEYSTORE_NAME);
            _keystore.Load(null);
        }

        public FingerprintManagerCompat.CryptoObject BuildCryptoObject()
        {
            Cipher cipher = CreateCipher();
            return new FingerprintManagerCompat.CryptoObject(cipher);
        }

        /// <summary>
        ///     Creates the cipher.
        /// </summary>
        /// <returns>The cipher.</returns>
        /// <param name="retry">If set to <c>true</c>, recreate the key and try again.</param>
        Cipher CreateCipher(bool retry = true)
        {
            IKey key = GetKey();
            Cipher cipher = Cipher.GetInstance(TRANSFORMATION);
            try
            {
                cipher.Init(CipherMode.EncryptMode, key);
            }
            catch (KeyPermanentlyInvalidatedException e)
            {
                Log.Debug(TAG, "The key was invalidated, creating a new key.");
                _keystore.DeleteEntry(KEY_NAME);
                if (retry)
                {
                    CreateCipher(false);
                }
                else
                {
                    throw new Java.Lang.Exception("Could not create the cipher for fingerprint authentication.", e);
                }
            }
            return cipher;
        }

        /// <summary>
        ///     Will get the key from the Android keystore, creating it if necessary.
        /// </summary>
        /// <returns></returns>
        IKey GetKey()
        {
            if (!_keystore.IsKeyEntry(KEY_NAME))
            {
                CreateKey();
            }

            IKey secretKey = _keystore.GetKey(KEY_NAME, null);
            return secretKey;
        }

        /// <summary>
        ///     Creates the Key for fingerprint authentication.
        /// </summary>
        void CreateKey()
        {
            KeyGenerator keyGen = KeyGenerator.GetInstance(KeyProperties.KeyAlgorithmAes, KEYSTORE_NAME);
            KeyGenParameterSpec keyGenSpec =
                new KeyGenParameterSpec.Builder(KEY_NAME, KeyStorePurpose.Encrypt | KeyStorePurpose.Decrypt)
                    .SetBlockModes(BLOCK_MODE)
                    .SetEncryptionPaddings(ENCRYPTION_PADDING)
                    .SetUserAuthenticationRequired(true)
                    .Build();
            keyGen.Init(keyGenSpec);
            keyGen.GenerateKey();
            Log.Debug(TAG, "New key created for fingerprint authentication.");
        }
    }

    class SimpleAuthCallbacks : FingerprintManagerCompat.AuthenticationCallback
    {
        // ReSharper disable once MemberHidesStaticFromOuterClass
        static readonly string TAG = "X:" + typeof(SimpleAuthCallbacks).Name;
        static readonly byte[] SECRET_BYTES = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        public SimpleAuthCallbacks()
        {
        }

        public override void OnAuthenticationSucceeded(FingerprintManagerCompat.AuthenticationResult result)
        {
            Log.Debug(TAG, "OnAuthenticationSucceeded");
            if (result.CryptoObject.Cipher != null)
            {
                try
                {
                    // Calling DoFinal on the Cipher ensures that the encryption worked.
                    byte[] doFinalResult = result.CryptoObject.Cipher.DoFinal(SECRET_BYTES);
                    Log.Debug(TAG, "Fingerprint authentication succeeded, doFinal results: {0}",
                              Convert.ToBase64String(doFinalResult));

                    ReportSuccess();
                }
                catch (BadPaddingException bpe)
                {
                    Log.Error(TAG, "Failed to encrypt the data with the generated key." + bpe);
                    ReportAuthenticationFailed();
                }
                catch (IllegalBlockSizeException ibse)
                {
                    Log.Error(TAG, "Failed to encrypt the data with the generated key." + ibse);
                    ReportAuthenticationFailed();
                }
            }
            else
            {
                // No cipher used, assume that everything went well and trust the results.
                Log.Debug(TAG, "Fingerprint authentication succeeded.");
                ReportSuccess();
            }
        }

        void ReportSuccess()
        {

        }

        void ReportScanFailure(int errMsgId, string errorMessage)
        {

        }

        void ReportAuthenticationFailed()
        {

        }

        public override void OnAuthenticationError(int errMsgId, ICharSequence errString)
        {
            // There are some situations where we don't care about the error. For example, 
            // if the user cancelled the scan, this will raise errorID #5. We don't want to
            // report that, we'll just ignore it as that event is a part of the workflow.
            bool reportError = (errMsgId == (int)FingerprintState.ErrorCanceled);

            string debugMsg = string.Format("OnAuthenticationError: {0}:`{1}`.", errMsgId, errString);

            ReportScanFailure(errMsgId, errString.ToString());
        }

        public override void OnAuthenticationFailed()
        {
            Log.Info(TAG, "Authentication failed.");
            ReportAuthenticationFailed();
        }

        public override void OnAuthenticationHelp(int helpMsgId, ICharSequence helpString)
        {
            Log.Debug(TAG, "OnAuthenticationHelp: {0}:`{1}`", helpString, helpMsgId);
            ReportScanFailure(helpMsgId, helpString.ToString());
        }
    }



    //You can specify additional application information in this attribute
    [Application]
    public class MainApplication : Application, Application.IActivityLifecycleCallbacks
    {
        public MainApplication(IntPtr handle, JniHandleOwnership transer)
          : base(handle, transer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();
            RegisterActivityLifecycleCallbacks(this);
            //A great place to initialize Xamarin.Insights and Dependency Services!
        }

        public override void OnTerminate()
        {
            base.OnTerminate();
            UnregisterActivityLifecycleCallbacks(this);
        }

        public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivityDestroyed(Activity activity)
        {
        }

        public void OnActivityPaused(Activity activity)
        {
        }

        public void OnActivityResumed(Activity activity)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
        {
        }

        public void OnActivityStarted(Activity activity)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivityStopped(Activity activity)
        {
        }
    }
}
