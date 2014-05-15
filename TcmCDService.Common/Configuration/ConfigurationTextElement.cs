#region Header
////////////////////////////////////////////////////////////////////////////////////
//
//	File Description: Configuration Text Element
// ---------------------------------------------------------------------------------
//	Date Created	: April 3, 2014
//	Author			: Rob van Oostenrijk
// ---------------------------------------------------------------------------------
// 	Change History
//	Date Modified       : 
//	Changed By          : 
//	Change Description  : 
//
////////////////////////////////////////////////////////////////////////////////////
#endregion
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Xml;

namespace TcmCDService.Configuration
{
	/// <summary>
	/// <see cref="ConfigurationTextElement{T}" /> is a <see cref="T:System.Configuration.ConfigurationElement" /> which de-serializes a xml textnode
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ConfigurationTextElement<T> : ConfigurationElement
	{
		private T mValue;

		/// <summary>
		/// Reads XML from the configuration file.
		/// </summary>
		/// <param name="reader">The <see cref="T:System.Xml.XmlReader" /> that reads from the configuration file.</param>
		/// <param name="serializeCollectionKey">true to serialize only the collection key properties; otherwise, false.</param>
		protected override void DeserializeElement(XmlReader reader, bool serializeCollectionKey)
		{
			if (reader == null)
				throw new ArgumentNullException("reader");

			mValue = (T)reader.ReadElementContentAs(typeof(T), null);
		}

		/// <summary>
		/// Returns the <see cref="ConfigurationTextElement{T}" /> value
		/// </summary>
		/// <value>
		/// <see cref="ConfigurationTextElement{T}" /> value
		/// </value>
		public T Value
		{
			get
			{
				return mValue;
			}
		}
	}
}
