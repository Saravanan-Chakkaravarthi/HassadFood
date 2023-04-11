using System;
using System.Globalization;
using System.Threading;
using HassadFood.Droid;
using HassadFood.Interface;
using Xamarin.Forms;

[assembly: Dependency(typeof(Localize))]
namespace HassadFood.Droid
{
    public class Localize : ILocalize
    {
        public Localize()
        {
        }
        /// <summary>
        /// Changes the locale.
        /// </summary>
        /// <param name="sLanguageCode">S language code.</param>
        public void ChangeLocale(string sLanguageCode)
        {
            var ci = CultureInfo.CreateSpecificCulture(sLanguageCode);
            ci.DateTimeFormat = DateTimeFormatInfo.InvariantInfo;
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
            Console.WriteLine("ChangeToLanguage: " + ci.Name);
        }
        /// <summary>
        /// Gets the current culture info.
        /// </summary>
        /// <returns>The current culture info.</returns>
        public CultureInfo GetCurrentCultureInfo()
        {
            //var androidLocale = Java.Util.Locale.Default; // user's preferred locale
            //         var netLocale = androidLocale.ToString().Replace("_", "-");

            //#region Debugging output
            //Console.WriteLine("android:  " + androidLocale.ToString());
            //Console.WriteLine("netlang:  " + netLocale);

            //var ci = new CultureInfo(netLocale);
            //Thread.CurrentThread.CurrentCulture = ci;
            //Thread.CurrentThread.CurrentUICulture = ci;
            //Console.WriteLine("thread:  " + Thread.CurrentThread.CurrentCulture);
            //Console.WriteLine("threadui:" + Thread.CurrentThread.CurrentUICulture);
            //#endregion

            return new CultureInfo("en");
        }
        /// <summary>
        /// Gets the current culture info.
        /// </summary>
        /// <returns>The current culture info.</returns>
        /// <param name="sLanguageCode">S language code.</param>
        public CultureInfo GetCurrentCultureInfo(string sLanguageCode)
        {
            return CultureInfo.CreateSpecificCulture(sLanguageCode);
        }
        /// <summary>
        /// Sets the locale.
        /// </summary>
        public void SetLocale()
        {
            var ci = GetCurrentCultureInfo();
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
        }
    }
}
