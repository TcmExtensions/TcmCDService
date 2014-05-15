#region Header
////////////////////////////////////////////////////////////////////////////////////
//
//	File Description: Component Presentation
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
	/// <see cref="ComponentPresentation" /> allows serialization of <see cref="T:Tridion.ContentDelivery.DynamicContent.ComponentPresentation" />
	/// </summary>
	[DataContract(Namespace = "urn:TcmCDService", Name = "componentPresentation")]
	public class ComponentPresentation
	{
		/// <summary>
		/// Gets or sets the component presentation component identifier.
		/// </summary>
		/// <value>
		/// The component presentation component identifier.
		/// </value>
		[DataMember(Name = "componentId", EmitDefaultValue = false, Order = 1)]
		public int ComponentId { get; set; }

		/// <summary>
		/// Gets or sets the component presentation component template identifier.
		/// </summary>
		/// <value>
		/// The component presentation component template identifier.
		/// </value>
		[DataMember(Name = "componentTemplateId", EmitDefaultValue = false, Order = 2)]
		public int ComponentTemplateId { get; set; }

		/// <summary>
		/// Gets or sets the component presentation content.
		/// </summary>
		/// <value>
		/// The componet presentation content.
		/// </value>
		[DataMember(Name = "content", EmitDefaultValue = false, Order = 3)]
		public String Content { get; set; }

		/// <summary>
		/// Gets or sets the component presentation file location.
		/// </summary>
		/// <value>
		/// The component presentation file location.
		/// </value>
		[DataMember(Name = "fileLocation", EmitDefaultValue = false, Order = 4)]
		public String FileLocation { get; set; }

		/// <summary>
		/// Gets or sets the component presentation <see cref="T:TcmCDService.Contracts.ComponentPresentationMeta" />
		/// </summary>
		/// <value>
		/// The component presentation <see cref="T:TcmCDService.Contracts.ComponentPresentationMeta" />
		/// </value>
		[DataMember(Name = "meta", EmitDefaultValue = false, Order = 5)]
		public ComponentPresentationMeta Meta { get; set; }

		/// <summary>
		/// Gets or sets the component presentation publication identifier.
		/// </summary>
		/// <value>
		/// The component presentation publication identifier.
		/// </value>
		[DataMember(Name = "publicationId", EmitDefaultValue = false, Order = 6)]
		public int PublicationId { get; set; }
	}
}
