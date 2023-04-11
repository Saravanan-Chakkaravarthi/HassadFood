using System;
using Xamarin.Forms;

namespace HassadFood
{
	public class CustomEntry : Xamarin.Forms.Entry
	{		
        // Need to overwrite default handler because we cant Invoke otherwise
		public new event EventHandler Completed;
		/// <summary>
		/// The return type property.
		/// </summary>
		public static readonly BindableProperty ReturnTypesProperty = BindableProperty.Create(
			nameof(ReturnType),
			typeof(ReturnType),
			typeof(CustomEntry),
			ReturnType.Done,
			BindingMode.OneWay
		);
		/// <summary>
		/// Gets or sets the type of the return.
		/// </summary>
		/// <value>The type of the return.</value>
		public ReturnType ReturnTypes
		{
			get { return (ReturnType)GetValue(ReturnTypeProperty); }
			set { SetValue(ReturnTypeProperty, value); }
		}
		/// <summary>
		/// Invokes the completed.
		/// </summary>
		public void InvokeCompleted()
		{
			if (this.Completed != null)
				this.Completed.Invoke(this, null);
		}

	}

	public enum ReturnTypes
	{
		Go,
		Next,
		Done,
		Send,
		Search
	}
}
