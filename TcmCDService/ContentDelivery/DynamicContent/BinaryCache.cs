#region Header
////////////////////////////////////////////////////////////////////////////////////
//
//	File Description: Binary Data Cache
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
	///   <see cref="BinaryCache" /> handles internal caching of Tridion API objects.
	/// </summary>
	/// <remarks>Internally caching the expensive Java CodeMesh proxy classes significantly enhances performance.</remarks>
	internal class BinaryCache : TcmCDService.ContentDelivery.APICache<BinaryFactory, BinaryData>
	{
		private static BinaryCache mInstance = new BinaryCache();

		/// <summary>
		/// Creates the cached <see cref="T:Tridion.ContentDelivery.DynamicContent.BinaryFactory" />
		/// </summary>
		/// <param name="publicationId">Publication id as <see cref="T:System.Int32" /></param>
		/// <returns>Cached<see cref="T:Tridion.ContentDelivery.DynamicContent.BinaryFactory" /></returns>
		protected override BinaryFactory CreateCachedItem(int publicationId)
		{
			return new BinaryFactory();
		}
		
		/// <summary>
		/// Retrieves <see cref="T:Tridion.ContentDelivery.DynamicContent.BinaryData" />
		/// </summary>
		/// <param name="binaryUri">Binary <see cref="T:TcmCDService.Tridion.TcmUri" /></param>
		/// <returns>
		///   <see cref="T:Tridion.ContentDelivery.DynamicContent.BinaryData" /> or null
		/// </returns>
		internal static BinaryData GetBinaryData(TcmUri binaryUri)
		{
			return GetBinaryData(binaryUri.PublicationId, binaryUri.ItemId);
		}

		/// <summary>
		/// Retrieves <see cref="T:Tridion.ContentDelivery.DynamicContent.BinaryData" />
		/// </summary>
		/// <param name="binaryUri">Binary <see cref="T:TcmCDService.Tridion.TcmUri" /></param>
		/// <param name="variantId">Binary variantId as <see cref="T:System.String" /></param>
		/// <returns>
		///   <see cref="T:Tridion.ContentDelivery.DynamicContent.BinaryData" /> or null
		/// </returns>
		internal static BinaryData GetBinaryData(TcmUri binaryUri, String variantId)
		{
			return GetBinaryData(binaryUri.PublicationId, binaryUri.ItemId, variantId);
		}

		/// <summary>
		/// Retrieves <see cref="T:Tridion.ContentDelivery.DynamicContent.BinaryData" />
		/// </summary>
		/// <param name="publicationId">Publication id as <see cref="T:System.Int32" /></param>
		/// <param name="binaryId">Binary Id id as <see cref="T:System.Int32" /></param>
		/// <returns>
		///   <see cref="T:Tridion.ContentDelivery.DynamicContent.BinaryData" /> or null
		/// </returns>
		internal static BinaryData GetBinaryData(int publicationId, int binaryId)
		{
			return mInstance.ProcessItem(0, (factory) =>
				{
					return factory.GetBinary(publicationId, binaryId);
				});
		}	

		/// <summary>
		/// Retrieves <see cref="T:Tridion.ContentDelivery.DynamicContent.BinaryData" />
		/// </summary>
		/// <param name="publicationId">Publication id as <see cref="T:System.Int32" /></param>
		/// <param name="binaryId">Binary Id id as <see cref="T:System.Int32" /></param>
		/// <param name="variantId">Binary variant id as <see cref="T:System.String" /></param>
		/// <returns>
		///   <see cref="T:Tridion.ContentDelivery.DynamicContent.BinaryData" /> or null
		/// </returns>
		internal static BinaryData GetBinaryData(int publicationId, int binaryId, String variantId)
		{
			return mInstance.ProcessItem(0, (factory) =>
				{
					return factory.GetBinary(publicationId, binaryId, variantId);
				});	
		}	
	}
}
