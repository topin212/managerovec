/*
 * Created by SharpDevelop.
 * User: Alex
 * Date: 03/19/2016
 * Time: 17:25
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Windows.Input;

namespace Managerovec.Util
{
	/// <summary>
	/// RelayCommand is just an updgrade for ICommand, serves as a wrapper.
	/// More info can be found here: http://www.codeproject.com/Tips/813345/Basic-MVVM-and-ICommand-Usage-Example
	/// Yeah, I read it, and I know what's happening here(kinda).
	/// </summary>
	public class RelayCommand : ICommand
	{
		private Action<object> execute;
		private Predicate<object> canExecute;
		
		//HACK The only secret here is this: EventHandler CanExecuteChangedInternal;
		private event EventHandler CanExecuteChangedInternal;
		
		public RelayCommand()
		{
			execute = x=>{return;};
			canExecute = x=>false;
		}
		public RelayCommand(Action<object> execute){
			if (execute == null)
				throw new ArgumentNullException("execute");
			this.execute = execute;
			canExecute = x => true;
		}
		public RelayCommand(Action<object> execute, Predicate<object> canExecute)
		{
			if (execute == null)
				throw new ArgumentNullException("execute");
			if (canExecute == null)
				throw new ArgumentNullException("canExecute");
			this.execute = execute;
			this.canExecute = canExecute;
			
		}
		
		#region ICommand implementation
		public event EventHandler CanExecuteChanged{
			add{
				CommandManager.RequerySuggested += value;
				this.CanExecuteChangedInternal += value;
			}
			remove{
				CommandManager.RequerySuggested -=value;
				CanExecuteChangedInternal -=value; 	
			}
		}
		//This is a pretty neat trick, both parts get run, if the first parameter is true, because of &&
		public bool CanExecute(object parameter)
		{
			return canExecute != null && this.canExecute(parameter);
		}
		public void Execute(object parameter)
		{
			execute(parameter);
		}
		#endregion
		
		//Another neat trick, in the world of multi-threading our object 
		//can become null RIGHT after we checked it for null
		//That's why we save the handler for a second.
		public void OnCanExecuteChanged(){
			EventHandler handler = CanExecuteChangedInternal;
			if(handler!=null){
				handler.Invoke(this,EventArgs.Empty);
			}
		}
		public void Clear(){
			canExecute = x => false;
			execute = _ => {return;};
		}
	}
}
