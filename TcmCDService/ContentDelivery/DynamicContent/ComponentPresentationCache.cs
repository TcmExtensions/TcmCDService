#region Header
////////////////////////////////////////////////////////////////////////////////////
//
//	File Description: Component Presentation Cache
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
	///   <see cref="ComponentPresentationCache" /> handles internal caching of Tridion API objects.
	/// </summary>
	/// <remarks>Internally caching the expensive Java CodeMesh proxy classes significantly enhances performance.</remarks>
	internal class ComponentPresentationCache : TcmCDService.ContentDelivery.APICache<ComponentPresentationFactory, ComponentPresentation>
	{
		private static ComponentPresentationCache mInstance = new ComponentPresentationCache();

		/// <summary>
		/// Creates the cached <see cref="T:Tridion.ContentDelivery.DynamicContent.ComponentPresentationFactory" />
		/// </summary>
		/// <param name="publicationId">Publication id as <see cref="T:System.Int32" /></param>
		/// <returns>Cached<see cref="T:Tridion.ContentDelivery.DynamicContent.ComponentPresentationFactory" /></returns>
		protected override ComponentPresentationFactory CreateCachedItem(int publicationId)
		{
			return new ComponentPresentationFactory(publicationId);
		}
		
		/// <summary>
		/// Gets the <see cref="T:Tridion.ContentDelivery.DynamicContent.ComponentPresentation" /> with highest priority.
		/// </summary>
		/// <param name="componentUri">Component <see cref="T:TcmCDService.Tridion.TcmUri" /></param>
		/// <returns><see cref="T:Tridion.ContentDelivery.DynamicContent.ComponentPresentation" /></returns>
		internal static ComponentPresentation GetComponentPresentationWithHighestPriority(TcmUri componentUri)
		{
			return GetComponentPresentationWithHighestPriority(componentUri.PublicationId, componentUri.ItemId);
		}

		/// <summary>
		/// Gets the <see cref="T:Tridion.ContentDelivery.DynamicContent.ComponentPresentation" /> with highest priority.
		/// </summary>
		/// <param name="publicationId">Publication id as <see cref="T:System.Int32" /></param>
		/// <param name="componentId">Component id as <see cref="T:System.Int32" /></param>
		/// <returns>
		///   <see cref="T:Tridion.ContentDelivery.DynamicContent.ComponentPresentation" />
		/// </returns>
		internal static ComponentPresentation GetComponentPresentationWithHighestPriority(int publicationId, int componentId)
		{
			return mInstance.ProcessItem(publicationId, (factory) =>
				{
					return factory.GetComponentPresentationWithHighestPriority(componentId);
				});			
		}

		/// <summary>
		/// Gets the <see cref="T:Tridion.ContentDelivery.DynamicContent.ComponentPresentation" /> with the specified templateUri
		/// </summary>
		/// <param name="componentUri">Component <see cref="T:TcmCDService.Tridion.TcmUri" /></param>
		/// <param name="templateUri">Component template <see cref="T:TcmCDService.Tridion.TcmUri" /></param>
		/// <returns>
		///   <see cref="T:Tridion.ContentDelivery.DynamicContent.ComponentPresentation" />
		/// </returns>
		internal static ComponentPresentation GetComponentPresentation(TcmUri componentUri, TcmUri templateUri)
		{
			return GetComponentPresentation(componentUri.PublicationId, componentUri.ItemId, templateUri.ItemId);
		}

		/// <summary>
		/// Gets the <see cref="T:Tridion.ContentDelivery.DynamicContent.ComponentPresentation" /> with the specified templateUri
		/// </summary>		
		/// <param name="componentUri">Component <see cref="T:TcmCDService.Tridion.TcmUri" /></param>
		/// <param name="templateId">Component template id as <see cref="T:System.Int32" /></param>
		/// <returns>
		///   <see cref="T:Tridion.ContentDelivery.DynamicContent.ComponentPresentation" />
		/// </returns>
		internal static ComponentPresentation GetComponentPresentation(TcmUri componentUri, int templateId)
		{
			return GetComponentPresentation(componentUri.PublicationId, componentUri.ItemId, templateId);
		}

		/// <summary>
		/// Gets the <see cref="T:Tridion.ContentDelivery.DynamicContent.ComponentPresentation" /> with the specified templateUri
		/// </summary>
		/// <param name="publicationId">Publication id as <see cref="T:System.Int32" /></param>
		/// <param name="componentId">Component id as <see cref="T:System.Int32" /></param>
		/// <param name="templateId">Component template id as <see cref="T:System.Int32" /></param>
		/// <returns>
		///   <see cref="T:Tridion.ContentDelivery.DynamicContent.ComponentPresentation" /> or null
		/// </returns>
		internal static ComponentPresentation GetComponentPresentation(int publicationId, int componentId, int templateId)
		{
			return mInstance.ProcessItem(publicationId, (factory) =>
				{
					return factory.GetComponentPresentation(componentId, templateId);
				});	
		}	
	}
}
