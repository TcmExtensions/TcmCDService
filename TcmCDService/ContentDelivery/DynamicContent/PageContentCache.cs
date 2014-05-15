#region Header
////////////////////////////////////////////////////////////////////////////////////
//
//	File Description: Page Content Cache
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
using Tridion.ContentDelivery.DynamicContent;

namespace TcmCDService.ContentDelivery.DynamicContent
{
	/// <summary>
	///   <see cref="PageContentCache" /> handles internal caching of Tridion API objects.
	/// </summary>
	/// <remarks>Internally caching the expensive Java CodeMesh proxy classes significantly enhances performance.</remarks>
	internal class PageContentCache : TcmCDService.ContentDelivery.APICache<PageContentFactory, CharacterData>
	{
		private static PageContentCache mInstance = new PageContentCache();

		/// <summary>
		/// Creates the cached <see cref="T:Tridion.ContentDelivery.DynamicContent.PageContentFactory" />
		/// </summary>
		/// <param name="publicationId">Publication id as <see cref="T:System.Int32" /></param>
		/// <returns>Cached<see cref="T:Tridion.ContentDelivery.DynamicContent.PageContentFactory" /></returns>
		protected override PageContentFactory CreateCachedItem(int publicationId)
		{
			return new PageContentFactory();
		}
		
		/// <summary>
		/// Retrieves <see cref="T:Tridion.ContentDelivery.DynamicContent.CharacterData" />
		/// </summary>
		/// <param name="pageUri">Binary <see cref="T:TcmCDService.Tridion.TcmUri" /></param>
		/// <returns>
		///   <see cref="T:Tridion.ContentDelivery.DynamicContent.CharacterData" /> or null
		/// </returns>
		internal static CharacterData GetPageContent(TcmUri pageUri)
		{
			return GetPageContent(pageUri.PublicationId, pageUri.ItemId);
		}

		/// <summary>
		/// Retrieves <see cref="T:Tridion.ContentDelivery.DynamicContent.CharacterData" />
		/// </summary>
		/// <param name="publicationId">Publication id as <see cref="T:System.Int32" /></param>
		/// <param name="pageId">Page id as <see cref="T:System.Int32" /></param>
		/// <returns>
		///   <see cref="T:Tridion.ContentDelivery.DynamicContent.CharacterData" /> or null
		/// </returns>
		internal static CharacterData GetPageContent(int publicationId, int pageId)
		{
			return mInstance.ProcessItem(0, (factory) =>
				{
					return factory.GetPageContent(publicationId, pageId);
				});
		}
	}
}
