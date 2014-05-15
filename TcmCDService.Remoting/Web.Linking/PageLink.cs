#region Header
////////////////////////////////////////////////////////////////////////////////////
//
//	File Description: Page Link
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
	///   <see cref="PageLink" /> connects to TcmCDService to provide Tridion page link resolving
	/// </summary>
	public static class PageLink
	{
		/// <summary>
		/// Resolves the page link
		/// </summary>
		/// <param name="targetPageUri">Page uri</param>
		/// <param name="anchor">Link anchor</param>
		/// <param name="parameters">Link parameters</param>
		/// <returns>Resolved page link or null</returns>		
		public static String Get(String targetPageUri, String anchor, String parameters)
		{
			return RemoteAPI.Execute<String>((client) =>
				client.Service.PageLink(targetPageUri, anchor, parameters),
				targetPageUri);
		}

		/// <summary>
		/// Resolves the page link
		/// </summary>
		/// <param name="targetPageUri">Page uri</param>
		/// <returns>Resolved page link or null</returns>
		public static String Get(String targetPageUri)
		{
			return RemoteAPI.Execute<String>((client) =>
				client.Service.PageLink(targetPageUri),
				targetPageUri);
		}
	}
}
