#region Header
////////////////////////////////////////////////////////////////////////////////////
//
//	File Description: Binary Link Cache
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
	///   <see cref="BinaryLinkCache" /> handles internal caching of Tridion API objects.
	/// </summary>
	/// <remarks>Internally caching the expensive Java CodeMesh proxy classes significantly enhances performance.</remarks>
	internal class BinaryLinkCache : TcmCDService.ContentDelivery.APICache<BinaryLink, String> 
	{
		private static BinaryLinkCache mInstance = new BinaryLinkCache();

		/// <summary>
		/// Creates the cached <see cref="T:Tridion.ContentDelivery.Web.Linking.BinaryLink" />
		/// </summary>
		/// <param name="publicationId">Publication id as <see cref="T:System.Int32" /></param>
		/// <returns>Cached<see cref="T:Tridion.ContentDelivery.Web.Linking.BinaryLink" /></returns>
		protected override BinaryLink CreateCachedItem(int publicationId)
		{
			return new BinaryLink(publicationId);
		}

		/// <summary>
		/// Resolves the binary link
		/// </summary>
		/// <param name="binaryComponentUri">Binary component <see cref="T:TcmCDService.Tridion.TcmUri" /></param>
		/// <param name="variantId">Binary variant id</param>
		/// <param name="anchor">Link anchor</param>
		/// <returns>Resolved binary url or null</returns>
		internal static String ResolveBinaryLink(TcmUri binaryComponentUri, String variantId, String anchor)
		{
			return mInstance.ProcessItem(binaryComponentUri.PublicationId, (factory) =>
			{
				Link result = factory.GetLink(binaryComponentUri, variantId, anchor, String.Empty, String.Empty, false);

				return result.IsResolved ? result.Url : null;
			});			
		}
	}
}
