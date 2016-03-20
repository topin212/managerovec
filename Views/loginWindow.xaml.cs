/*
 * Created by SharpDevelop.
 * User: Alex
 * Date: 19.03.2016
 * Time: 16:43
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
using Managerovec.Util;

namespace Managerovec.Views
{
	/// <summary>
	/// Interaction logic for loginWindow.xaml
	/// </summary>
	public partial class loginWindow : Window, IClosable
	{		
		public loginWindow()
		{
			InitializeComponent();
		}
		public void close(){
			//MessageBox.Show("Me closing");
			this.Close();
		}
	}
}