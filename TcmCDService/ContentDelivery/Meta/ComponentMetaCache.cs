#region Header
////////////////////////////////////////////////////////////////////////////////////
//
//	File Description: Component Meta Cache
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
using System.Linq;
using System.Text;
using TcmCDService.Logging;
using Tridion.ContentDelivery.Meta;

namespace TcmCDService.ContentDelivery.Meta
{
	/// <summary>
	///   <see cref="ComponentMetaCache" /> handles internal caching of Tridion API objects.
	/// </summary>
	/// <remarks>Internally caching the expensive Java CodeMesh proxy classes significantly enhances performance.</remarks>
	internal class ComponentMetaCache : TcmCDService.ContentDelivery.APICache<ComponentMetaFactory, IComponentMeta>
	{
		private static ComponentMetaCache mInstance = new ComponentMetaCache();

		/// <summary>
		/// Creates the cached <see cref="T:Tridion.ContentDelivery.Meta.ComponentMetaFactory" />
		/// </summary>
		/// <param name="publicationId">Publication id as <see cref="T:System.Int32" /></param>
		/// <returns>Cached<see cref="T:Tridion.ContentDelivery.Meta.ComponentMetaFactory" /></returns>
		protected override ComponentMetaFactory CreateCachedItem(int publicationId)
		{
			return new ComponentMetaFactory(publicationId);
		}

		/// <summary>
		/// Retrieves the <see cref="I:Tridion.ContentDelivery.Meta.IComponentMeta" /> for a given component Uri
		/// </summary>
		/// <param name="componentUri">Component <see cref="T:TcmCDService.Tridion.TcmUri" /></param>
		/// <returns><see cref="I:Tridion.ContentDelivery.Meta.IComponentMeta" /></returns>
		internal static IComponentMeta GetComponentMeta(TcmUri componentUri)
		{
			return GetComponentMeta(componentUri.PublicationId, componentUri.ItemId);
		}

		/// <summary>
		/// Retrieves the <see cref="I:Tridion.ContentDelivery.Meta.IComponentMeta" /> for a given component id
		/// </summary>
		/// <param name="publicationId">Publication id as <see cref="T:System.Int32" /></param>
		/// <param name="componentId">Component id as <see cref="T:System.Int32" /></param>
		/// <returns>
		///   <see cref="I:Tridion.ContentDelivery.Meta.IComponentMeta" /> or null
		/// </returns>
		internal static IComponentMeta GetComponentMeta(int publicationId, int componentId)
		{
			return mInstance.ProcessItem(publicationId, (factory) =>
			{
				return factory.GetMeta(componentId);
			});	
		}
	}
}
