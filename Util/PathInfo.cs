/*
 * Created by SharpDevelop.
 * User: Alex
 * Date: 03/22/2016
 * Time: 19:15
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Text.RegularExpressions;

namespace Managerovec.Util
{
	/// <summary>
	/// Description of PathInfo.
	/// </summary>
	public class PathInfo
	{
		public string path{get;set;}
		private string[] pathElements;
		public int levels;
		
		public int getNumberOfLevels(){
			pathElements = path.Split('\\');
			levels = pathElements.Length;
			Console.WriteLine("Source path: {0}", path);
			Console.WriteLine("{0} elements found", levels);
			Console.WriteLine("Here are all the elements: ");
			
			foreach (var element in pathElements) {
				Console.WriteLine("\t{0}", element);
			}return levels;
		}
		
		public PathInfo(){}
		public PathInfo(string path)
		{
			if (path == null)
				throw new ArgumentNullException("path");
			this.path = path;
		}
	}
}
