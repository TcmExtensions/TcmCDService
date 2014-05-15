#region Header
////////////////////////////////////////////////////////////////////////////////////
//
//	File Description: Cache Type
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
using TcmCDService.Logging;

namespace TcmCDService.CacheTypes
{
	/// <summary>
	/// <see cref="CacheType" /> implements an abstract base class for CacheType plugin implementations
	/// </summary>
	public abstract class CacheType : IDisposable
	{
		public event EventHandler Connected;
		public event EventHandler Disconnected;

		public event CacheEventHandler CacheEvent;

		/// <summary>
		/// Trigger a Connected event
		/// </summary>
		protected void OnConnected()
		{
			if (Connected != null)
				Connected(null, new EventArgs());
		}

		/// <summary>
		/// Trigger a Disconnected event
		/// </summary>
		protected void OnDisconnected()
		{
			if (Disconnected != null)
				Disconnected(null, new EventArgs());			
		}

		/// <summary>
		/// Trigger a CacheEvent for the <see cref="P:region" />, <see cref="P:key" /> and <see cref="P:eventType" />
		/// </summary>
		/// <param name="region"><see cref="T:TcmCDService.CacheClient.CacheRegion" /></param>
		/// <param name="key">Cache item key</param>
		/// <param name="eventType"><see cref="T:TcmCDService.CacheClient.CacheEventType" /></param>
		protected void OnCacheEvent(CacheRegion region, String key, CacheEventType eventType)
		{
			if (CacheEvent != null)
				CacheEvent(null, new CacheEventArgs(region, key, eventType));
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CacheType"/> class.
		/// </summary>
		/// <param name="settings"><see cref="T:TcmCDService.Configuration.Settings" /></param>
		protected CacheType(Settings settings)
		{
		}

		/// <summary>
		/// Returns the configured <see cref="CacheType" /> or the default <see cref="T:TcmCDService.CacheTypes.NullCache" />
		/// </summary>
		/// <returns></returns>
		public static CacheType GetCacheType()
		{
			if (Config.Instance != null)
			{
				try
				{
					CacheTypeElement cacheTypeElement = Config.Instance.CacheType;

					Type cacheType = Type.GetType(cacheTypeElement.CacheType);

					if (cacheType != null)
						return (CacheType)Activator.CreateInstance(cacheType, new Settings(cacheTypeElement.Settings));
				}
				catch (Exception ex)
				{
					Logger.Error("Error initializing CacheType", ex);
				}
			}

			// Default to standard NullCache
			return new NullCache(null);
		}

		/// <summary>
		/// Gets the unique client identifier of this <see cref="CacheType" />
		/// </summary>
		/// <value>
		/// The unique client identifier of this <see cref="CacheType" />
		/// </value>
		public virtual String Identifier
		{
			get
			{
				return this.GetType().Name;
			}
		}

		/// <summary>
		/// Gets the expiration of cache items in minutes
		/// </summary>
		/// <value>
		/// Expiration of cache items in minutes
		/// </value>
		/// <remarks>-1 means no caching applies</remarks>
		public virtual int Expiration
		{
			get
			{
				return Config.Instance.DefaultCacheExpiry;
			}
		}

		/// <summary>
		/// Instruct this <see cref="CacheType" /> to connect to a remote cache system if required.
		/// </summary>
		public virtual void Connect()
		{
		}

		/// <summary>
		/// Broadcasts a cache event to all other connected clients
		/// </summary>
		/// <param name="cacheRegion"><see cref="T:TcmCDService.CacheTypes.CacheRegion" /></param>
		/// <param name="key">Cache key as <see cref="T:System.String" /></param>
		/// <param name="eventType"><see cref="T:TcmCDService.CacheTypes.CacheEventType" /></param>
		public virtual void BroadcastEvent(CacheRegion cacheRegion, String key, CacheEventType eventType)
		{
		}

		/// <summary>
		/// Broadcasts a cache event to all other connected clients
		/// </summary>
		/// <param name="cacheRegion"><see cref="T:TcmCDService.CacheTypes.CacheRegion" /></param>
		/// <param name="key">Cache key as <see cref="T:System.Int32" /></param>
		/// <param name="eventType"><see cref="T:TcmCDService.CacheTypes.CacheEventType" /></param>
		public virtual void BroadcastEvent(CacheRegion cacheRegion, int key, CacheEventType eventType)
		{
		}

		/// <summary>
		/// Instruct this <see cref="CacheType" /> to disconnect from a remote cache system if required.
		/// </summary>
		public virtual void Disconnect()
		{
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources.
		/// </summary>
		/// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
		protected virtual void Dispose(Boolean disposing)
		{
			if (disposing)
			{
				Disconnect();
			}
		}
	}
}
