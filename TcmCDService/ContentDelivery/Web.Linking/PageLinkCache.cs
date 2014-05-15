#region Header
////////////////////////////////////////////////////////////////////////////////////
//
//	File Description: Page Link Cache
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
using Tridion.ContentDelivery.Web.Linking;

namespace TcmCDService.ContentDelivery.Web.Linking
{
	/// <summary>
	///   <see cref="PageLinkCache" /> handles internal caching of Tridion API objects.
	/// </summary>
	/// <remarks>Internally caching the expensive Java CodeMesh proxy classes significantly enhances performance.</remarks>
	internal class PageLinkCache : TcmCDService.ContentDelivery.APICache<PageLink, String> 
	{
		private static PageLinkCache mInstance = new PageLinkCache();

		/// <summary>
		/// Creates the cached <see cref="T:Tridion.ContentDelivery.Web.Linking.PageLink" />
		/// </summary>
		/// <param name="publicationId">Publication id as <see cref="T:System.Int32" /></param>
		/// <returns>Cached<see cref="T:Tridion.ContentDelivery.Web.Linking.PageLink" /></returns>
		protected override PageLink CreateCachedItem(int publicationId)
		{
			return new PageLink(publicationId);
		}

		/// <summary>
		/// Resolves the page link
		/// </summary>
		/// <param name="targetPageUri">Page <see cref="T:TcmCDService.Tridion.TcmUri" /></param>
		/// <param name="anchor">Link anchor</param>
		/// <param name="parameters">Link parameters</param>
		/// <returns>Resolved page link or null</returns>
		internal static String ResolvePageLink(TcmUri targetPageUri, String anchor, String parameters)
		{
			return mInstance.ProcessItem(targetPageUri.PublicationId, (factory) =>
			{
				Link result = factory.GetLink(targetPageUri, anchor, String.Empty, String.Empty, false, parameters);

				return result.IsResolved ? result.Url : null;
			});
		}

		/// <summary>
		/// Resolves the page link
		/// </summary>
		/// <param name="targetPageUri">Page <see cref="T:TcmCDService.Tridion.TcmUri" /></param>
		/// <returns>Resolved page link or null</returns>
		internal static String ResolvePageLink(TcmUri targetPageUri)
		{
			return mInstance.ProcessItem(targetPageUri.PublicationId, (factory) =>
			{
				Link result = factory.GetLink(targetPageUri);

				return result.IsResolved ? result.Url : null;
			});
		}
	}
}
