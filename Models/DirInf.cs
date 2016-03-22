/*
 * Created by SharpDevelop.
 * User: Alex
 * Date: 03/22/2016
 * Time: 20:24
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

namespace Managerovec.Models
{
	/// <summary>
	/// Description of DirInf.
	/// </summary>
	[Serializable]
	public class DirInf
	{
		public string  path { get; set; }
		public List<FileContainer> filesAndDirectories { get; set; }
		
		public DirInf(){}
		public DirInf(string path, List<FileContainer> filesAndDirectories)
		{
			if (path == null)
				throw new ArgumentNullException("path");
			if (filesAndDirectories == null)
				throw new ArgumentNullException("filesAndDirectories");
			this.path = path;
			this.filesAndDirectories = filesAndDirectories;
			
		}
	}
}
