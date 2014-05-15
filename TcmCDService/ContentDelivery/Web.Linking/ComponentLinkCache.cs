#region Header
////////////////////////////////////////////////////////////////////////////////////
//
//	File Description: Component Link Cache
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
	///   <see cref="ComponentLinkCache" /> handles internal caching of Tridion API objects.
	/// </summary>
	/// <remarks>Internally caching the expensive Java CodeMesh proxy classes significantly enhances performance.</remarks>
	internal class ComponentLinkCache : TcmCDService.ContentDelivery.APICache<ComponentLink, String> 
	{
		private static ComponentLinkCache mInstance = new ComponentLinkCache();

		/// <summary>
		/// Creates the cached <see cref="T:Tridion.ContentDelivery.Web.Linking.ComponentLink" />
		/// </summary>
		/// <param name="publicationId">Publication id as <see cref="T:System.Int32" /></param>
		/// <returns>Cached<see cref="T:Tridion.ContentDelivery.Web.Linking.ComponentLink" /></returns>
		protected override ComponentLink CreateCachedItem(int publicationId)
		{
			return new ComponentLink(publicationId);
		}

		/// <summary>
		/// Resolves the component link
		/// </summary>
		/// <param name="sourcePageUri">Page <see cref="T:TcmCDService.Tridion.TcmUri" /></param>
		/// <param name="targetComponentUri">Target component <see cref="T:TcmCDService.Tridion.TcmUri" /></param>
		/// <param name="excludeTemplateUri">Excluded template <see cref="T:TcmCDService.Tridion.TcmUri" /></param>
		/// <param name="showAnchor">If <c>true</c>, render the url anchor</param>
		/// <returns>Resolved component url or null</returns>
		internal static String ResolveComponentLink(TcmUri sourcePageUri, TcmUri targetComponentUri, TcmUri excludeTemplateUri, Boolean showAnchor)
		{
			return mInstance.ProcessItem(targetComponentUri.PublicationId, (factory) =>
			{
				Link result = factory.GetLink(sourcePageUri, targetComponentUri, excludeTemplateUri, String.Empty, String.Empty, false, showAnchor);

				return result.IsResolved ? result.Url : null;
			});
		}

		/// <summary>
		/// Resolves the component link
		/// </summary>
		/// <param name="componentUri">Component <see cref="T:TcmCDService.Tridion.TcmUri" /></param>
		/// <returns>Resolved component url or null</returns>
		internal static String ResolveComponentLink(TcmUri componentUri)
		{
			return mInstance.ProcessItem(componentUri.PublicationId, (factory) =>
			{
				Link result = factory.GetLink(componentUri);

				return result.IsResolved ? result.Url : null;
			});
		}
	}
}
