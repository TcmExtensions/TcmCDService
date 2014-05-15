#region Header
////////////////////////////////////////////////////////////////////////////////////
//
//	File Description: Broker Query
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
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TcmCDService.Contracts
{
	/// <summary>
	/// <see cref="BrokerQuery" /> wraps a broker query request
	/// </summary>
	[DataContract(Namespace = "urn:TcmCDService", Name = "brokerQuery")]
	public class BrokerQuery
	{
		/// <summary>
		/// Gets or sets the <see cref="BrokerQuery" /> result limit.
		/// </summary>
		/// <value>
		/// The <see cref="BrokerQuery" /> result limit.
		/// </value>
		[DataMember(Name = "resultLimit", Order = 1, EmitDefaultValue = false)]
		public int? ResultLimit { get; set; }

		/// <summary>
		/// Gets or sets the <see cref="I:System.Collections.Generic.IEnumerable{System.String}" /> schema uris for this <see cref="BrokerQuery" />
		/// </summary>
		/// <value>
		/// The <see cref="I:System.Collections.Generic.IEnumerable{System.String}" /> schema uris for this <see cref="BrokerQuery" />
		/// </value>
		[DataMember(Name = "schemaUris", Order = 2, EmitDefaultValue = false)]
		public IEnumerable<String> SchemaUris { get; set; }

		/// <summary>
		/// Gets or sets the <see cref="I:System.Collections.Generic.IEnumerable{System.String}" /> component uris for this <see cref="BrokerQuery" />
		/// </summary>
		/// <value>
		/// The <see cref="I:System.Collections.Generic.IEnumerable{System.String}" /> component uris for this <see cref="BrokerQuery" />
		/// </value>
		[DataMember(Name = "componentUris", Order = 3, EmitDefaultValue = false)]
		public IEnumerable<String> ComponentUris { get; set; }

		/// <summary>
		/// Gets or sets the publication for this <see cref="BrokerQuery" />
		/// </summary>
		/// <value>
		/// The publication for this <see cref="BrokerQuery" />
		/// </value>
		[DataMember(Name = "publication", Order = 4, EmitDefaultValue = false)]
		public String Publication { get; set; }

		/// <summary>
		/// Gets or sets the component template uri for this <see cref="BrokerQuery" />
		/// </summary>
		/// <value>
		/// The component template uri for this <see cref="BrokerQuery" />
		/// </value>
		[DataMember(Name = "componentTemplateUri", Order = 5, EmitDefaultValue = false)]
		public String ComponentTemplateUri { get; set; }

		/// <summary>
		/// Gets or sets the item type for this <see cref="BrokerQuery" />
		/// </summary>
		/// <value>
		/// The item type for this <see cref="BrokerQuery" />
		/// </value>
		[DataMember(Name = "itemType", Order = 6, EmitDefaultValue = false)]
		public int ItemType { get; set; }

		/// <summary>
		/// Additional <see cref="I:System.Collections.Generic.IEnumerable{TcmCDService.Contracts.MetaQuery}" /> or 
		/// <see cref="I:System.Collections.Generic.IEnumerable{TcmCDService.Contracts.KeywordQuery}" /> to filter on
		/// </summary>
		/// <value><see cref="I:System.Collections.Generic.IEnumerable{TcmCDService.Contracts.SubQuery}" /></value>
		[DataMember(Name = "subQueries", Order = 7, EmitDefaultValue = false)]
		public IEnumerable<SubQuery> SubQueries { get; set; }
	}
}
