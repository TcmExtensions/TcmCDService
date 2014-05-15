#region Header
////////////////////////////////////////////////////////////////////////////////////
//
//	File Description: Component Presentation Assembly
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
	///   <see cref="ComponentPresentationAssembly" /> connects to TcmCDService to provide Tridion component presentation assembly
	/// </summary>
	public static class ComponentPresentationAssembly
	{
		/// <summary>
		/// Assembles the component presentation with the given componentUri and componentTemplateUri
		/// </summary>
		/// <param name="componentUri">Component uri</param>
		/// <param name="componentTemplateUri">Component template uri</param>
		/// <returns>Assembled component presentation content</returns>
		/// <remarks>Note this only works for XML based dynamic component presentations</remarks>
		public static String Get(String componentUri, String componentTemplateUri)
		{
			return RemoteAPI.Execute<String>((client) =>
				client.Service.AssembleComponentPresentation(componentUri, componentTemplateUri),
				componentUri, componentTemplateUri);
		}
	}
}
