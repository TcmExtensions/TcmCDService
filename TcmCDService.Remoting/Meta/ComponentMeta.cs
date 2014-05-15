#region Header
////////////////////////////////////////////////////////////////////////////////////
//
//	File Description: Component Meta
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

namespace TcmCDService.Remoting.Meta
{
	/// <summary>
	///   <see cref="ComponentMeta" /> connects to TcmCDService to provide Tridion component meta retrieval
	/// </summary>
	public static class ComponentMeta
	{
		/// <summary>
		/// Retrieves the <see cref="T:TcmCDService.Contracts.ComponentMeta" /> for a given component Uri
		/// </summary>
		/// <param name="componentUri">Component uri</param>
		/// <returns>
		///   <see cref="T:TcmCDService.Contracts.ComponentMeta" />
		/// </returns>		
		public static Contracts.ComponentMeta Get(String componentUri)
		{
			return RemoteAPI.Execute<Contracts.ComponentMeta>((client) =>
				client.Service.ComponentMeta(componentUri), componentUri);
		}

		/// <summary>
		/// Retrieves the <see cref="T:TcmCDService.Contracts.ComponentMeta" /> for a given component id
		/// </summary>
		/// <param name="publicationId">Publication id as <see cref="T:System.Int32" /></param>
		/// <param name="componentId">Component id as <see cref="T:System.Int32" /></param>
		/// <returns>
		///   <see cref="T:TcmCDService.Contracts.ComponentMeta" /> or null
		/// </returns>		
		public static Contracts.ComponentMeta Get(int publicationId, int componentId)
		{
			return RemoteAPI.Execute<Contracts.ComponentMeta>((client) =>
				client.Service.ComponentMeta(publicationId, componentId), publicationId, componentId);
		}
	}
}
