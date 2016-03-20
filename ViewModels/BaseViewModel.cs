/*
 * Created by SharpDevelop.
 * User: Alex
 * Date: 03/20/2016
 * Time: 19:13
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Managerovec.ViewModels
{
	/// <summary>
	/// Description of BaseViewModel.
	/// </summary>
	public class BaseViewModel:INotifyPropertyChanged
	{
		public BaseViewModel()
		{
		}
		//zvoroval, because the old implementation seemed to get stuck sometimes
		#region INotifyPropertyChanged implementation
	    public event PropertyChangedEventHandler PropertyChanged;
	    protected virtual void RaisePropertyChanged(string propertyName)
	    {
	        PropertyChangedEventHandler handler = PropertyChanged;
	        if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
	    }
	    protected bool SetField<T>(ref T field, T value, string propertyName)
	    {
	        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
	        field = value;
	        RaisePropertyChanged(propertyName);
	        return true;
	    }

		#endregion
	}
}
