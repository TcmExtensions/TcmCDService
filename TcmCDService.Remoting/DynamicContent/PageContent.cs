#region Header
////////////////////////////////////////////////////////////////////////////////////
//
//	File Description: Page Content
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
	///   <see cref="PageContent" /> connects to TcmCDService to provide Tridion page content retrieval
	/// </summary>
	public static class PageContent
	{
		/// <summary>
		/// Retrieves the page data for a given page tcm uri
		/// </summary>
		/// <param name="pageUri">Page uri</param>
		/// <returns>Page data as <see cref="T:System.String" /></returns>		
		public static String Get(String pageUri)
		{
			return RemoteAPI.Execute<String>((client) =>
				client.Service.Page(pageUri),
				pageUri);
		}

		/// <summary>
		/// Retrieves the page data for a given page tcm uri
		/// </summary>
		/// <param name="publicationId">Publication id as <see cref="T:System.Int32" /></param>
		/// <param name="pageId">Page id as <see cref="T:System.Int32" /></param>
		/// <returns>Page data as <see cref="T:System.String" /> array</returns>
		public static String Get(int publicationId, int pageId)
		{
			return RemoteAPI.Execute<String>((client) =>
				client.Service.Page(publicationId, pageId),
				publicationId, pageId);
		}
	}
}
