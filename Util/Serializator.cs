/*
 * Created by SharpDevelop.
 * User: Alex
 * Date: 03/21/2016
 * Time: 00:42
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Managerovec.Util
{
	/// <summary>
	/// Description of Serializator.
	/// </summary>
	public class Serializator
	{
		public static void Save(List<Managerovec.Models.FileContainer> obj){
			XmlSerializer serializator = new XmlSerializer(typeof(List<Managerovec.Models.FileContainer>));
			TextWriter writer = new StreamWriter("tags.tags");
			serializator.Serialize(writer, obj);
		}
		
		public static List<Managerovec.Models.FileContainer> Load(){
			XmlSerializer serializator = new XmlSerializer(typeof(List<Managerovec.Models.FileContainer>));
			TextReader reader = new StreamReader("tags.tags");
			return (serializator.Deserialize(reader)) as List<Managerovec.Models.FileContainer>;
		}
		
		public Serializator()
		{
		}
	}
}
