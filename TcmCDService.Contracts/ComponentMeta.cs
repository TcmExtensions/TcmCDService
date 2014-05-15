#region Header
////////////////////////////////////////////////////////////////////////////////////
//
//	File Description: Component Meta
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
	/// <see cref="ComponentMeta" /> allows serialization of <see cref="T:Tridion.ContentDelivery.Meta.IComponentMeta" />
	/// </summary>
	[DataContract(Namespace = "urn:TcmCDService", Name = "componentMeta")]
	public class ComponentMeta
	{
		/// <summary>
		/// Gets or sets the component author.
		/// </summary>
		/// <value>
		/// The component author.
		/// </value>
		[DataMember(Name = "author", EmitDefaultValue = false, Order = 1)]
		public String Author { get; set; }

		/// <summary>
		/// Gets or sets the component creation date.
		/// </summary>
		/// <value>
		/// The component creation date.
		/// </value>
		[DataMember(Name = "creationDate", EmitDefaultValue = false, Order = 2)]
		public DateTime CreationDate { get; set; }

		/// <summary>
		/// Gets or sets the component <see cref="I:System.Collections.Generic.IEnumerable{TcmCDService.Contracts.CustomMeta}" />
		/// </summary>
		/// <value>
		/// The component <see cref="I:System.Collections.Generic.IEnumerable{TcmCDService.Contracts.CustomMeta}" />
		/// </value>
		[DataMember(Name = "customMeta", EmitDefaultValue = false, Order = 3)]
		public IEnumerable<CustomMeta> CustomMeta { get; set; }

		/// <summary>
		/// Gets or sets the component identifier.
		/// </summary>
		/// <value>
		/// The component identifier.
		/// </value>
		[DataMember(Name = "id", EmitDefaultValue = false, Order = 4)]
		public int Id { get; set; }

		/// <summary>
		/// Gets or sets the component initial publication date.
		/// </summary>
		/// <value>
		/// The component initial publication date.
		/// </value>
		[DataMember(Name = "initialPublicationDate", EmitDefaultValue = false, Order = 5)]
		public DateTime InitialPublicationDate { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this component is a multimedia component.
		/// </summary>
		/// <value>
		/// <c>true</c> if this component is a multimedia component; otherwise, <c>false</c>.
		/// </value>
		[DataMember(Name = "isMultimedia", EmitDefaultValue = false, Order = 6)]
		public Boolean IsMultimedia { get; set; }

		/// <summary>
		/// Gets or sets the component last publication date.
		/// </summary>
		/// <value>
		/// The component last publication date.
		/// </value>
		[DataMember(Name = "lastPublicationDate", EmitDefaultValue = false, Order = 7)]
		public DateTime LastPublicationDate { get; set; }

		/// <summary>
		/// Gets or sets the component major version.
		/// </summary>
		/// <value>
		/// The component major version.
		/// </value>
		[DataMember(Name = "majorVersion", EmitDefaultValue = false, Order = 8)]
		public int MajorVersion { get; set; }

		/// <summary>
		/// Gets or sets the component minor version.
		/// </summary>
		/// <value>
		/// The component minor version.
		/// </value>
		[DataMember(Name = "minorVersion", EmitDefaultValue = false, Order = 9)]
		public int MinorVersion { get; set; }

		/// <summary>
		/// Gets or sets the component modification date.
		/// </summary>
		/// <value>
		/// The component modification date.
		/// </value>
		[DataMember(Name = "modificationDate", EmitDefaultValue = false, Order = 10)]
		public DateTime ModificationDate { get; set; }

		/// <summary>
		/// Gets or sets the component owning publication identifier.
		/// </summary>
		/// <value>
		/// The component owning publication identifier.
		/// </value>
		[DataMember(Name = "owningPublicationId", EmitDefaultValue = false, Order = 11)]
		public int OwningPublicationId { get; set; }

		/// <summary>
		/// Gets or sets the component publication identifier.
		/// </summary>
		/// <value>
		/// The component publication identifier.
		/// </value>
		[DataMember(Name = "publicationId", EmitDefaultValue = false, Order = 12)]
		public int PublicationId { get; set; }

		/// <summary>
		/// Gets or sets the component schema identifier.
		/// </summary>
		/// <value>
		/// The component schema identifier.
		/// </value>
		[DataMember(Name = "schemaId", EmitDefaultValue = false, Order = 13)]
		public int SchemaId { get; set; }

		/// <summary>
		/// Gets or sets the component title.
		/// </summary>
		/// <value>
		/// The component title.
		/// </value>
		[DataMember(Name = "title", EmitDefaultValue = false, Order = 14)]
		public String Title { get; set; }
	}
}
