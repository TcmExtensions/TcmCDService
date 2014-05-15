#region Header
////////////////////////////////////////////////////////////////////////////////////
//
//	File Description: Configuration Key Value Element
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
using System.Text;
using System.Xml;

namespace TcmCDService.Configuration
{
	public class ConfigurationKeyValueElement : ConfigurationTextElement<String>
	{
		private String mKey;

		public String Key
		{
			get
			{
				return mKey;
			}
		}

		/// <summary>
		/// Reads XML from the configuration file.
		/// </summary>
		/// <param name="reader">The <see cref="T:System.Xml.XmlReader" /> that reads from the configuration file.</param>
		/// <param name="serializeCollectionKey">true to serialize only the collection key properties; otherwise, false.</param>
		protected override void DeserializeElement(XmlReader reader, bool serializeCollectionKey)
		{
			if (reader == null)
				throw new ArgumentNullException("reader");

			if (reader.MoveToAttribute("key"))
				mKey = reader.Value;

			reader.MoveToElement();

			base.DeserializeElement(reader, serializeCollectionKey);
		}

		/// <summary>
		/// Returns a string that represents the current object.
		/// </summary>
		/// <returns>
		/// A string that represents the current object.
		/// </returns>
		public override String ToString()
		{
			return mKey;
		}
	}
}
