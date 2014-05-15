#region Header
////////////////////////////////////////////////////////////////////////////////////
//
//	File Description: Tridion Dependency
// ---------------------------------------------------------------------------------
//	Date Created	: April 4, 2014
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
using System.Linq;
using System.Text;
using TcmCDService.CacheTypes;
using TcmCDService.Logging;

namespace TcmCDService.Caching
{
	/// <summary>
	/// <see cref="TridionDependency" /> manages Tridion dependencies and <see cref="TridionChangeMonitor" />
	/// </summary>
	public class TridionDependency
	{
		private CacheRegion mCacheRegion;
		private String mKey;

		public EventHandler OnChange;

		/// <summary>
		/// Gets the <see cref="T:TcmCDService.CacheClient.CacheRegion" />
		/// </summary>
		/// <value>
		/// The <see cref="T:TcmCDService.CacheClient.CacheRegion" />
		/// </value>
		public CacheRegion CacheRegion
		{
			get
			{
				return mCacheRegion;
			}
		}

		/// <summary>
		/// Gets cache item key
		/// </summary>
		/// <value>
		/// The cache item key
		/// </value>
		public String Key
		{
			get
			{
				return mKey;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TridionDependency"/> class.
		/// </summary>
		/// <param name="cacheRegion"><see cref="T:TcmCDService.CacheClient.CacheRegion" /></param>
		/// <param name="cacheKey">Cache Item Key</param>
		public TridionDependency(CacheRegion cacheRegion, String cacheKey)
		{
			mCacheRegion = cacheRegion;
			mKey = cacheKey;
		}

		/// <summary>
		/// Triggers the <see cref="TridionDependency" />
		/// </summary>
		public void TriggerDependency()
		{
			Logger.Info("Triggering cache dependency for region \"{0}\", key \"{1}\".", mCacheRegion, mKey);

			if (OnChange != null)
				OnChange(this, new EventArgs());
		}

		/// <summary>
		/// Returns a string that represents the current <see cref="TridionDependency" />.
		/// </summary>
		/// <returns>
		/// A string that represents the current <see cref="TridionDependency" />.
		/// </returns>
		public override String ToString()
		{
			return String.Format("{0}-{1}", mCacheRegion.ToString(), mKey);
		}
	}
}
