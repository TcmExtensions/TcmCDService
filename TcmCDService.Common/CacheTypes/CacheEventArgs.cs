#region Header
////////////////////////////////////////////////////////////////////////////////////
//
//	File Description: Cache Event Args
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

namespace TcmCDService.CacheTypes
{
	public delegate void CacheEventHandler(Object sender, CacheEventArgs data);

	/// <summary>
	/// <see cref="CacheEventArgs" /> provides a event callback for any <see cref="T:TcmCDService.CacheTypes.CacheType" /> events.
	/// </summary>
	public class CacheEventArgs : EventArgs
	{
		private CacheRegion mRegion;
		private String mKey;
		private CacheEventType mEventType;

		/// <summary>
		/// Gets the <see cref="T:TcmCDService.CacheTypes.CacheRegion" />
		/// </summary>
		/// <value>
		/// The <see cref="T:TcmCDService.CacheTypes.CacheRegion" />
		/// </value>
		public CacheRegion Region
		{
			get
			{
				return mRegion;
			}
		}

		/// <summary>
		/// Gets the cache item key
		/// </summary>
		/// <value>
		/// The cache item key.
		/// </value>
		public String Key
		{
			get
			{
				return mKey;
			}
		}

		/// <summary>
		/// Gets the <see cref="T:TcmCDService.CacheTypes.CacheEventType" />
		/// </summary>
		/// <value>
		/// The <see cref="T:TcmCDService.CacheTypes.CacheEventType" />
		/// </value>
		public CacheEventType EventType
		{
			get
			{
				return mEventType;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CacheEventArgs" /> class.
		/// </summary>
		/// <param name="region"><see cref="T:TcmCDService.CacheTypes.CacheRegion" /></param>
		/// <param name="key">Cache item Key.</param>
		/// <param name="eventType"><see cref="T:TcmCDService.CacheTypes.CacheEventType" /></param>
		public CacheEventArgs(CacheRegion region, String key, CacheEventType eventType)
		{
			mRegion = region;
			mKey = key;
			mEventType = eventType;
		}
	}
}
