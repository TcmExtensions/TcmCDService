#region Header
////////////////////////////////////////////////////////////////////////////////////
//
//	File Description: RMI Listener
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
using Com.Tridion.Cache;
using TcmCDService.Logging;

namespace TcmCDService.CacheTypes
{
	/// <summary>
	/// <see cref="RMIListener" /> implements <see cref="I:Com.Tridion.Cache.ICacheChannelEventListener" /> in order
	///  to process callbacks from Java through events.
	/// </summary>
	internal class RMIListener : ICacheChannelEventListener
	{
		public System.EventHandler OnConnect;
		public System.EventHandler OnDisconnect;
		public CacheEventHandler OnRemoteEvent;

		/// <summary>
		/// Process "HandleConnect" callback from Java
		/// </summary>
		void ICacheChannelEventListener.HandleConnect()
		{
			if (OnConnect != null)
				OnConnect(this, new System.EventArgs());
		}

		/// <summary>
		/// Process "HandleDisconnect" callback from Java
		/// </summary>
		void ICacheChannelEventListener.HandleDisconnect()
		{
			if (OnDisconnect != null)
				OnDisconnect(this, new System.EventArgs());
		}

		/// <summary>
		/// Process "HandleRemoteEvent" callback from Java
		/// </summary>
		/// <param name="cacheEvent">com.tridion.cache.CacheEvent</param>
		void ICacheChannelEventListener.HandleRemoteEvent(CacheEvent cacheEvent)
		{
			if (OnRemoteEvent != null)
				OnRemoteEvent(this, new CacheEventArgs(cacheEvent.Region, cacheEvent.Key, cacheEvent.EventType));
		}
	}
}
