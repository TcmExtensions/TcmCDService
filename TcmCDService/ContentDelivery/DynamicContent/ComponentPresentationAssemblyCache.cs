#region Header
////////////////////////////////////////////////////////////////////////////////////
//
//	File Description: Component Presentation Assembly Cache
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
	///   <see cref="ComponentPresentationAssemblyCache" /> handles internal caching of Tridion API objects.
	/// </summary>
	/// <remarks>Internally caching the expensive Java CodeMesh proxy classes significantly enhances performance.</remarks>
	internal class ComponentPresentationAssemblyCache : TcmCDService.ContentDelivery.APICache<ComponentPresentationAssembler, String>
	{
		private static ComponentPresentationAssemblyCache mInstance = new ComponentPresentationAssemblyCache();

		/// <summary>
		/// Creates the cached <see cref="T:Tridion.ContentDelivery.DynamicContent.ComponentPresentationAssembler" />
		/// </summary>
		/// <param name="publicationId">Publication id as <see cref="T:System.Int32" /></param>
		/// <returns>Cached<see cref="T:Tridion.ContentDelivery.DynamicContent.ComponentPresentationAssembler" /></returns>
		protected override ComponentPresentationAssembler CreateCachedItem(int publicationId)
		{
			return new ComponentPresentationAssembler();
		}
		
		/// <summary>
		/// Gets the assembled component presentation
		/// </summary>
		/// <param name="componentUri">Component <see cref="T:TcmCDService.Tridion.TcmUri" /></param>
		/// <param name="templateUri">Component template <see cref="T:TcmCDService.Tridion.TcmUri" /></param>
		/// <returns>Assembled component presentation as <see cref="T:System.String" /></returns>
		internal static String AssembleComponentPresentation(TcmUri componentUri, TcmUri templateUri)
		{
			return mInstance.ProcessItem(0, (factory) =>
				{
					return factory.GetContent(componentUri, templateUri);
				});			
		}
	}
}
