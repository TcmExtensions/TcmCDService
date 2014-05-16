#region Header
////////////////////////////////////////////////////////////////////////////////////
//
//	File Description: Custom Meta
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
	/// <see cref="CustomMetaType" /> defines the <see cref="CustomMeta" /> data types
	/// </summary>
	[DataContract(Namespace = "urn:TcmCDService", Name = "customMetaType")]
	public enum CustomMetaType
	{
		/// <summary>
		/// <see cref="T:System.String" /> value
		/// </summary>
		[EnumMember(Value = "String")]
		String = 1,
		/// <summary>
		/// <see cref="T:System.DateTime" /> value
		/// </summary>
		[EnumMember(Value = "Date")]
		Date = 2,
		/// <summary>
		/// <see cref="T:System.Float" /> value
		/// </summary>
		[EnumMember(Value = "Float")]
		Float = 3
	}

	/// <summary>
	/// <see cref="CustomMeta" /> allows serialization of <see cref="T:Tridion.ContentDelivery.Meta.CustomMeta" />
	/// </summary>
	[DataContract(Namespace = "urn:TcmCDService", Name = "customMeta")]
	public class CustomMeta
	{
		/// <summary>
		/// Gets or sets the <see cref="CustomMeta" /> name.
		/// </summary>
		/// <value>
		/// The <see cref="CustomMeta" /> name.
		/// </value>
		[DataMember(Name = "name", EmitDefaultValue = false, Order = 1)]
		public String Name { get; set; }

		/// <summary>
		/// Gets or sets the <see cref="CustomMeta" /> <see cref="CustomMetaType" />
		/// </summary>
		/// <value>
		/// The <see cref="CustomMeta" /> <see cref="CustomMetaType" />
		/// </value>
		[DataMember(Name = "dataType", EmitDefaultValue = false, Order = 2)]
		public CustomMetaType DataType { get; set; }

		/// <summary>
		/// Gets or sets the <see cref="CustomMeta" /> <see cref="I:System.Collections.Generic.IEnumerable{System.String}" /> values.
		/// </summary>
		/// <value>
		/// The <see cref="CustomMeta" /> <see cref="I:System.Collections.Generic.IEnumerable{System.String}" /> values or null
		/// </value>
		[DataMember(Name = "stringValues", EmitDefaultValue = false, Order = 3)]
		public IEnumerable<String> StringValues { get; set; }

		/// <summary>
		/// Gets or sets the <see cref="CustomMeta" /> <see cref="I:System.Collections.Generic.IEnumerable{System.DateTime}" /> values.
		/// </summary>
		/// <value>
		/// The <see cref="CustomMeta" /> <see cref="I:System.Collections.Generic.IEnumerable{System.DateTime}" /> values or null
		/// </value>
		[DataMember(Name = "dateTimeValues", EmitDefaultValue = false, Order = 4)]
		public IEnumerable<DateTime> DateTimeValues { get; set; }

		/// <summary>
		/// Gets or sets the <see cref="CustomMeta" /> <see cref="I:System.Collections.Generic.IEnumerable{System.Single}" /> values.
		/// </summary>
		/// <value>
		/// The <see cref="CustomMeta" /> <see cref="I:System.Collections.Generic.IEnumerable{System.Single}" /> values or null
		/// </value>
		[DataMember(Name = "floatValues", EmitDefaultValue = false, Order = 5)]
		public IEnumerable<float> FloatValues { get; set; }
	}
}
