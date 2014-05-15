#region Header
////////////////////////////////////////////////////////////////////////////////////
//
//	File Description: Query
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
	///   <see cref="Query" /> connects to TcmCDService to provide Tridion execute broker queries
	/// </summary>
	public static class Query
	{
		/// <summary>
		/// Gets the <see cref="T:System.Collections.Generic.IEnumerable{System.String}" /> of matched component uris
		/// </summary>
		/// <param name="brokerQuery"><see cref="T:TcmCDService.Contracts.BrokerQuery" /></param>
		/// <returns>
		///   <see cref="T:System.Collections.Generic.IEnumerable{System.String}" />
		/// </returns>		
		public static IEnumerable<String> Execute(Contracts.BrokerQuery brokerQuery)
		{
			return RemoteAPI.Execute<IEnumerable<String>>((client) =>
				client.Service.BrokerQuery(brokerQuery), 1);
		}

		/// <summary>
		/// Gets the <see cref="T:System.Collections.Generic.IEnumerable{System.String}" /> of matched component uris
		/// </summary>
		/// <param name="brokerQueries"><see cref="I:System.Collections.Generic.IEnumerable{TcmCDService.Contracts.BrokerQuery}" /></param>
		/// <returns>
		///   <see cref="T:System.Collections.Generic.IEnumerable{System.String}" />
		/// </returns>		
		public static IEnumerable<String> Execute(IEnumerable<Contracts.BrokerQuery> brokerQueries)
		{
			return RemoteAPI.Execute<IEnumerable<String>>((client) =>
				client.Service.BrokerQuery(brokerQueries), 1);
		}

		/// <summary>
		/// Gets the <see cref="T:System.Collections.Generic.IEnumerable{TcmCDService.Contracts.ComponentPresentation}" /> of matched components
		/// </summary>
		/// <param name="brokerQuery"><see cref="T:TcmCDService.Contracts.BrokerQuery" /></param>
		/// <returns>
		///   <see cref="T:System.Collections.Generic.IEnumerable{TcmCDService.Contracts.ComponentPresentation}" />
		/// </returns>
		public static IEnumerable<Contracts.ComponentPresentation> ExecutePresentations(Contracts.BrokerQuery brokerQuery)
		{
			return RemoteAPI.Execute<IEnumerable<Contracts.ComponentPresentation>>((client) =>
				client.Service.BrokerQueryPresentations(brokerQuery), 1);
		}

		/// <summary>
		/// Gets the <see cref="T:System.Collections.Generic.IEnumerable{TcmCDService.Contracts.ComponentPresentation}" /> of matched components
		/// </summary>
		/// <param name="brokerQueries"><see cref="I:System.Collections.Generic.IEnumerable{TcmCDService.Contracts.BrokerQuery}" /></param>
		/// <returns>
		///   <see cref="T:System.Collections.Generic.IEnumerable{TcmCDService.Contracts.ComponentPresentation}" />
		/// </returns>
		public static IEnumerable<Contracts.ComponentPresentation> ExecutePresentations(IEnumerable<Contracts.BrokerQuery> brokerQueries)
		{
			return RemoteAPI.Execute<IEnumerable<Contracts.ComponentPresentation>>((client) =>
				client.Service.BrokerQueryPresentations(brokerQueries), 1);
		}
	}
}
