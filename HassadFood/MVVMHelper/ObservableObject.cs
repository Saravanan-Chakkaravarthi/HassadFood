using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HassadFood.MVVMHelper
{
	/// <summary>
	/// Observable object with INotifyPropertyChanged implemented
	/// </summary>
	public class ObservableObject : INotifyPropertyChanged
	{
		/// <summary>
		/// Sets the property.
		/// </summary>
		/// <returns><c>true</c>, if property was set, <c>false</c> otherwise.</returns>
		/// <param name="Details">Object details.</param>
		/// <param name="value">Value.</param>
		/// <param name="propertyName">Property name.</param>
		/// <param name="onChanged">On changed.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		protected bool SetProperty<T>(
			ref T Details, T value,
			[CallerMemberName]string propertyName = "",
			Action onChanged = null)
		{
			if (EqualityComparer<T>.Default.Equals(Details, value))
				return false;

			Details = value;
			onChanged?.Invoke();
			OnPropertyChanged(propertyName);
			return true;
		}

		/// <summary>
		/// Occurs when property changed.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;
		/// <summary>
		/// Raises the property changed event.
		/// </summary>
		/// <param name="propertyName">Property name.</param>
		protected void OnPropertyChanged([CallerMemberName]string propertyName = "") =>
		 PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


		//public event PropertyChangedEventHandler PropertyChangedGesture;
		protected virtual void OnPropertyChangedGesture(string propertyName)
		{
			var handler = PropertyChanged;
			if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
		}

		protected bool SetField<T>(ref T field, T value, Action action = null, IEnumerable<string> additionalprops = null, [CallerMemberName] string propertyName = null)
		{

			if (EqualityComparer<T>.Default.Equals(field, value)) return false;
			field = value;
			OnPropertyChangedGesture(propertyName);
			//Check for related fields
			if (additionalprops != null)
			{
				foreach (var s in additionalprops)
					OnPropertyChangedGesture(s);
			};
			//Fire any post set action that was supplied
			if (action != null) action();
			return true;
		}

	}	
}
