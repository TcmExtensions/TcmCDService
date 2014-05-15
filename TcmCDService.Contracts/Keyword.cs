using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TcmCDService.Contracts
{
	/// <summary>
	/// <see cref="Keyword" /> allows serialization of <see cref="T:Tridion.ContentDelivery.Taxonomies.Keyword" />
	/// </summary>
	[DataContract(Namespace = "urn:TcmCDService", Name = "keyword", IsReference = true)]
	public class Keyword
	{
		/// <summary>
		/// Gets or sets if <see cref="Keyword" /> has children.
		/// </summary>
		/// <value>
		/// <c>true</c> if the <see cref="Keyword" /> has children; otherwise <c>false</c>.
		/// </value>
		[DataMember(Name = "hasChildren", EmitDefaultValue = false, Order = 1)]
		public Boolean HasChildren { get; set; }

		/// <summary>
		/// Gets or sets if <see cref="Keyword" /> is abstract.
		/// </summary>
		/// <value>
		/// <c>true</c> if the <see cref="Keyword" /> is abstract; otherwise <c>false</c>.
		/// </value>
		[DataMember(Name = "isAbstract", EmitDefaultValue = false, Order = 2)]
		public Boolean IsAbstract { get; set; }

		/// <summary>
		/// Gets or sets if <see cref="Keyword" /> is navigable.
		/// </summary>
		/// <value>
		/// <c>true</c> if the <see cref="Keyword" /> is navigable; otherwise <c>false</c>.
		/// </value>
		[DataMember(Name = "isNavigable", EmitDefaultValue = false, Order = 3)]
		public Boolean IsNavigable { get; set; }

		/// <summary>
		/// Gets or sets if <see cref="Keyword" /> is used for navigation.
		/// </summary>
		/// <value>
		/// <c>true</c> if the <see cref="Keyword" /> is used for navigation; otherwise <c>false</c>.
		/// </value>
		[DataMember(Name = "isUsedForNavigation", EmitDefaultValue = false, Order = 4)]
		public Boolean IsUsedForNavigation { get; set; }

		/// <summary>
		/// Gets or sets the <see cref="I:System.Collections.Generic.IEnumerable{TcmCDService.Contracts.Keyword}" /> children
		/// </summary>
		/// <value>
		/// The <see cref="I:System.Collections.Generic.IEnumerable{TcmCDService.Contracts.Keyword}" /> children
		/// </value>
		[DataMember(Name = "children", EmitDefaultValue = false, Order = 5)]
		public IEnumerable<Keyword> Children { get; set; }

		/// <summary>
		/// Gets or sets the <see cref="Keyword" /> depth.
		/// </summary>
		/// <value>
		/// The <see cref="Keyword" /> depth.
		/// </value>
		[DataMember(Name = "depth", EmitDefaultValue = false, Order = 6)]
		public int Depth { get; set; }

		/// <summary>
		/// Gets or sets the <see cref="Keyword" /> description.
		/// </summary>
		/// <value>
		/// The <see cref="Keyword" /> description.
		/// </value>
		[DataMember(Name = "description", EmitDefaultValue = false, Order = 7)]
		public String Description { get; set; }

		/// <summary>
		/// Gets or sets the <see cref="Keyword" /> key.
		/// </summary>
		/// <value>
		/// The <see cref="Keyword" /> key.
		/// </value>
		[DataMember(Name = "key", EmitDefaultValue = false, Order = 8)]
		public String Key { get; set; }

		/// <summary>
		/// Gets or sets the <see cref="Keyword" /> left.
		/// </summary>
		/// <value>
		/// The <see cref="Keyword" /> left.
		/// </value>
		[DataMember(Name = "left", EmitDefaultValue = false, Order = 9)]
		public int Left { get; set; }

		/// <summary>
		/// Gets or sets the <see cref="Keyword" /> <see cref="I:System.Collections.Generic.IEnumerable{TcmCDService.Contracts.CustomMeta}" />
		/// </summary>
		/// <value>
		/// The <see cref="Keyword" /> <see cref="I:System.Collections.Generic.IEnumerable{TcmCDService.Contracts.CustomMeta}" />
		/// </value>	
		[DataMember(Name = "meta", EmitDefaultValue = false, Order = 10)]
		public IEnumerable<CustomMeta> Meta { get; set; }

		/// <summary>
		/// Gets or sets the <see cref="Keyword" /> name.
		/// </summary>
		/// <value>
		/// The <see cref="Keyword" /> name.
		/// </value>
		[DataMember(Name = "name", EmitDefaultValue = false, Order = 11)]
		public String Name { get; set; }

		/// <summary>
		/// Gets or sets the <see cref="Keyword" /> right.
		/// </summary>
		/// <value>
		/// The <see cref="Keyword" /> right.
		/// </value>
		[DataMember(Name = "right", EmitDefaultValue = false, Order = 12)]
		public int Right { get; set; }

		/// <summary>
		/// Gets or sets the <see cref="Keyword" /> Uri.
		/// </summary>
		/// <value>
		/// The <see cref="Keyword" /> Uri.
		/// </value>
		[DataMember(Name = "uri", EmitDefaultValue = false, Order = 13)]
		public String Uri { get; set; }

		/// <summary>
		/// Gets or sets the <see cref="Keyword" /> parent <see cref="Keyword" />.
		/// </summary>
		/// <value>
		/// The <see cref="Keyword" /> parent <see cref="Keyword" />.
		/// </value>
		[DataMember(Name = "parentKeyword", EmitDefaultValue = false, Order = 14)]
		public Keyword ParentKeyword { get; set; }

		/// <summary>
		/// Gets or sets the <see cref="Keyword" /> parent <see cref="I:System.Collections.Generic.IEnumerable{Keyword}" />
		/// </summary>
		/// <value>
		/// The <see cref="Keyword" /> parent <see cref="I:System.Collections.Generic.IEnumerable{Keyword}" />.
		/// </value>
		[DataMember(Name = "parentKeywords", EmitDefaultValue = false, Order = 15)]
		public IEnumerable<Keyword> ParentKeywords { get; set; }

		/// <summary>
		/// Gets or sets the <see cref="Keyword" /> referenced content count.
		/// </summary>
		/// <value>
		/// The <see cref="Keyword" /> referenced content count.
		/// </value>
		[DataMember(Name = "referencedContentCount", EmitDefaultValue = false, Order = 16)]
		public int ReferencedContentCount { get; set; }

		/// <summary>
		/// Gets or sets the <see cref="Keyword" /> taxonomy uri
		/// </summary>
		/// <value>
		/// The <see cref="Keyword" /> taxonomy uri
		/// </value>
		[DataMember(Name = "taxonomyUri", EmitDefaultValue = false, Order = 17)]
		public String TaxonomyUri { get; set; }

		/// <summary>
		/// Gets or sets the <see cref="Keyword" /> related <see cref="Keyword" /> uris.
		/// </summary>
		/// <value>
		/// The <see cref="Keyword" /> related <see cref="Keyword" /> uris.
		/// </value>
		[DataMember(Name = "relatedKeywordUris", EmitDefaultValue = false, Order = 18)]
		public IEnumerable<String> RelatedKeywordUris { get; set; }
	}
}
