#region Header
////////////////////////////////////////////////////////////////////////////////////
//
//	File Description: Sub Query
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
using System.Runtime.Serialization;

namespace TcmCDService.Contracts
{
	/// <summary>
	/// Allowed operators for a subquery definition
	/// </summary>
	public enum QueryOperator
	{
		/// <summary>
		/// = (Is equal to)
		/// </summary>
		Equal,
		/// <summary>
		/// != (Is not equal to)
		/// </summary>
		NotEqual,
		/// <summary>
		/// ~= (Is like)
		/// </summary>
		Like,
		/// <summary>
		/// &lt; (Less than)
		/// </summary>
		Less,
		/// <summary>
		/// &lt;= (Less or equal than)
		/// </summary>
		LessEqual,
		/// <summary>
		/// &gt; (Greater than)
		/// </summary>
		Greater,
		/// <summary>
		/// &gt; (Greater or equal than)
		/// </summary>
		GreaterEqual
	}

	/// <summary>
	/// <see cref="SubQuery" /> is an abstract base class for all <see cref="T:TcmCDService.Contracts.BrokerQuery" /> subquery types.
	/// </summary>
	[DataContract(Namespace = "urn:TcmCDService", Name = "subQuery")]
	[KnownType(typeof(MetaQuery))]
	[KnownType(typeof(KeywordQuery))]
	public abstract class SubQuery
	{
		/// <summary>
		/// Gets or sets the target field for the <see cref="SubQuery" />
		/// </summary>
		/// <value><see cref="SubQuery" /> target field.</value>        
		[DataMember(Name = "field", EmitDefaultValue = false, Order = 1)]
		public String Field { get; set; }

		/// <summary>
		/// Gets or sets the value to use for the <see cref="SubQuery" />
		/// </summary>
		/// <value><see cref="SubQuery" /> Value</value>
		[DataMember(Name = "value", EmitDefaultValue = false, Order = 2)]
		public String Value { get; set; }

		/// <summary>
		/// Gets or sets the operation to apply for the <see cref="SubQuery" />
		/// </summary>
		/// <value><see cref="SubQuery" /> <see cref="QueryOperator" /></value>
		[DataMember(Name = "operation", EmitDefaultValue = false, Order = 3)]
		public QueryOperator Operation { get; set; }
	}
}
