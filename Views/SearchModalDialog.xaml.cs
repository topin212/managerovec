/*
 * Created by SharpDevelop.
 * User: Alex
 * Date: 03/21/2016
 * Time: 01:57
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

namespace Managerovec.Views
{
	/// <summary>
	/// Interaction logic for SearchModalDialog.xaml
	/// </summary>
	public partial class SearchModalDialog : Window
	{
		public SearchModalDialog()
		{
			InitializeComponent();
		}
		
		public string tag{
			get{
				return tagtextBox.Text;
			}
		}
		void button1_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}
	}
}