#region Header
////////////////////////////////////////////////////////////////////////////////////
//
//	File Description: Meta Query
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
	/// Allowed value types for a <see cref="MetaQuery" /> definition
	/// </summary>
	[DataContract(Namespace = "urn:TcmCDService", Name = "queryValueType")]
	public enum QueryValueType
	{
		/// <summary>
		/// Compare against string values (KEY_STRING_VALUE)
		/// </summary>
		[EnumMember(Value = "String")]
		String,
		/// <summary>
		/// Compare against numeric values (KEY_FLOAT_VALUE)
		/// </summary>
		[EnumMember(Value = "Number")]
		Number,
		/// <summary>
		/// Compare against date values (KEY_DATE_VALUE)
		/// </summary>
		[EnumMember(Value = "Date")]
		Date
	}

	/// <summary>
	/// <see cref="MetaQuery" /> allows a <see cref="T:TcmCDService.Contracts.BrokerQuery" /> to query based on metadata conditions
	/// </summary>
	[DataContract(Namespace = "urn:TcmCDService")]
	public class MetaQuery : SubQuery
	{
		/// <summary>
		/// Gets or sets the valuetype to lookup for the <see cref="MetaQuery" /
		/// </summary>
		/// <value>
		/// Query value type, i.e. string, numeric or date.
		/// </value>
		[DataMember(Name = "valueType", EmitDefaultValue = false, Order = 1)]
		public QueryValueType ValueType { get; set; }
	}
}
