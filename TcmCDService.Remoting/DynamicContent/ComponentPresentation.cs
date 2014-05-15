#region Header
////////////////////////////////////////////////////////////////////////////////////
//
//	File Description: Component Presentation
// ---------------------------------------------------------------------------------
//	Date Created	: April 6, 2014
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
using TcmCDService.Contracts;

namespace TcmCDService.Remoting.DynamicContent
{
	/// <summary>
	///   <see cref="ComponentPresentation" /> connects to TcmCDService to provide Tridion component presentation retrieval
	/// </summary>
	public static class ComponentPresentation
	{
		/// <summary>
		/// Gets the <see cref="T:TcmCDService.Contracts.ComponentPresentation" /> with highest priority.
		/// </summary>
		/// <param name="componentUri">Component uri</param>
		/// <returns>
		///   <see cref="T:TcmCDService.Contracts.ComponentPresentation" />
		/// </returns>		
		public static Contracts.ComponentPresentation GetHighestPriority(String componentUri)
		{			
			return RemoteAPI.Execute<Contracts.ComponentPresentation>((client) =>
				client.Service.ComponentPresentationWithHighestPriority(componentUri), 
				componentUri);
		}

		/// <summary>
		/// Gets the <see cref="T:TcmCDService.Contracts.ComponentPresentation" /> with highest priority.
		/// </summary>
		/// <param name="publicationId">Publication id as <see cref="T:System.Int32" /></param>
		/// <param name="componentId">Component id as <see cref="T:System.Int32" /></param>
		/// <returns>
		///   <see cref="T:TcmCDService.Contracts.ComponentPresentation" />
		/// </returns>		
		public static Contracts.ComponentPresentation GetHighestPriority(int publicationId, int componentId)
		{
			return RemoteAPI.Execute<Contracts.ComponentPresentation>((client) =>
				client.Service.ComponentPresentationWithHighestPriority(publicationId, componentId),
				publicationId, componentId);
		}

		/// <summary>
		/// Gets the <see cref="T:TcmCDService.Contracts.ComponentPresentation" /> with the specified templateUri
		/// </summary>
		/// <param name="componentUri">Component uri</param>
		/// <param name="templateUri">Component template uri</param>
		/// <returns>
		///   <see cref="T:TcmCDService.Contracts.ComponentPresentation" />
		/// </returns>
		public static Contracts.ComponentPresentation Get(String componentUri, String templateUri)
		{
			return RemoteAPI.Execute<Contracts.ComponentPresentation>((client) =>
				client.Service.ComponentPresentation(componentUri, templateUri),
				componentUri, templateUri);
		}

		/// <summary>
		/// Gets the <see cref="T:TcmCDService.Contracts.ComponentPresentation" /> with the specified template id
		/// </summary>
		/// <param name="componentUri">Component uri</param>
		/// <param name="templateId">Component template id as <see cref="T:System.Int32" /></param>
		/// <returns>
		///   <see cref="T:TcmCDService.Contracts.ComponentPresentation" />
		/// </returns>		
		public static Contracts.ComponentPresentation Get(String componentUri, int templateId)
		{
			return RemoteAPI.Execute<Contracts.ComponentPresentation>((client) =>
				client.Service.ComponentPresentation(componentUri, templateId),
				componentUri);
		}

		/// <summary>
		/// Gets the <see cref="T:TcmCDService.Contracts.ComponentPresentation" /> with the specified templateUri
		/// </summary>
		/// <param name="publicationId">Publication id as <see cref="T:System.Int32" /></param>
		/// <param name="componentId">Component id as <see cref="T:System.Int32" /></param>
		/// <param name="templateId">Component template id as <see cref="T:System.Int32" /></param>
		/// <returns>
		///   <see cref="T:TcmCDService.Contracts.ComponentPresentation" /> or null
		/// </returns>
		public static Contracts.ComponentPresentation Get(int publicationId, int componentId, int templateId)
		{
			return RemoteAPI.Execute<Contracts.ComponentPresentation>((client) =>
				client.Service.ComponentPresentation(publicationId, componentId, templateId),
				publicationId, componentId, templateId);
		}
	}
}
