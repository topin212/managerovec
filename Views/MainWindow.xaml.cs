/*
 * Created by SharpDevelop.
 * User: Alex
 * Date: 03/19/2016
 * Time: 21:34
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Managerovec.ViewModels;

namespace Managerovec.Views
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		MainWindowViewModel viewModel;
		public MainWindow()
		{
			InitializeComponent();
			viewModel = DataContext as MainWindowViewModel;
		}
		
		#region Not mvvm-y, but I had no choice
		//I know, I know, don't blame me, I tried to use pure wpf3, which doesn't yet have event2command bind.
		void cliInpuTextBoxEventToCommandTransferer(object sender, KeyEventArgs e)
		{
			string text = (sender as TextBox).Text;
			if(!e.Key.Equals(Key.Enter))
				return;
			if(viewModel.cliProcessorCommand.CanExecute(text)){
				viewModel.cliProcessorCommand.Execute(text);
				(sender as TextBox).Text = "";
			}
		}
		void filesListViewSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Managerovec.Models.FileContainer  selected = ((sender as ListView).SelectedItem as Managerovec.Models.FileContainer);
			viewModel.fileListViewSelectionChangedCommand.Execute(selected);
		}
		void onSaveClickEventTransferer(object sender, RoutedEventArgs e){
			viewModel.saveTagsCommand.Execute(null);
		}
		void onLoadClickEventTransferer(object sender, RoutedEventArgs e){
			viewModel.saveTagsCommand.Execute(null);
		}
		void onSearchClickEventTransferer(object sender, RoutedEventArgs e)
		{
			var dialogWindow = new SearchModalDialog();
			dialogWindow.ShowDialog();
			viewModel.searchTagCommand.Execute(dialogWindow.tag);
		}
		void fileListViewDoubleClickEventTransferer(object sender, MouseButtonEventArgs e)
		{
			try{
				Managerovec.Models.FileContainer selectedItem = (sender as ListView).SelectedItem as Managerovec.Models.FileContainer;
				viewModel.fileListViewDoubleClickCommand.Execute(selectedItem);
			}catch(NullReferenceException exc){
				MessageBox.Show(exc.Message);
			}
			
		}
		
		
		#endregion
	}
}