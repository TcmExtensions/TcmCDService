#region Header
////////////////////////////////////////////////////////////////////////////////////
//
//	File Description: Cache Channel Event Listener
// ---------------------------------------------------------------------------------
//	Date Created	: April 15, 2014
//	Author			: Rob van Oostenrijk
// ---------------------------------------------------------------------------------
// 	Change History
//	Date Modified       : 
//	Changed By          : 
//	Change Description  : 
//
////////////////////////////////////////////////////////////////////////////////////
#endregion

namespace Com.Tridion.Cache
{
	/// <summary>
	/// <see cref="ICacheChannelEventListener" /> provides an equivalent to interface com.tridion.cache.CacheChannelEventListener
	/// </summary>
	public interface ICacheChannelEventListener
	{
		/// <summary>
		/// Process "HandleConnect" callback from Java
		/// </summary>
		void HandleConnect();

		/// <summary>
		/// Process "HandleDisconnect" callback from Java
		/// </summary>
		void HandleDisconnect();

		/// <summary>
		/// Process "HandleRemoteEvent" callback from Java
		/// </summary>
		void HandleRemoteEvent(CacheEvent cacheEvent);
	}
}
