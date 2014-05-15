#region Header
////////////////////////////////////////////////////////////////////////////////////
//
//	File Description: Binary Data
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
	///   <see cref="BinaryData" /> connects to TcmCDService to provide Tridion binary data retrieval
	/// </summary>
	public static class BinaryData
	{
		/// <summary>
		/// Retrieves the binary data for a given binary tcm uri
		/// </summary>
		/// <param name="binaryUri">Binary uri</param>
		/// <returns>Binary data as <see cref="T:System.Byte" /> array</returns>		
		public static Byte[] Get(String binaryUri)
		{
			return RemoteAPI.Execute<Byte[]>((client) => 
				client.Service.Binary(binaryUri), 
				binaryUri);
		}

		/// <summary>
		/// Retrieves the binary data for a given binary tcm uri and variant id
		/// </summary>
		/// <param name="binaryUri">Binary uri</param>
		/// <param name="variantId">Binary variant id</param>
		/// <returns>Binary data as <see cref="T:System.Byte" /> array</returns>
		public static Byte[] Get(String binaryUri, String variantId)
		{
			return RemoteAPI.Execute<Byte[]>((client) => 
				client.Service.Binary(binaryUri, variantId), 
				binaryUri);
		}

		/// <summary>
		/// Retrieves the binary data for a given binary id variant id
		/// </summary>
		/// <param name="publicationId">Publication id as <see cref="T:System.Int32" /></param>
		/// <param name="binaryId">Binary id as <see cref="T:System.Int32" /></param>
		/// <returns>Binary data as <see cref="T:System.Byte" /> array</returns>		
		public static Byte[] Get(int publicationId, int binaryId)
		{
			return RemoteAPI.Execute<Byte[]>((client) => 
				client.Service.Binary(publicationId, binaryId), 
				publicationId, binaryId);
		}

		/// <summary>
		/// Retrieves the binary data for a given binary id variant id
		/// </summary>
		/// <param name="publicationId">Publication id as <see cref="T:System.Int32" /></param>
		/// <param name="binaryId">Binary id as <see cref="T:System.Int32" /></param>
		/// <param name="variantId">Binary variant id</param>
		/// <returns>Binary data as <see cref="T:System.Byte" /> array</returns>		
		public static Byte[] Get(int publicationId, int binaryId, String variantId)
		{
			return RemoteAPI.Execute<Byte[]>((client) => 
				client.Service.Binary(publicationId, binaryId, variantId), 
				publicationId, binaryId);
		}
	}
}
