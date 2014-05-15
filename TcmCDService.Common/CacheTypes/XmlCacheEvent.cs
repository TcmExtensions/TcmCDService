using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using TcmCDService.Logging;

namespace TcmCDService.CacheTypes
{
	/// <summary>
	/// <see cref="XmlCacheEvent" /> is an <see cref="T:System.Xml.Serialization.IXmlSerializable" /> object which serializes
	/// and deserializes cache event XML.
	/// </summary>
	[XmlRoot("cacheEvent", Namespace = "")]
	public class XmlCacheEvent : IXmlSerializable
	{
		private static XmlSerializer mSerializer = new XmlSerializer(typeof(XmlCacheEvent));

		private String mRegionPath;
		private String mKey;
		private CacheEventType mEventType;

		/// <summary>
		/// Gets or sets the <see cref="XmlCacheEvent" /> region.
		/// </summary>
		/// <value>
		/// The <see cref="XmlCacheEvent" /> region.
		/// </value>
		public String RegionPath
		{
			get
			{
				return mRegionPath;
			}
			set
			{
				mRegionPath = value;
			}
		}

		/// <summary>
		/// Gets or sets the <see cref="XmlCacheEvent" /> key.
		/// </summary>
		/// <value>
		/// The <see cref="XmlCacheEvent" /> key.
		/// </value>
		public String Key
		{
			get
			{
				return mKey;
			}
			set
			{
				mKey = value;
			}
		}

		/// <summary>
		/// Gets or sets the <see cref="XmlCacheEvent" /> <see cref="T:TcmCDService.CacheTypes.CacheEventType" />.
		/// </summary>
		/// <value>
		/// The <see cref="XmlCacheEvent" /> <see cref="T:TcmCDService.CacheTypes.CacheEventType" />.
		/// </value>
		public CacheEventType EventType
		{
			get
			{
				return mEventType;
			}
			set
			{
				mEventType = value;
			}
		}

		/// <summary>
		/// Gets the <see cref="T:TcmCDService.CacheTypes.CacheRegion" /> for this <see cref="XmlCacheEvent" />
		/// </summary>
		/// <value>
		/// <see cref="T:TcmCDService.CacheTypes.CacheRegion" /> for this <see cref="XmlCacheEvent" />
		/// </value>
		public CacheRegion CacheRegion
		{
			get
			{
				return CacheRegionExtensions.ToCacheRegion(mRegionPath);
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="XmlCacheEvent" /> class.
		/// </summary>
		public XmlCacheEvent()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="XmlCacheEvent" /> class.
		/// </summary>
		/// <param name="cacheRegion"><see cref="T:TcmCDService.CacheTypes.CacheRegion" /></param>
		/// <param name="key">Cache Key as <see cref="T:System.String" /></param>
		/// <param name="eventType"><see cref="T:TcmCDService.CacheTypes.CacheEventType"/></param>
		public XmlCacheEvent(CacheRegion cacheRegion, String key, CacheEventType eventType)
		{
			mRegionPath = CacheRegionExtensions.FromCacheRegion(cacheRegion);
			mKey = key;
			mEventType = eventType;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="XmlCacheEvent" /> class.
		/// </summary>
		/// <param name="cacheRegion"><see cref="T:TcmCDService.CacheTypes.CacheRegion" /></param>
		/// <param name="key">Cache Key as <see cref="T:System.Int32" /></param>
		/// <param name="eventType"><see cref="T:TcmCDService.CacheTypes.CacheEventType"/></param>
		public XmlCacheEvent(CacheRegion cacheRegion, int key, CacheEventType eventType)
		{
			mRegionPath = CacheRegionExtensions.FromCacheRegion(cacheRegion);
			mKey = key.ToString();
			mEventType = eventType;
		}

		/// <summary>
		/// This method is reserved and should not be used. When implementing the IXmlSerializable interface, you should return null (Nothing in Visual Basic) from this method, and instead, if specifying a custom schema is required, apply the <see cref="T:System.Xml.Serialization.XmlSchemaProviderAttribute" /> to the class.
		/// </summary>
		/// <returns>
		/// An <see cref="T:System.Xml.Schema.XmlSchema" /> that describes the XML representation of the object that is produced by the <see cref="M:System.Xml.Serialization.IXmlSerializable.WriteXml(System.Xml.XmlWriter)" /> method and consumed by the <see cref="M:System.Xml.Serialization.IXmlSerializable.ReadXml(System.Xml.XmlReader)" /> method.
		/// </returns>
		public XmlSchema GetSchema()
		{
			return null;
		}

		/// <summary>
		/// Generates an object from its XML representation.
		/// </summary>
		/// <param name="reader">The <see cref="T:System.Xml.XmlReader" /> stream from which the object is deserialized.</param>
		public void ReadXml(XmlReader reader)
		{
			if (reader.MoveToAttribute("regionPath"))
				mRegionPath = reader.Value;

			if (reader.MoveToAttribute("key"))
				mKey = reader.Value;

			if (reader.MoveToAttribute("type"))
				mEventType = (CacheEventType)int.Parse(reader.Value);
		}

		/// <summary>
		/// Converts an object into its XML representation.
		/// </summary>
		/// <param name="writer">The <see cref="T:System.Xml.XmlWriter" /> stream to which the object is serialized.</param>
		public void WriteXml(XmlWriter writer)
		{
			writer.WriteAttributeString("regionPath", mRegionPath);
			writer.WriteAttributeString("key", mKey);
			writer.WriteAttributeString("type", ((int)mEventType).ToString());
		}

		/// <summary>
		/// Deserialize a <see cref="XmlCacheEvent" /> from <see cref="P:xml" />
		/// </summary>
		/// <returns><see cref="XmlCacheEvent" /> or null</returns>
		public static XmlCacheEvent FromXml(String xml)
		{
			if (!String.IsNullOrEmpty(xml))
			{
				try
				{
					using (StringReader stringReader = new StringReader(xml))
					{
						return mSerializer.Deserialize(stringReader) as XmlCacheEvent;
					}
				}
				catch (Exception ex)
				{
					Logger.Warning("XmlCacheEvent: Failed to deserialize from \"{0}\".", ex, xml);
				}
			}

			return null;
		}

		/// <summary>
		/// Serializes the <see cref="XmlCacheEvent" /> to Xml
		/// </summary>
		/// <returns>Serialized Xml or null</returns>
		public String ToXml()
		{
			try
			{
				using (StringWriter stringWriter = new StringWriter())
				{
					using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings { OmitXmlDeclaration = true }))
					{
						mSerializer.Serialize(xmlWriter, this);
					}

					return stringWriter.ToString();
				}
			}
			catch (Exception ex)
			{
				Logger.Error("XmlCacheEvent: Failed to serialize to xml", ex);
			}

			return null;
		}
	}
}
