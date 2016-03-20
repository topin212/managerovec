/*
 * Created by SharpDevelop.
 * User: Alex
 * Date: 03/19/2016
 * Time: 14:30
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
#region system usings
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
#endregion
#region my usings
using Managerovec.Models;
using Managerovec.Util;
using Managerovec.Views;
#endregion
namespace Managerovec.ViewModels
{
	/// <summary>
	/// View model for login view.
	/// </summary>
	public class loginViewModel : BaseViewModel
	{
		LoginModel loginModel = new LoginModel();
		public ICommand loginButtonCommand {
			get;
			set;
		}
		//public ICommand inputLostFocus { get; set; }
		
		public string login { 
			get{
				return loginModel.login;
			}
			set{
				loginModel.login = value;
				RaisePropertyChanged("login");
			}
		}
		public string password{
			get{
				return loginModel.password;
			}
			set{
				loginModel.password = value;
				RaisePropertyChanged("password");
			}
		}
		public bool CanLogIn(){
			bool canlogIn;
			try {
				canlogIn = !login.Equals(null) && !password.Equals(null);
			} catch (NullReferenceException exc) {
				canlogIn = false;
			}
			return canlogIn;
		}
		
		public loginViewModel()
		{
			//loginButtonCommand = new RelayCommand(showMessage, param=>true);
			//inputLostFocus = new RelayCommand(refresh, param=>true);
			loginButtonCommand = new RelayCommand(showMessage, param=>true);
		}
		void refresh(object nul){
			loginButtonCommand = new RelayCommand(showMessage, param=>CanLogIn());
		}
		void showMessage(object message){
			//TODO add a check here for login/password
			var window = message as Window;
			var newWindow = new MainWindow();
			//MessageBox.Show("HEHEHEHE " + message.ToString());
			window.Close();
			newWindow.Show();
		}
	}
}
