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
using System.Windows;

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
	    protected void OnPropertyChanged(string propertyName)
	    {
	        PropertyChangedEventHandler handler = PropertyChanged;
	        if (handler != null) 
	        	handler(this, new PropertyChangedEventArgs(propertyName));
	    }
	    
		#endregion
	}
}
