#region Header
////////////////////////////////////////////////////////////////////////////////////
//
//	File Description: RMI Cache
// ---------------------------------------------------------------------------------
//	Date Created	: April 14, 2014
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
using System.Configuration;
using Codemesh.JuggerNET;
using Com.Tridion.Cache;
using TcmCDService.Configuration;
using TcmCDService.Logging;

namespace TcmCDService.CacheTypes
{
	/// <summary>
	/// <see cref="RMICache" /> exposes a <see cref="T:TcmCDService.CacheTypes.CacheType" /> using Java Remove Method Invocation to register with the Tridion 
	/// Cache Channel service in order to invalidate cache items.
	/// </summary>
	/// <remarks>This class uses a CodeMesh JuggerNET wrapper to leverage the existing Tridion JuggerNET implementation to use com.tridion.cache.RMICacheChannelListener from .NET</remarks>
	public class RMICache : TcmCDService.CacheTypes.CacheType
	{
		private RMICacheChannelConnector mConnector;
		private bool mIsClosed = false;

		/// <summary>
		/// Gets the unique client identifier of this <see cref="RMICache" />
		/// </summary>
		/// <value>
		/// The unique client identifier of this <see cref="RMICache" />
		/// </value>
		public override String Identifier
		{
			get
			{
				return mConnector.Identifier;
			}
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

		/// <summary>
		/// Initializes a new instance of the <see cref="RMICache"/> class.
		/// </summary>
		/// <param name="settings"><see cref="T:TcmCDService.Configuration.Settings" /></param>
		public RMICache(Settings settings): base(settings)
		{
			String host = settings.Get<String>("host");

			if (String.IsNullOrEmpty(host))
				throw new ConfigurationErrorsException("RMICache: Host is unconfigured.");

			int port = settings.Get<int>("port");

			if (port == 0)
				throw new ConfigurationErrorsException("RMICache: Port is unconfigured.");

			String instanceIdentifier = settings.Get<String>("instanceIdentifier");

			try
			{
				CacheRegionExtensions.FromCacheRegion(CacheRegion.Publication);

				mConnector = new Com.Tridion.Cache.RMICacheChannelConnector(host, port, instanceIdentifier);

				Logger.Info("RMICache: {0}", mConnector.Identifier);

				RMIListener listener = new RMIListener();

				listener.OnConnect += (sender, args) => { base.OnConnected(); };
				listener.OnDisconnect += (sender, args) => { base.OnDisconnected(); };
				listener.OnRemoteEvent += (sender, args) => { base.OnCacheEvent(args.Region, args.Key, args.EventType); };

				// Assign a listener interface
				mConnector.Listener = listener;
			}
			catch (Exception ex)
			{
				Logger.Error("RMICache", ex);
			}
		}

		/// <summary>
		/// Instruct this <see cref="RMICache" /> to connect to the remote cache system if required.
		/// </summary>
		public override void Connect()
		{
			if (!mIsClosed)
				mConnector.Validate();
		}

		/// <summary>
		/// Instruct this <see cref="RMICache" /> to disconnect from a remote cache system if required.
		/// </summary>
		public override void Disconnect()
		{
			if (!mIsClosed && mConnector != null)
			{
				mIsClosed = true;

				mConnector.Listener = null;
				mConnector.Close();
			}
		}

		/// <summary>
		/// Broadcasts a cache event to all other connected clients
		/// </summary>
		/// <param name="cacheRegion"><see cref="T:TcmCDService.CacheTypes.CacheRegion" /></param>
		/// <param name="key">Cache key as <see cref="T:System.String" /></param>
		/// <param name="eventType"><see cref="T:TcmCDService.CacheTypes.CacheEventType" /></param>
		public override void BroadcastEvent(CacheRegion cacheRegion, String key, CacheEventType eventType)
		{
			mConnector.BroadcastEvent(new CacheEvent(CacheRegionExtensions.FromCacheRegion(cacheRegion), key, eventType));
		}

		/// <summary>
		/// Broadcasts a cache event to all other connected clients
		/// </summary>
		/// <param name="cacheRegion"><see cref="T:TcmCDService.CacheTypes.CacheRegion" /></param>
		/// <param name="key">Cache key as <see cref="T:System.Int32" /></param>
		/// <param name="eventType"><see cref="T:TcmCDService.CacheTypes.CacheEventType" /></param>
		public override void BroadcastEvent(CacheRegion cacheRegion, int key, CacheEventType eventType)
		{
			mConnector.BroadcastEvent(new CacheEvent(CacheRegionExtensions.FromCacheRegion(cacheRegion), key, eventType));
		}

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources.
		/// </summary>
		/// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
		protected override void Dispose(Boolean disposing)
		{
			base.Dispose(disposing);

			if (disposing)
			{
				try
				{
					Java.Rmi.Server.UnicastRemoteObject.UnexportObject(mConnector, true);
				}
				catch (Exception ex)
				{
					Logger.Error("RMICache", ex);
				}
			}
		}
	}
}
