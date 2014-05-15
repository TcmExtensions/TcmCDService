#region Header
////////////////////////////////////////////////////////////////////////////////////
//
//	File Description: Component Link
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

namespace TcmCDService.Remoting.Web.Linking
{
	/// <summary>
	///   <see cref="ComponentLink" /> connects to TcmCDService to provide Tridion component link resolving
	/// </summary>
	public static class ComponentLink
	{
		/// <summary>
		/// Resolves the component link
		/// </summary>
		/// <param name="sourcePageUri">Page uri</param>
		/// <param name="targetComponentUri">Target component uri</param>
		/// <param name="excludeTemplateUri">Excluded template uri</param>
		/// <param name="showAnchor">If <c>true</c>, render the url anchor</param>
		/// <returns>Resolved component url or null</returns>
		public static String Get(String sourcePageUri, String targetComponentUri, String excludeTemplateUri, Boolean showAnchor)
		{
			return RemoteAPI.Execute<String>((client) =>
				client.Service.ComponentLink(sourcePageUri, targetComponentUri, excludeTemplateUri, showAnchor),
				sourcePageUri, targetComponentUri, excludeTemplateUri);
		}

		/// <summary>
		/// Resolves the component link
		/// </summary>
		/// <param name="componentUri">Component uri</param>
		/// <returns>Resolved component url or null</returns>		
		public static String Get(String componentUri)
		{
			return RemoteAPI.Execute<String>((client) =>
				client.Service.ComponentLink(componentUri),
				componentUri);
		}
	}
}
