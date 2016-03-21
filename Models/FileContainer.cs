/*
 * Created by SharpDevelop.
 * User: Alex
 * Date: 03/19/2016
 * Time: 14:07
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

namespace Managerovec.Models
{
	/// <summary>
	/// Obviously, this is a container, that descripts a file, with tags, attributes and it's 
	/// filePath.
	/// </summary>
	[Serializable]
	public class FileContainer
	{
		public string filename{get;set;}
		public string fullName { get; set; }
		public List<string> tags{get;set;}
		public string extension { get; set; }
		public string systemAttributes{get;set;}
		public string systemAttributesConcatted{get;set;}
		public string tagsConcatted{get;set;}
		public DateTime createdAt { get; set; }

		public FileContainer()
		{
		}
		public FileContainer(string filename)
		{
			if (filename == null)
				throw new ArgumentNullException("filename");
			this.filename = filename;
		}
		public FileContainer(string filename, string systemAttributes)
		{
			if (filename == null)
				throw new ArgumentNullException("filename");
			if (systemAttributes == null)
				throw new ArgumentNullException("systemAttributes");
			this.filename = filename;
			this.systemAttributes = systemAttributes;
			this.systemAttributesConcatted = String.Join(" ", systemAttributes);			
		}
		public FileContainer(string filename, List<string> tags, string systemAttributes)
		{
			if (filename == null)
				throw new ArgumentNullException("filename");
			if (tags == null)
				throw new ArgumentNullException("tags");
			if (systemAttributes == null)
				throw new ArgumentNullException("systemAttributes");
			this.filename = filename;
			this.tags = tags;
			this.systemAttributes = systemAttributes;
			this.systemAttributesConcatted = String.Join(" ", systemAttributes);
			this.tagsConcatted = String.Join(" ", tags);		
		}
		public FileContainer(string filename, List<string> tags, string extension, string systemAttributes)
		{
			if (filename == null)
				throw new ArgumentNullException("filename");
			if (tags == null)
				throw new ArgumentNullException("tags");
			if (extension == null)
				throw new ArgumentNullException("extension");
			if (systemAttributes == null)
				throw new ArgumentNullException("systemAttributes");
			this.filename = filename;
			this.tags = tags;
			this.extension = extension;
			this.systemAttributes = systemAttributes;
			this.tagsConcatted = String.Join(" ", tags);
		}
		public FileContainer(string filename, string extension, string systemAttributes, DateTime createdAt)
		{
			if (filename == null)
				throw new ArgumentNullException("filename");
			if (extension == null)
				throw new ArgumentNullException("extension");
			if (systemAttributes == null)
				throw new ArgumentNullException("systemAttributes");
			this.filename = filename;
			this.extension = extension;
			this.systemAttributes = systemAttributes;
			this.createdAt = createdAt;
		}
		public FileContainer(string filename, string fullName, string extension, string systemAttributes, DateTime createdAt)
		{
			if (filename == null)
				throw new ArgumentNullException("filename");
			if (fullName == null)
				throw new ArgumentNullException("fullName");
			if (extension == null)
				throw new ArgumentNullException("extension");
			if (systemAttributes == null)
				throw new ArgumentNullException("systemAttributes");
			this.filename = filename;
			this.fullName = fullName;
			this.extension = extension;
			this.systemAttributes = systemAttributes;
			this.createdAt = createdAt;
		}
		
		public List<string> addTag(string tagName){
			tags.Add(tagName);
			return tags;
		}
		public List<string> removeTag(string tagname){
			tags.Remove(tagname);
			return tags;
		}
		
		public string joinedTags {
			get { return this.tagsConcatted = String.Join(" ", tags); }
		}
		
		//thanks SharpDevelop for this:
		#region Equals and GetHashCode implementation
		public override bool Equals(object obj)
		{
			FileContainer other = obj as FileContainer;
				if (other == null)
					return false;
						return this.filename == other.filename && 
							this.fullName == other.fullName && 
							object.Equals(this.tags, other.tags) && 
							this.extension == other.extension && 
							this.systemAttributes == other.systemAttributes && 
							this.systemAttributesConcatted == other.systemAttributesConcatted && 
							this.tagsConcatted == other.tagsConcatted && 
							this.createdAt == other.createdAt;
		}

		public override int GetHashCode()
		{
			int hashCode = 0;
			unchecked {
				if (filename != null)
					hashCode += 1000000007 * filename.GetHashCode();
				if (fullName != null)
					hashCode += 1000000009 * fullName.GetHashCode();
				if (tags != null)
					hashCode += 1000000021 * tags.GetHashCode();
				if (extension != null)
					hashCode += 1000000033 * extension.GetHashCode();
				if (systemAttributes != null)
					hashCode += 1000000087 * systemAttributes.GetHashCode();
				if (systemAttributesConcatted != null)
					hashCode += 1000000093 * systemAttributesConcatted.GetHashCode();
				if (tagsConcatted != null)
					hashCode += 1000000097 * tagsConcatted.GetHashCode();
				hashCode += 1000000103 * createdAt.GetHashCode();
			}
			return hashCode;
		}

		public static bool operator ==(FileContainer lhs, FileContainer rhs) {
			if (ReferenceEquals(lhs, rhs))
				return true;
			if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null))
				return false;
			return lhs.Equals(rhs);
		}

		public static bool operator !=(FileContainer lhs, FileContainer rhs) {
			return !(lhs == rhs);
		}

		#endregion
	}
}
