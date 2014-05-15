#region Header
////////////////////////////////////////////////////////////////////////////////////
//
//	File Description: Binary Link
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
	///   <see cref="BinaryLink" /> connects to TcmCDService to provide Tridion binary link resolving
	/// </summary>
	public static class BinaryLink
	{
		/// <summary>
		/// Resolves the binary link
		/// </summary>
		/// <param name="binaryUri">Binary component uri</param>
		/// <param name="variantId">Binary variant id</param>
		/// <param name="anchor">Link anchor</param>
		/// <returns>Resolved binary url or null</returns>
		public static String Get(String binaryUri, String variantId, String anchor)
		{
			return RemoteAPI.Execute<String>((client) =>
				client.Service.BinaryLink(binaryUri, variantId, anchor),
				binaryUri);
		}
	}
}
