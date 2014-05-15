#region Header
////////////////////////////////////////////////////////////////////////////////////
//
//	File Description: Null Cache
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

using TcmCDService.Configuration;
namespace TcmCDService.CacheTypes
{
	/// <summary>
	/// <see cref="NullCache" /> is a cache type which does not allow caching of any data.
	/// </summary>
	public class NullCache : CacheType
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NullCache" /> class.
		/// </summary>
		/// <param name="settings"><see cref="T:TcmCDService.Configuration.Settings" /></param>
		public NullCache(Settings settings): base(settings)
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
				return -1;
			}
		}
	}
}
