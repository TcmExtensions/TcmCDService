#region Header
////////////////////////////////////////////////////////////////////////////////////
//
//	File Description: Taxonomy Filter
// ---------------------------------------------------------------------------------
//	Date Created	: April 4, 2014
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
	/// <see cref="TaxonomyFormatter" /> allows to specify the formatter to use for a taxonomy result
	/// </summary>
	[DataContract(Namespace = "urn:TcmCDService", Name = "taxonomyFormatter")]
	public enum TaxonomyFormatter
	{
		/// <summary>
		/// List taxonomy formatter
		/// </summary>
		[EnumMember(Value = "List")]
		List = 0,
		/// <summary>
		/// Hierarchy taxonomy formatter
		/// </summary>
		[EnumMember(Value = "Hierarchy")]
		Hierarchy = 1,
		/// <summary>
		/// Hierarchy taxonomy formatter with re-linking floating children
		/// </summary>
		[EnumMember(Value = "HierarchyRelink")]
		HierarchyRelink = 2
	}

	/// <summary>
	/// <see cref="TaxonomyFilterDirecton" /> allows to specify the direction of the taxonomy depth filter
	/// </summary>
	[DataContract(Namespace = "urn:TcmCDService", Name = "taxonomyFilterDirecton")]
	public enum TaxonomyFilterDirecton
	{
		/// <summary>
		/// Filter direction: Up
		/// </summary>
		[EnumMember(Value = "Up")]
		Up = 0,
		/// <summary>
		/// Filter direction: Down
		/// </summary>
		[EnumMember(Value = "Down")]
		Down = 1
	}

	/// <summary>
	/// <see cref="BrokerQuery" /> wraps a taxonomy filter request
	/// </summary>
	[DataContract(Namespace = "urn:TcmCDService", Name = "taxonomyFilter")]
	public class TaxonomyFilter
	{
		/// <summary>
		/// Gets or sets wether to filter if a <see cref="Keyword" /> is concrete.
		/// </summary>
		/// <value>
		/// <c>true</c> if filtering if a <see cref="Keyword" /> is concrete; otherwise <c>false</c>.
		/// </value>
		/// <remarks>Value of null means no filtering is applied</remarks>
		[DataMember(Name = "filterConcrete", EmitDefaultValue = false, Order = 1)]
		public Boolean? FilterConcrete { get; set; }

		/// <summary>
		/// Gets or sets wether to filter if a <see cref="Keyword" /> is abstract.
		/// </summary>
		/// <value>
		/// <c>true</c> if filtering if a <see cref="Keyword" /> is abstract; otherwise <c>false</c>.
		/// </value>
		/// <remarks>Value of null means no filtering is applied</remarks>
		[DataMember(Name = "filterAbstract", EmitDefaultValue = false, Order = 2)]
		public Boolean? FilterAbstract { get; set; }

		/// <summary>
		/// Gets or sets the depth filtering level for this <see cref="TaxonomyFilter" />
		/// </summary>
		/// <value>
		/// The depth filtering level for this <see cref="TaxonomyFilter" />
		/// </value>
		/// <remarks>Value of null means no filtering is applied</remarks>
		[DataMember(Name = "depthFilteringLevel", EmitDefaultValue = false, Order = 3)]
		public int? DepthFilteringLevel { get; set; }

		/// <summary>
		/// Gets or sets the <see cref="T:TcmCDService.Contracts.TaxonomyFilterDirection" /> for this <see cref="TaxonomyFilter" />
		/// </summary>
		/// <value>
		/// The <see cref="T:TcmCDService.Contracts.TaxonomyFilterDirection" /> for this <see cref="TaxonomyFilter" />
		/// </value>
		/// <remarks>Value of null means no filtering is applied</remarks>
		[DataMember(Name = "depthFiltering", EmitDefaultValue = false, Order = 4)]
		public TaxonomyFilterDirecton DepthFilteringDirection { get; set; }

		/// <summary>
		/// Gets or sets wether to filter if a <see cref="Keyword" /> has children.
		/// </summary>
		/// <value>
		/// <c>true</c> if filtering if a <see cref="Keyword" /> has children; otherwise <c>false</c>.
		/// </value>
		/// <remarks>Value of null means no filtering is applied</remarks>
		[DataMember(Name = "filterHasChildren", EmitDefaultValue = false, Order = 5)]
		public Boolean? FilterHasChildren { get; set; }

		/// <summary>
		/// Gets or sets wether to filter if a <see cref="Keyword" /> is navigable.
		/// </summary>
		/// <value>
		/// <c>true</c> if filtering if a <see cref="Keyword" /> is navigable; otherwise <c>false</c>.
		/// </value>
		/// <remarks>Value of null means no filtering is applied</remarks>
		[DataMember(Name = "filterNavigable", EmitDefaultValue = false, Order = 6)]
		public Boolean? FilterNavigable { get; set; }
	}
}
