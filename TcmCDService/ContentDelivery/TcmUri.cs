#region Header
////////////////////////////////////////////////////////////////////////////////////
//
//	File Description: TcmUri
// ---------------------------------------------------------------------------------
//	Date Created	: March 19, 2014
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
using System.Text.RegularExpressions;

namespace TcmCDService.ContentDelivery
{
	/// <summary>
	/// Class to parse and extract elements from Tcm URIs
	/// </summary>
	public class TcmUri
	{
		private const String NULL_URI = "tcm:0-0-0";

		private static Regex mRegex = new Regex(@"tcm:(\d+)-(\d+)-?(\d*)-?v?(\d*)");
		private int mPublicationId;
		private int mItemId;
		private int mItemType;
		private int mVersion;

		/// <summary>
		/// Create a <see cref="TcmUri"/> from its integer parts
		/// </summary>
		/// <param name="publicationId">publication Id (0 for publication uris)</param>
		/// <param name="itemId">item ID</param>
		/// <param name="itemType">item type</param>
		/// <param name="version">item version</param>
		public TcmUri(int publicationId, int itemId, int itemType, int version)
		{
			mPublicationId = publicationId;
			mItemId = itemId;
			mItemType = itemType;
			mVersion = version;
		}

		/// <summary>
		/// Create a <see cref="TcmUri"/> from its integer parts
		/// </summary>
		/// <param name="publicationId">publication Id (0 for publication uris)</param>
		/// <param name="itemId">item ID</param>
		/// <param name="itemType">item type</param>
		public TcmUri(int publicationId, int itemId, int itemType): this(publicationId, itemId, itemType, 0)
		{
		}

		/// <summary>
		/// Create a <see cref="TcmUri"/> from its integer parts
		/// </summary>
		/// <param name="publicationId">publication Id (0 for publication uris)</param>
		/// <param name="itemId">item ID</param>
		public TcmUri(int publicationId, int itemId): this(publicationId, itemId, 0, 0)
		{
		}

		/// <summary>
		/// Parse a <see cref="T:System.String"/> into a <see cref="TcmUri" />
		/// </summary>
		/// <param name="uri">The uri string to parse</param>
		/// <remarks>
		/// Invalid values are converted into the null <see cref="TcmUri" /> (tcm:0-0-0)
		/// </remarks>
		public TcmUri(String uri)
		{
			if (!String.IsNullOrEmpty(uri))
			{
				Match m = mRegex.Match(uri);

				if (m.Success)
				{
					mPublicationId = Convert.ToInt32(m.Groups[1].Value);
					mItemId = Convert.ToInt32(m.Groups[2].Value);

					if (m.Groups.Count > 3 && !string.IsNullOrEmpty(m.Groups[3].Value))
						mItemType = Convert.ToInt32(m.Groups[3].Value);
					else
						mItemType = 16;

					if (m.Groups.Count > 4 && !string.IsNullOrEmpty(m.Groups[4].Value))
						mVersion = Convert.ToInt32(m.Groups[4].Value);
					else
						mVersion = 0;

					return;
				}
			}

			// Null Uri
			mPublicationId = 0;
			mItemId = 0;
			mItemType = 0;
			mVersion = 0;
		}

		/// <summary>
		/// Maps the current <see cref="TcmUri" /> instance to the given publication <see cref="TcmUri" />.
		/// </summary>
		/// <param name="publicationUri">The publication URI.</param>
		/// <returns></returns>
		public void MapUri(TcmUri publicationUri)
		{
			if (this.IsNull || (publicationUri == null || publicationUri.IsNull))
				return;
			else
				this.PublicationId = publicationUri.ItemId;
		}

		/// <summary>
		/// Maps the current <see cref="TcmUri" /> instance to the given publication URI.
		/// </summary>
		/// <param name="publicationUri">The publication URI.</param>
		/// <returns></returns>
		public void MapUri(String publicationUri)
		{
			if (this.IsNull)
				return;

			if (String.IsNullOrEmpty(publicationUri))
				return;
			else
			{
				TcmUri pub = new TcmUri(publicationUri);
				MapUri(pub);
			}
		}

		/// <summary>
		/// Maps the given item uri to the passed publication uri
		/// </summary>
		/// <param name="uri">The URI.</param>
		/// <param name="publicationUri">The publication URI.</param>
		/// <returns></returns>
		public static String MapUri(String uri, String publicationUri)
		{
			if (String.IsNullOrEmpty(publicationUri))
				return uri;
			else
			{
				TcmUri res = new TcmUri(uri);

				if (res.IsNull)
					return uri;

				res.MapUri(publicationUri);
				return res.ToString();
			}
		}

		/// <summary>
		/// <see cref="TcmUri"/> publication id
		/// </summary>
		/// <value>
		/// <see cref="TcmUri"/> publication id
		/// </value>
		public int PublicationId
		{
			get
			{
				return mPublicationId;
			}
			set
			{
				mPublicationId = value;
			}
		}

		/// <summary>
		/// <see cref="TcmUri"/> item id
		/// </summary>
		/// <value>
		/// <see cref="TcmUri"/> item id
		/// </value>
		public int ItemId
		{
			get
			{
				return mItemId;
			}
			set
			{
				mItemId = value;
			}
		}

		/// <summary>
		/// <see cref="TcmUri"/> item type
		/// </summary>
		/// <value>
		/// <see cref="TcmUri"/> item type
		/// </value>
		public int ItemType
		{
			get
			{
				return mItemType;
			}
			set
			{
				mItemType = value;
			}
		}

		/// <summary>
		/// <see cref="TcmUri"/> version
		/// </summary>
		/// <value>
		/// <see cref="TcmUri"/> version
		/// </value>
		public int Version
		{
			get
			{
				return mVersion;
			}
			set
			{
				mVersion = value;
			}
		}

		/// <summary>
		/// Get the uri in string form (tcm:x-y-z)
		/// </summary>
		/// <returns></returns>
		public override String ToString()
		{
			if (mVersion > 0)
				return String.Format("tcm:{0}-{1}-{2}-v{3}", mPublicationId, mItemId, mItemType, mVersion);
			else if (mItemType == 16)
				return String.Format("tcm:{0}-{1}", mPublicationId, mItemId);
			else
				return String.Format("tcm:{0}-{1}-{2}", mPublicationId, mItemId, mItemType);
		}

		/// <summary>
		/// Converts the <see cref="TcmUri" /> into a cache key compatible format
		/// </summary>
		/// <returns></returns>
		public String ToCacheKey()
		{
			if (mPublicationId == 0)
				return mPublicationId.ToString();
			else
				return String.Format("{0}:{1}", mPublicationId, mItemId);
		}

		/// <summary>
		/// Test if this is a null uri (tcm:0-0-0)
		/// </summary>
		/// <returns></returns>
		public Boolean IsNull
		{
			get
			{
				return (mPublicationId == 0 && mItemId == 0 && mItemType == 0);
			}
		}

		/// <summary>
		/// Get the null uri value (tcm:0-0-0)
		/// </summary>
		public static String NullUri
		{
			get
			{
				return NULL_URI;
			}
		}

		/// <summary>
		/// Compares two different <see cref="TcmUri"/> instances for equality
		/// </summary>
		/// <param name="uriA"><see cref="TcmUri"/> A</param>
		/// <param name="uriB"><see cref="TcmUri"/> B</param>
		/// <returns><c>true</c> if the <see cref="TcmUri"/> objects are referencing the same <see cref="TcmUri"/>; otherwise <c>false</c></returns>
		public static bool operator ==(TcmUri uriA, TcmUri uriB)
		{
			// If both are null, or both are same instance, return true.
			if (System.Object.ReferenceEquals(uriA, uriB))
				return true;

			// If one is null, but not both, return false.
			if (((object)uriA == null) || ((object)uriB == null))
				return false;

			return (uriA.PublicationId == uriB.PublicationId &&
					uriA.ItemId == uriB.ItemId &&
					uriA.ItemType == uriB.ItemType &&
					uriA.Version == uriB.Version);
		}

		/// <summary>
		/// Compares two different <see cref="TcmUri"/> instances for non-equality
		/// </summary>
		/// <param name="uriA"><see cref="TcmUri"/> A</param>
		/// <param name="uriB"><see cref="TcmUri"/> B</param>
		/// <returns><c>true</c> if the <see cref="TcmUri"/> objects are referencing the different <see cref="TcmUri"/>; otherwise <c>false</c></returns>
		public static bool operator !=(TcmUri uriA, TcmUri uriB)
		{
			return !(uriA == uriB);
		}

		/// <summary>
		/// Implicitly convert a <see cref="T:System.String"/> into a <see cref="TcmUri"/>
		/// </summary>
		/// <param name="value"><see cref="T:System.String"/> value.</param>
		/// <returns><see cref="TcmUri"/> or <see cref="M:TcmUri.NullUri" /></returns>
		/// <remarks>Input needs to be in tcm uri format of tcm:0-0-0</remarks>
		public static implicit operator TcmUri(String value)
		{
			if (String.IsNullOrEmpty(value))
				return TcmUri.NullUri;

			return new TcmUri(value);
		}

		/// <summary>
		/// Implicitly convert a <see cref="TcmUri"/> into a <see cref="T:System.String" />
		/// </summary>
		/// <param name="value"><see cref="TcmUri"/> value.</param>
		/// <returns><see cref="T:System.String"/></returns>
		/// <remarks>Input needs to be in tcm uri format of tcm:0-0-0</remarks>
		public static implicit operator String(TcmUri value)
		{
			if (value == null)
				return NULL_URI;

			return value.ToString();
		}

		/// <summary>
		/// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
		/// </summary>
		/// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
		/// <returns>
		///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
		/// </returns>
		public override bool Equals(object obj)
		{
			if (obj is TcmUri)
				return this == (TcmUri)obj;

			return false;
		}

		/// <summary>
		/// Returns a hash code for this instance.
		/// </summary>
		/// <returns>
		/// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
		/// </returns>
		public override int GetHashCode()
		{
			return ToString().GetHashCode();
		}
	}
}
