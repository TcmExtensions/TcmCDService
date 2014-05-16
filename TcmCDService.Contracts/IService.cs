#region Header
////////////////////////////////////////////////////////////////////////////////////
//
//	File Description: IService
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
using System.ServiceModel;

namespace TcmCDService.Contracts
{
	/// <summary>
	/// <see cref="IService" /> defines the service contract for <see cref="T:TcmCDService.Service" />
	/// </summary>
	[ServiceContract(Namespace = "urn:TcmCDService", Name = "TcmCDService")]
	public interface IService
	{
		/// <summary>
		/// Resolves the component link
		/// </summary>
		/// <param name="sourcePageUri">Page uri</param>
		/// <param name="targetComponentUri">Target component uri</param>
		/// <param name="excludeTemplateUri">Excluded template uri</param>
		/// <param name="showAnchor">If <c>true</c>, render the url anchor</param>
		/// <returns>Resolved component url or null</returns>
		[OperationContract(Name = "componentLink")]
		String ComponentLink(String sourcePageUri, String targetComponentUri, String excludeTemplateUri, Boolean showAnchor);

		/// <summary>
		/// Resolves the component link
		/// </summary>
		/// <param name="componentUri">Component uri</param>
		/// <returns>Resolved component url or null</returns>
		[OperationContract(Name = "componentLinkUri")]
		String ComponentLink(String componentUri);

		/// <summary>
		/// Resolves the binary link
		/// </summary>
		/// <param name="binaryUri">Binary component uri</param>
		/// <param name="variantId">Binary variant id</param>
		/// <param name="anchor">Link anchor</param>
		/// <returns>Resolved binary url or null</returns>
		[OperationContract(Name = "binaryLink")]
		String BinaryLink(String binaryUri, String variantId, String anchor);

		/// <summary>
		/// Resolves the page link
		/// </summary>
		/// <param name="targetPageUri">Page uri</param>
		/// <param name="anchor">Link anchor</param>
		/// <param name="parameters">Link parameters</param>
		/// <returns>Resolved page link or null</returns>
		[OperationContract(Name = "pageLink")]
		String PageLink(String targetPageUri, String anchor, String parameters);

		/// <summary>
		/// Resolves the page link
		/// </summary>
		/// <param name="targetPageUri">Page uri</param>
		/// <returns>Resolved page link or null</returns>
		[OperationContract(Name = "pageLinkUri")]
		String PageLink(String targetPageUri);

		/// <summary>
		/// Retrieves the <see cref="T:TcmCDService.Contracts.ComponentMeta" /> for a given component Uri
		/// </summary>
		/// <param name="componentUri">Component uri</param>
		/// <returns>
		///   <see cref="T:TcmCDService.Contracts.ComponentMeta" />
		/// </returns>
		[OperationContract(Name = "componentMetaUri")]
		Contracts.ComponentMeta ComponentMeta(String componentUri);

		/// <summary>
		/// Retrieves the <see cref="T:TcmCDService.Contracts.ComponentMeta" /> for a given component id
		/// </summary>
		/// <param name="publicationId">Publication id as <see cref="T:System.Int32" /></param>
		/// <param name="componentId">Component id as <see cref="T:System.Int32" /></param>
		/// <returns>
		///   <see cref="T:TcmCDService.Contracts.ComponentMeta" /> or null
		/// </returns>
		[OperationContract(Name = "componentMeta")]
		Contracts.ComponentMeta ComponentMeta(int publicationId, int componentId);

		/// <summary>
		/// Retrieves the page data for a given page tcm uri
		/// </summary>
		/// <param name="pageUri">Page uri</param>
		/// <returns>Page data as <see cref="T:System.String" /></returns>
		[OperationContract(Name = "pageUri")]
		String Page(String pageUri);

		/// <summary>
		/// Retrieves the page data for a given page tcm uri
		/// </summary>
		/// <param name="publicationId">Publication id as <see cref="T:System.Int32" /></param>
		/// <param name="pageId">Page id as <see cref="T:System.Int32" /></param>
		/// <returns>Page data as <see cref="T:System.String" /> array</returns>
		[OperationContract(Name = "page")]
		String Page(int publicationId, int pageId);

		/// <summary>
		/// Retrieves the binary data for a given binary tcm uri
		/// </summary>
		/// <param name="binaryUri">Binary uri</param>
		/// <returns>Binary data as <see cref="T:System.Byte" /> array</returns>
		[OperationContract(Name = "binaryUri")]
		Byte[] Binary(String binaryUri);

		/// <summary>
		/// Retrieves the binary data for a given binary tcm uri and variant id
		/// </summary>
		/// <param name="binaryUri">Binary uri</param>
		/// <param name="variantId">Binary variant id</param>
		/// <returns>Binary data as <see cref="T:System.Byte" /> array</returns>
		[OperationContract(Name = "binaryUriVariant")]
		Byte[] Binary(String binaryUri, String variantId);

		/// <summary>
		/// Retrieves the binary data for a given binary id variant id
		/// </summary>
		/// <param name="publicationId">Publication id as <see cref="T:System.Int32" /></param>
		/// <param name="binaryId">Binary id as <see cref="T:System.Int32" /></param>
		/// <returns>Binary data as <see cref="T:System.Byte" /> array</returns>
		[OperationContract(Name = "binary")]
		Byte[] Binary(int publicationId, int binaryId);

		/// <summary>
		/// Retrieves the binary data for a given binary id variant id
		/// </summary>
		/// <param name="publicationId">Publication id as <see cref="T:System.Int32" /></param>
		/// <param name="binaryId">Binary id as <see cref="T:System.Int32" /></param>
		/// <param name="variantId">Binary variant id</param>
		/// <returns>Binary data as <see cref="T:System.Byte" /> array</returns>
		[OperationContract(Name = "binaryVariant")]
		Byte[] Binary(int publicationId, int binaryId, String variantId);

		/// <summary>
		/// Gets the <see cref="T:TcmCDService.Contracts.ComponentPresentation" /> with highest priority.
		/// </summary>
		/// <param name="componentUri">Component uri</param>
		/// <returns>
		///   <see cref="T:TcmCDService.Contracts.ComponentPresentation" />
		/// </returns>
		[OperationContract(Name = "componentPresentationWithHighestPriorityUri")]
		Contracts.ComponentPresentation ComponentPresentationWithHighestPriority(String componentUri);

		/// <summary>
		/// Gets the <see cref="T:TcmCDService.Contracts.ComponentPresentation" /> with highest priority.
		/// </summary>
		/// <param name="publicationId">Publication id as <see cref="T:System.Int32" /></param>
		/// <param name="componentId">Component id as <see cref="T:System.Int32" /></param>
		/// <returns>
		///   <see cref="T:TcmCDService.Contracts.ComponentPresentation" />
		/// </returns>
		[OperationContract(Name = "componentPresentationWithHighestPriority")]
		Contracts.ComponentPresentation ComponentPresentationWithHighestPriority(int publicationId, int componentId);

		/// <summary>
		/// Gets the <see cref="T:TcmCDService.Contracts.ComponentPresentation" /> with the specified templateUri
		/// </summary>
		/// <param name="componentUri">Component uri</param>
		/// <param name="templateUri">Component template uri</param>
		/// <returns>
		///   <see cref="T:TcmCDService.Contracts.ComponentPresentation" />
		/// </returns>
		[OperationContract(Name = "componentPresentationWithHighestPriorityTemplateBothUri")]
		Contracts.ComponentPresentation ComponentPresentation(String componentUri, String templateUri);

		/// <summary>
		/// Gets the <see cref="T:TcmCDService.Contracts.ComponentPresentation" /> with the specified template id
		/// </summary>
		/// <param name="componentUri">Component uri</param>
		/// <param name="templateId">Component template id as <see cref="T:System.Int32" /></param>
		/// <returns>
		///   <see cref="T:TcmCDService.Contracts.ComponentPresentation" />
		/// </returns>
		[OperationContract(Name = "componentPresentationWithHighestPriorityTemplateUri")]
		Contracts.ComponentPresentation ComponentPresentation(String componentUri, int templateId);

		/// <summary>
		/// Gets the <see cref="T:TcmCDService.Contracts.ComponentPresentation" /> with the specified templateUri
		/// </summary>
		/// <param name="publicationId">Publication id as <see cref="T:System.Int32" /></param>
		/// <param name="componentId">Component id as <see cref="T:System.Int32" /></param>
		/// <param name="templateId">Component template id as <see cref="T:System.Int32" /></param>
		/// <returns>
		///   <see cref="T:TcmCDService.Contracts.ComponentPresentation" /> or null
		/// </returns>
		[OperationContract(Name = "componentPresentationWithHighestPriorityTemplate")]
		Contracts.ComponentPresentation ComponentPresentation(int publicationId, int componentId, int templateId);

		/// <summary>
		/// Assembles the component presentation with the given componentUri and componentTemplateUri
		/// </summary>
		/// <param name="componentUri">Component uri</param>
		/// <param name="componentTemplateUri">Component template uri</param>
		/// <returns>Assembled component presentation content</returns>
		/// <remarks>Note this only works for XML based dynamic component presentations</remarks>
		[OperationContract(Name = "assembleComponentPresentation")]
		String AssembleComponentPresentation(String componentUri, String componentTemplateUri);

		/// <summary>
		/// Gets the <see cref="I:System.Collections.Generic.IEnumerable{System.String}" /> of matched component uris
		/// </summary>
		/// <param name="brokerQuery"><see cref="T:TcmCDService.Contracts.BrokerQuery" /></param>
		/// <returns>
		///   <see cref="I:System.Collections.Generic.IEnumerable{System.String}" />
		/// </returns>
		[OperationContract(Name = "brokerQuery")]
		IEnumerable<String> BrokerQuery(Contracts.BrokerQuery brokerQuery);

		/// <summary>
		/// Gets the <see cref="I:System.Collections.Generic.IEnumerable{System.String}" /> of matched component uris
		/// </summary>
		/// <param name="brokerQueries"><see cref="I:System.Collections.Generic.IEnumerable{TcmCDService.Contracts.BrokerQuery}" /></param>
		/// <returns>
		///   <see cref="I:System.Collections.Generic.IEnumerable{System.String}" />
		/// </returns>
		[OperationContract(Name = "brokerQueries")]
		IEnumerable<String> BrokerQuery(IEnumerable<Contracts.BrokerQuery> brokerQueries);

		/// <summary>
		/// Gets the <see cref="I:System.Collections.Generic.IEnumerable{TcmCDService.Contracts.ComponentPresentation}" /> of matched components
		/// </summary>
		/// <param name="brokerQuery"><see cref="T:TcmCDService.Contracts.BrokerQuery" /></param>
		/// <returns>
		///   <see cref="I:System.Collections.Generic.IEnumerable{TcmCDService.Contracts.ComponentPresentation}" />
		/// </returns>
		[OperationContract(Name = "brokerQueryPresentations")]
		IEnumerable<Contracts.ComponentPresentation> BrokerQueryPresentations(Contracts.BrokerQuery brokerQuery);

		/// <summary>
		/// Gets the <see cref="I:System.Collections.Generic.IEnumerable{TcmCDService.Contracts.ComponentPresentation}" /> of matched components
		/// </summary>
		/// <param name="brokerQueries"><see cref="I:System.Collections.Generic.IEnumerable{TcmCDService.Contracts.BrokerQuery}" /></param>
		/// <returns>
		///   <see cref="I:System.Collections.Generic.IEnumerable{TcmCDService.Contracts.ComponentPresentation}" />
		/// </returns>
		[OperationContract(Name = "brokerQueriesPresentations")]
		IEnumerable<Contracts.ComponentPresentation> BrokerQueryPresentations(IEnumerable<Contracts.BrokerQuery> brokerQueries);

		/// <summary>
		/// Gets the <see cref="I:System.Collections.Generic.IEnumerable{System.String}" /> taxonomy uris for a <see cref="P:publicationUri" />
		/// </summary>
		/// <param name="publicationUri">Publication Uri</param>
		/// <returns>
		///   <see cref="I:System.Collections.Generic.IEnumerable{System.String}" />
		/// </returns>
		[OperationContract(Name = "taxonomies")]
		IEnumerable<String> Taxonomies(String publicationUri);

		/// <summary>
		/// Gets the <see cref="T:TcmCDService.Contracts.Keyword" /> for a <see cref="P:keywordUri" />
		/// </summary>
		/// <param name="keywordUri">Keyword Uri</param>
		/// <returns>
		///   <see cref="T:TcmCDService.Contracts.Keyword" />
		/// </returns>
		[OperationContract(Name = "keyword")]
		Contracts.Keyword Keyword(String keywordUri);

		/// <summary>
		/// Gets the root keyword for the given <see cref="P:taxonomyUri" />
		/// </summary>
		/// <param name="taxonomyUri">Taxonomy <see cref="T:TcmCDService.Tridion.TcmUri" /></param>
		/// <returns>
		///   <see cref="T:TcmCDService.Contracts.Keyword" />
		/// </returns>
		[OperationContract(Name = "keywords")]
		Contracts.Keyword Keywords(String taxonomyUri);	

		/// <summary>
		/// Gets the root keyword for the given <see cref="P:taxonomyUri" />
		/// </summary>
		/// <param name="taxonomyUri">Taxonomy <see cref="T:TcmCDService.Tridion.TcmUri" /></param>
		/// <param name="taxonomyFilter"><see cref="T:TcmCDService.Contracts.TaxonomyFilter" /> to apply.</param>
		/// <returns>
		///   <see cref="T:TcmCDService.Contracts.Keyword" />
		/// </returns>
		[OperationContract(Name = "keywordsFilter")]
		Contracts.Keyword Keywords(String taxonomyUri, Contracts.TaxonomyFilter taxonomyFilter);

		/// <summary>
		/// Gets the root keyword for the given <see cref="P:taxonomyUri" />
		/// </summary>
		/// <param name="taxonomyUri">Taxonomy <see cref="T:TcmCDService.Tridion.TcmUri" /></param>
		/// <param name="taxonomyFilter"><see cref="T:TcmCDService.Contracts.TaxonomyFilter" /> to apply.</param>
		/// <param name="taxonomyFormatter"><see cref="T:TcmCDService.Contracts.TaxonomyFormatter" /> to apply.</param>
		/// <returns>
		///   <see cref="T:TcmCDService.Contracts.Keyword" />
		/// </returns>
		[OperationContract(Name = "keywordsFilterFormatter")]
		Contracts.Keyword Keywords(String taxonomyUri, Contracts.TaxonomyFilter taxonomyFilter, Contracts.TaxonomyFormatter taxonomyFormatter);
	}
}
