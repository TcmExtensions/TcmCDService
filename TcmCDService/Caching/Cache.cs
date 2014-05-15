#region Header
////////////////////////////////////////////////////////////////////////////////////
//
//	File Description: Cache
// ---------------------------------------------------------------------------------
//	Date Created	: March 30, 2014
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
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Caching;
using TcmCDService.Logging;
using TcmCDService.ContentDelivery;
using TcmCDService.CacheTypes;

namespace TcmCDService.Caching
{
	/// <summary>
	/// <see cref="Cache" /> leverages the <see cref="T:System.Runtime.Caching.MemoryCache" /> with <see cref="T:TcmCDService.Cache.TridionChangeMonitor" />
	/// to cache content delivery API results.
	/// </summary>
	public static class Cache
	{
		private static int mExpiration;
		private static List<TridionDependency> mDependencies = new List<TridionDependency>();

		private static void ItemRemovedCallback(CacheEntryRemovedArguments arguments)
		{
			Logger.Debug("Item {0} removed from cache.", arguments.CacheItem.Key);
		}

		/// <summary>
		/// Gets or sets cache expiration in minutes
		/// </summary>
		/// <value>
		/// Cache item expiration in minutes
		/// </value>
		/// <remarks>0 means no caching</remarks>
		public static int Expiration
		{
			get
			{
				return mExpiration;
			}
			set
			{
				mExpiration = value;
			}
		}

		/// <summary>
		/// Processes a cache event and trigger the dependent dependencies
		/// </summary>
		/// <param name="cacheRegion"><see cref="T:TcmCDService.CacheClient.CacheRegion" /></param>
		/// <param name="cacheEventType"><see cref="T:TcmCDService.CacheClient.CacheEventType" /></param>
		/// <param name="key">Cache item key</param>
		public static void ProcessCacheEvent(CacheRegion cacheRegion, CacheEventType cacheEventType, String key)
		{
			Logger.Debug("Processing cache event: region \"{0}\", key \"{1}\", type \"{2}\".", cacheRegion, key, cacheEventType);

			foreach (TridionDependency dependency in mDependencies)
			{
				if (dependency.CacheRegion.HasFlag(cacheRegion))
				{
					Logger.Debug("\tDependency: {0}", dependency);
					Logger.Debug("\tComparison: {0} vs {1}", dependency.Key, key);

					switch (cacheEventType)
					{
						// Instruction to flush all entries from a CacheRegion
						case CacheEventType.Flush:
							dependency.TriggerDependency();
							break;
						// Instruction to invalidate a specific entry from a CacheRegion
						case CacheEventType.Invalidate:
							if (key.StartsWith(dependency.Key, StringComparison.OrdinalIgnoreCase))
								dependency.TriggerDependency();
							break;
					}
				}
			}
		}

		/// <summary>
		/// Gets the specified key.
		/// </summary>
		/// <typeparam name="T">Cache item type</typeparam>
		/// <param name="key">Cache item key</param>
		/// <param name="valueInitializer">Cache item value initializer.</param>
		/// <returns>Cached item or newly created item</returns>
		public static T Get<T>(String key, Func<T> valueInitializer, CacheRegion cacheRegion, TcmUri cacheUri) where T : class
		{
			// No caching is applied
			if (mExpiration == -1)
				return valueInitializer();

			MemoryCache cache = MemoryCache.Default;

			// Retrieve the value from the cache as the generic requested type
			var value = cache.Get(key);

			if (value != null)
			{
				T cachedItem = value as T;

				if (cachedItem != null)
					return cachedItem;

				return default(T);
			}

			// Initialize the value if its not existing in the cache
			if (valueInitializer != null)
			{
				T initializedValue = valueInitializer();

				// Store the item in cache only if it returned an actual value
				if (!EqualityComparer<T>.Default.Equals(initializedValue, default(T)))
				{
					CacheItemPolicy cacheItemPolicy = new CacheItemPolicy()
					{
						AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(Expiration),
						RemovedCallback = new CacheEntryRemovedCallback(ItemRemovedCallback)
					};

					if (cacheRegion != CacheRegion.Unknown)
					{
						String cacheKey = cacheUri.ToCacheKey();

						TridionDependency dependency = mDependencies.FirstOrDefault(d => d.CacheRegion == cacheRegion && d.Key == cacheKey);

						// Dependency did not exist yet, create it and add it to the cache dependencies list
						if (dependency == null)
						{
							dependency = new TridionDependency(cacheRegion, cacheKey);
							mDependencies.Add(dependency);
						}

						cacheItemPolicy.ChangeMonitors.Add(new TridionChangeMonitor(dependency));
					}

					cache.Set(key, initializedValue, cacheItemPolicy);
					return initializedValue;
				}
			}

			return default(T);
		}
	}
}
