using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace HassadFood.MVVMHelper
{
	public abstract class ExtendedBindableObject : BindableObject, INotifyPropertyChanged
	{
		public void RaisePropertyChanged<T>(Expression<Func<T>> property)
		{
			var name = GetMemberInfo(property).Name;
			OnPropertyChanged(name);
		}

		MemberInfo GetMemberInfo(Expression expression)
		{
			MemberExpression operand;
			LambdaExpression lambdaExpression = (LambdaExpression)expression;
			if (lambdaExpression.Body as UnaryExpression != null)
			{
				UnaryExpression body = (UnaryExpression)lambdaExpression.Body;
				operand = (MemberExpression)body.Operand;
			}
			else
			{
				operand = (MemberExpression)lambdaExpression.Body;
			}
			return operand.Member;
		}

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
		public event PropertyChangedEventHandler PropertyChangedNotify;
		/// <summary>
		/// Raises the property changed event.
		/// </summary>
		/// <param name="propertyName">Property name.</param>
		protected void OnPropertyChangedNotify([CallerMemberName]string propertyName = "") =>
		PropertyChangedNotify?.Invoke(this, new PropertyChangedEventArgs(propertyName));


		//public event PropertyChangedEventHandler PropertyChangedGesture;
		protected virtual void OnPropertyChangedGesture(string propertyName)
		{
			var handler = PropertyChangedNotify;
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

	public class ReadOnlyWrapperViewModel<T> : ExtendedBindableObject
	{
		private T _item;
		public ReadOnlyWrapperViewModel(T item)
		{
			Item = item;
		}

		public T Item { get { return this._item; } private set { SetField(ref this._item, value); } }
	}
	public class ReadOnlySelectable<T> : ExtendedBindableObject
	{

		public ReadOnlySelectable(T item)
		{
			Item = item;
		}
		public T Item { get; private set; }
		//Selectable Support
		private bool _selected;

		public bool Selected
		{
			get { return this._selected; }
			set { SetField(ref this._selected, value); }
		}
	}


}