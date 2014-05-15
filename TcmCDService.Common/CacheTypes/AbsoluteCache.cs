#region Header
////////////////////////////////////////////////////////////////////////////////////
//
//	File Description: Absolute Cache
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
using TcmCDService.Configuration;
namespace TcmCDService.CacheTypes
{
	/// <summary>
	/// <see cref="AbsoluteCache" /> is a cache type which specifies absolute expiry caching of data
	/// </summary>
	public class AbsoluteCache : CacheType
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="AbsoluteCache" /> class.
		/// </summary>
		/// <param name="settings"><see cref="T:TcmCDService.Configuration.Settings" /></param>
		public AbsoluteCache(Settings settings): base(settings)
		{
		}

		/// <summary>
		/// Gets the expiration of cache items in minutes
		/// </summary>
		/// <value>
		/// Expiration of cache items in minutes
		/// </value>
		/// <remarks>-1 means no caching applies</remarks>
		public override int Expiration
		{
			get
			{
				return Config.Instance.DefaultCacheExpiry;
			}
		}
	}
}
