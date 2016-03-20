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
		public MainWindow()
		{
			InitializeComponent();
			selectedFileNameLabel.Content = "Hello world";
		}
		//I know, I know, don't blame me, I tried to use pure wpf3, which doesn't yet have event2command bind.
		void cliInpuTextBoxEventToCommandTransferer(object sender, KeyEventArgs e)
		{
			string text = (sender as TextBox).Text;
			var viewModel = DataContext as MainWindowViewModel;
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
			var viewModel = DataContext as MainWindowViewModel;
			viewModel.fileListViewSelectionChangedCommand.Execute(selected);
		}
	}
}