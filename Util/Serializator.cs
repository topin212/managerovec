/*
 * Created by SharpDevelop.
 * User: Alex
 * Date: 03/21/2016
 * Time: 00:42
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Collections.ObjectModel;

namespace Managerovec.Util
{
	/// <summary>
	/// Description of Serializator.
	/// </summary>
	public class Serializator
	{
		public static void Save(List<Managerovec.Models.DirInf> obj){
			XmlSerializer serializator = new XmlSerializer(typeof(List<Managerovec.Models.DirInf>));
			TextWriter writer = new StreamWriter("tags.tags");
			serializator.Serialize(writer, obj);
		}
		
		public static List<Managerovec.Models.DirInf> Load(){
			XmlSerializer serializator = new XmlSerializer(typeof(List<Managerovec.Models.DirInf>));
			TextReader reader = new StreamReader("tags.tags");
			return (serializator.Deserialize(reader)) as List<Managerovec.Models.DirInf>;
		}
		
		public Serializator()
		{
		}
	}
}
