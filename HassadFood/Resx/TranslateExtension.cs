using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using HassadFood.Interface;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HassadFood.Resx
{
    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
        readonly CultureInfo ci;
        const string ResourceId = "HassadFood.Resx.AppResources";
        /// <summary>
        /// Initializes a new instance of the <see cref="T:HassadFood.Resx.TranslateExtension"/> class.
        /// </summary>
        public TranslateExtension()
        {
            if (string.IsNullOrEmpty(App.CultureCode))
                ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
            else 
                ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo(App.CultureCode);
        }
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text { get; set; }
        /// <summary>
        /// Provides the value.
        /// </summary>
        /// <returns>The value.</returns>
        /// <param name="serviceProvider">Service provider.</param>
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Text == null)
                return "";

            ResourceManager temp = new ResourceManager(ResourceId, typeof(TranslateExtension).GetTypeInfo().Assembly);
            var translation = temp.GetString(Text, ci);
            if (translation == null)
            {

            #if DEBUG
                throw new ArgumentException(string.Format("Key '{0}' was not found in resources '{1}' for culture '{2}'.", Text, ResourceId, ci.Name), "Text");
            #else
                    translation = Text; // HACK: returns the key, which GETS DISPLAYED TO THE USER
            #endif
            }
            return translation;
        }
    }
}
