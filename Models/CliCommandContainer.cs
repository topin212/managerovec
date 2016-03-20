/*
 * Created by SharpDevelop.
 * User: Alex
 * Date: 03/19/2016
 * Time: 14:08
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace Managerovec.Models
{
	/// <summary>
	/// Description of CliCommandContainer.
	/// </summary>
	public class CliCommandContainer
	{
		public string command{get; set;}
		public string response { get; set; }
		public CliCommandContainer(string command, string response)
		{
			if (command == null)
				throw new ArgumentNullException("command");
			if (response == null)
				throw new ArgumentNullException("response");
			this.command = command;
			this.response = response;
			
		}
	}
}
