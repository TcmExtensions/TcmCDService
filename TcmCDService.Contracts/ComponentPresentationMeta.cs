#region Header
////////////////////////////////////////////////////////////////////////////////////
//
//	File Description: Component Presentation Meta
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
	/// <see cref="ComponentPresentationMeta" /> allows serialization of <see cref="T:Tridion.ContentDelivery.Meta.ComponentPresentationMeta" />
	/// </summary>
	[DataContract(Namespace = "urn:TcmCDService", Name = "componentPresentationMeta")]
	public class ComponentPresentationMeta
	{
		/// <summary>
		/// Gets or sets the component identifier.
		/// </summary>
		/// <value>
		/// The component identifier.
		/// </value>
		[DataMember(Name = "componentId", EmitDefaultValue = false, Order = 1)]
		public int ComponentId { get; set; }

		/// <summary>
		/// Gets or sets the type of the content.
		/// </summary>
		/// <value>
		/// The type of the content.
		/// </value>
		[DataMember(Name = "contentType", EmitDefaultValue = false, Order = 2)]
		public String ContentType { get; set; }

		/// <summary>
		/// Gets or sets the publication identifier.
		/// </summary>
		/// <value>
		/// The publication identifier.
		/// </value>
		[DataMember(Name = "publicationId", EmitDefaultValue = false, Order = 3)]
		public int PublicationId { get; set; }

		/// <summary>
		/// Gets or sets the template identifier.
		/// </summary>
		/// <value>
		/// The template identifier.
		/// </value>
		[DataMember(Name = "templateId", EmitDefaultValue = false, Order = 4)]
		public int TemplateId { get; set; }

		/// <summary>
		/// Gets or sets the template priority.
		/// </summary>
		/// <value>
		/// The template priority.
		/// </value>
		[DataMember(Name = "templatePriority", EmitDefaultValue = false, Order = 5)]
		public int TemplatePriority { get; set; }
	}
}
