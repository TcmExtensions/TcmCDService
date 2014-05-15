#region Header
////////////////////////////////////////////////////////////////////////////////////
//
//	File Description: Keyword Query
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
using System.Runtime.Serialization;

namespace TcmCDService.Contracts
{
	/// <summary>
	/// <see cref="KeywordQuery" /> allows a <see cref="T:TcmCDService.Contracts.BrokerQuery" /> to query based on keyword conditions
	/// </summary>
	[DataContract(Namespace = "urn:TcmCDService", Name = "keywordQuery")]
	public class KeywordQuery : SubQuery
	{
	}
}
