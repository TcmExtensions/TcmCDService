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
using System.Collections.Concurrent;
using System.Collections.Generic;
using TcmCDService.Logging;
using Tridion.ContentDelivery.DynamicContent;
using Tridion.ContentDelivery.Meta;
using Tridion.ContentDelivery.Taxonomies;
using Tridion.ContentDelivery.Web.Linking;

namespace TcmCDService.ContentDelivery
{
	/// <summary>
	///   <see cref="APICache"/> handles internal caching of Tridion API objects.
	/// </summary>
	/// <remarks>Internally caching the expensive Java CodeMesh proxy classes significantly enhances performance.</remarks>
	internal abstract class APICache<T, I> where T : class, IDisposable where I : class
	{
		// Number of times to retry (and recreate) interaction with the Java CodeMesh proxy class
		private const int NUM_RETRIES = 1;

		private ConcurrentDictionary<int, T> mCache;

		/// <summary>
		/// Instruct a derived class to created a item of type T
		/// </summary>
		/// <param name="publicationId">Publication id as <see cref="T:System.Int32" /></param>
		/// <returns>Item of type T</returns>
		protected abstract T CreateCachedItem(int publicationId);

		/// <summary>
		/// Initializes a new instance of the <see cref="APICache{T, I}"/> class.
		/// </summary>
		protected APICache()
		{
			mCache = new ConcurrentDictionary<int, T>();
		}

		/// <summary>
		/// Retrieves a cached item from the <see cref="APICache{T}" />
		/// </summary>
		/// <param name="publicationId">Publication id as <see cref="T:System.Int32" /></param>
		/// <returns>
		///   Cached Item
		/// </returns>
		private T CachedItem(int publicationId)
		{
			T result;

			// Return cached item if available
			if (mCache.TryGetValue(publicationId, out result) && result != null)
				return result;

			// Cached item did not exist, create a new item
			result = CreateCachedItem(publicationId);
			mCache[publicationId] = result;

			return result;
		}

		/// <summary>
		/// Initializes a <see cref="P:processItem" /> callback against the given cached item identified be <see cref="P:publicationId" />
		/// </summary>
		/// <typeparam name="I">Dynamic function output item type</typeparam>
		/// <param name="publicationId">Publication id as <see cref="T:System.Int32" /></param>
		/// <param name="processItem">Dynamic function to process an item.</param>
		/// <returns>Dynamic function result or null</returns>
		protected X ProcessItem<X>(int publicationId, Func<T, X> processItem) where X : class
		{
			// Possibly recover if an exception was generated, for example the Java proxy "disconnected".
			for (int i = 0; i <= NUM_RETRIES; i++)
			{
				T cachedItem = null;

				try
				{
					cachedItem = CachedItem(publicationId);

					// Allow the derived cache class to interact with the broker API
					return processItem(cachedItem);
				}
				catch (Exception ex)
				{
					Logger.Error("APICache.ProcessItem", ex);

					if (cachedItem != null)
						cachedItem.Dispose();

					// Ensure we remove possible invalid objects from the local cache, so they get re-instantiated
					mCache[publicationId] = null;
				}
			}

			return null;
		}

		/// <summary>
		/// Initializes a <see cref="P:processItem" /> callback against the given cached item identified be <see cref="P:publicationId" />
		/// </summary>
		/// <param name="publicationId">Publication id as <see cref="T:System.Int32" /></param>
		/// <param name="processItem">Dynamic function to process an item.</param>
		/// <returns>Dynamic function result or null</returns>
		protected I ProcessItem(int publicationId, Func<T, I> processItem)
		{
			return ProcessItem<I>(publicationId, processItem);
		}
	}
}
