/*
 * Created by SharpDevelop.
 * User: Alex
 * Date: 03/19/2016
 * Time: 18:16
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace Managerovec.Util
{
	/// <summary>
	/// Implementors should have a close() method.
	/// </summary>
	public interface IClosable
	{
		void close();
	}
}
