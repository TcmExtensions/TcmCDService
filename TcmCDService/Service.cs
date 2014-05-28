#region Header
////////////////////////////////////////////////////////////////////////////////////
//
//	File Description: Service
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
using System.IO;
using System.Linq;
using System.ServiceModel;
using TcmCDService.Logging;
using TcmCDService.Extensions;
using Tridion.ContentDelivery.DynamicContent;
using Tridion.ContentDelivery.Meta;
using Tridion.ContentDelivery.Taxonomies;
using TcmCDService.Caching;
using TcmCDService.ContentDelivery;
using TcmCDService.CacheTypes;
using TcmCDService.Configuration;
using Tridion.ContentDelivery.Web.Jvm;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.Text;
using System.Net;

namespace TcmCDService
{
	/// <summary>
	/// <see cref="Service" /> implements the <see cref="I:TcmCDService.Contracts.IService" /> service contract
	/// </summary>
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
	public class Service : TcmCDService.Contracts.IService, IDisposable
	{
		private CacheType mCacheType;
		private IEnumerable<HealthChecks.HealthCheckType> mHealthCheckTypes;

		#region CacheType events
		private void CacheEvents_Connected(Object sender, EventArgs e)
		{
			Logger.Info("CacheConnector Connected");
		}

		private void CacheEvents_Disconnected(Object sender, EventArgs e)
		{
			Logger.Info("CacheConnector Disconnected");

			// Re-connect the CacheConnector
			mCacheType.Connect();
		}

		private void CacheEvents_CacheEvent(Object sender, CacheEventArgs data)
		{
			Cache.ProcessCacheEvent(data.Region, data.EventType, data.Key);
		}
		#endregion

		/// <summary>
		/// Gets the application path.
		/// </summary>
		/// <value>
		/// The application path.
		/// </value>
		private String ApplicationPath
		{
			get
			{
				return Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Service"/> class.
		/// </summary>		
		public Service()
		{
			String tridionHome = Config.Instance.TridionHome;
			String tridionConfiguration = Config.Instance.TridionConfiguration;

			if (!String.IsNullOrEmpty(tridionHome))
			{
				Logger.Info("Configuring TRIDION_HOME to \"{0}\".", tridionHome);
				Environment.SetEnvironmentVariable("TRIDION_HOME", tridionHome);

			}

			if (!String.IsNullOrEmpty(tridionConfiguration))
			{
				Logger.Info("Configuring tridion configuration folder to \"{0}\".", tridionConfiguration);
				ConfigurationHook.configFolder = tridionConfiguration;
			}

			mCacheType = CacheType.GetCacheType();

			mCacheType.Connected += CacheEvents_Connected;
			mCacheType.CacheEvent += CacheEvents_CacheEvent;
			mCacheType.Disconnected += CacheEvents_Disconnected;

			mCacheType.Connect();

			Cache.Expiration = mCacheType.Expiration;

			// Load any configured health checks
			mHealthCheckTypes = HealthChecks.HealthCheckType.LoadHealthCheckTypes();
		}

		/// <summary>
		/// Resolves the component link
		/// </summary>
		/// <param name="sourcePageUri">Page uri</param>
		/// <param name="targetComponentUri">Target component uri</param>
		/// <param name="excludeTemplateUri">Excluded template uri</param>
		/// <param name="showAnchor">If <c>true</c>, render the url anchor</param>
		/// <returns>Resolved component url or null</returns>
		public String ComponentLink(String sourcePageUri, String targetComponentUri, String excludeTemplateUri, Boolean showAnchor)
		{
			Logger.Debug("ComponentLink: sourcePageUri \"{0}\", targetComponentUri \"{1}\", excludeTemplateUri \"{2}\", showAnchor \"{3}\".", sourcePageUri, targetComponentUri, excludeTemplateUri, showAnchor);

			return Cache.Get<String>(
				String.Format("ComponentLink-{0}-{1}-{2}-{3}", sourcePageUri, targetComponentUri, excludeTemplateUri, showAnchor),
				() =>
				{
					return ContentDelivery.Web.Linking.ComponentLinkCache.ResolveComponentLink(sourcePageUri, targetComponentUri, excludeTemplateUri, showAnchor);
				},
				CacheRegion.ItemMeta | CacheRegion.ComponentLink,
				targetComponentUri);
		}

		/// <summary>
		/// Resolves the component link
		/// </summary>
		/// <param name="componentUri">Component uri</param>
		/// <returns>Resolved component url or null</returns>
		public String ComponentLink(String componentUri)
		{
			Logger.Debug("ComponentLink: componentUri \"{0}\".", componentUri);

			return Cache.Get<String>(
				String.Format("ComponentLink-{0}", componentUri),
				() =>
				{
					return ContentDelivery.Web.Linking.ComponentLinkCache.ResolveComponentLink(componentUri);
				},
				CacheRegion.ItemMeta | CacheRegion.ComponentLink,
				componentUri);
		}

		/// <summary>
		/// Resolves the binary link
		/// </summary>
		/// <param name="binaryUri">Binary component uri</param>
		/// <param name="variantId">Binary variant id</param>
		/// <param name="anchor">Link anchor</param>
		/// <returns>Resolved binary url or null</returns>
		public String BinaryLink(String binaryUri, String variantId, String anchor)
		{
			Logger.Debug("BinaryLink: binaryUri \"{0}\", variantId \"{1}\", anchor \"{2}\".", binaryUri, variantId, anchor);

			return Cache.Get<String>(
				String.Format("BinaryLink-{0}-{1}-{2}", binaryUri, variantId, anchor),
				() =>
				{
					return ContentDelivery.Web.Linking.BinaryLinkCache.ResolveBinaryLink(binaryUri, variantId, anchor);
				},
				CacheRegion.ItemMeta | CacheRegion.BinaryMeta,
				binaryUri);
		}

		/// <summary>
		/// Resolves the page link
		/// </summary>
		/// <param name="targetPageUri">Page uri</param>
		/// <param name="anchor">Link anchor</param>
		/// <param name="parameters">Link parameters</param>
		/// <returns>Resolved page link or null</returns>
		public String PageLink(String targetPageUri, String anchor, String parameters)
		{
			Logger.Debug("PageLink: targetPageUri \"{0}\", anchor \"{1}\", parameters \"{2}\".", targetPageUri, anchor, parameters);

			return Cache.Get<String>(
				String.Format("PageLink-{0}-{1}-{2}", targetPageUri, anchor, parameters),
				() =>
				{
					return ContentDelivery.Web.Linking.PageLinkCache.ResolvePageLink(targetPageUri, anchor, parameters);
				},
				CacheRegion.ItemMeta | CacheRegion.PageLink | CacheRegion.PageLinkInfo,
				targetPageUri);
		}

		/// <summary>
		/// Resolves the page link
		/// </summary>
		/// <param name="targetPageUri">Page uri</param>
		/// <returns>Resolved page link or null</returns>
		public String PageLink(String targetPageUri)
		{
			Logger.Debug("PageLink: targetPageUri \"{0}\".", targetPageUri);

			return Cache.Get<String>(
				String.Format("PageLink-{0}", targetPageUri),
				() =>
				{
					return ContentDelivery.Web.Linking.PageLinkCache.ResolvePageLink(targetPageUri);
				},
				CacheRegion.ItemMeta | CacheRegion.PageLink | CacheRegion.PageLinkInfo,
				targetPageUri);
		}

		/// <summary>
		/// Retrieves the <see cref="T:TcmCDService.Contracts.ComponentMeta" /> for a given component Uri
		/// </summary>
		/// <param name="componentUri">Component uri</param>
		/// <returns>
		///   <see cref="T:TcmCDService.Contracts.ComponentMeta" />
		/// </returns>
		public Contracts.ComponentMeta ComponentMeta(String componentUri)
		{
			Logger.Debug("ComponentMeta: componentUri \"{0}\".", componentUri);

			return Cache.Get<Contracts.ComponentMeta>(
				String.Format("ComponentMeta-{0}", componentUri),
				() =>
				{
					using (IComponentMeta componentMeta = ContentDelivery.Meta.ComponentMetaCache.GetComponentMeta(componentUri))
					{
						return componentMeta.ToContract();
					}
				},
				CacheRegion.ItemMeta | CacheRegion.ComponentMeta | CacheRegion.ComponentPresentationMeta,
				componentUri);
		}

		/// <summary>
		/// Retrieves the <see cref="T:TcmCDService.Contracts.ComponentMeta" /> for a given component id
		/// </summary>
		/// <param name="publicationId">Publication id as <see cref="T:System.Int32" /></param>
		/// <param name="componentId">Component id as <see cref="T:System.Int32" /></param>
		/// <returns>
		///   <see cref="T:TcmCDService.Contracts.ComponentMeta" /> or null
		/// </returns>
		public Contracts.ComponentMeta ComponentMeta(int publicationId, int componentId)
		{
			Logger.Debug("ComponentMeta: publicationId \"{0}\", componentId \"{1}\".", publicationId, componentId);

			return Cache.Get<Contracts.ComponentMeta>(
				String.Format("ComponentMeta-{0}-{1}", publicationId, componentId),
				() =>
				{
					using (IComponentMeta componentMeta = ContentDelivery.Meta.ComponentMetaCache.GetComponentMeta(publicationId, componentId))
					{
						return componentMeta.ToContract();
					}
				},
				CacheRegion.ItemMeta | CacheRegion.ComponentMeta | CacheRegion.ComponentPresentationMeta,
				new TcmUri(publicationId, componentId));
		}

		/// <summary>
		/// Retrieves the page data for a given page tcm uri
		/// </summary>
		/// <param name="pageUri">Page uri</param>
		/// <returns>Page data as <see cref="T:System.String" /></returns>
		public String Page(String pageUri)
		{
			Logger.Debug("Page: pageUri \"{0}\".", pageUri);

			return Cache.Get<String>(
				String.Format("Page-{0}", pageUri),
				() =>
				{
					using (CharacterData characterData = ContentDelivery.DynamicContent.PageContentCache.GetPageContent(pageUri))
					{
						return characterData.String;
					}
				},
				CacheRegion.ItemMeta | CacheRegion.PageMeta,
				pageUri);
		}

		/// <summary>
		/// Retrieves the binary data for a given binary tcm uri
		/// </summary>
		/// <param name="publicationId">Publication id as <see cref="T:System.Int32" /></param>
		/// <param name="pageId">Page id as <see cref="T:System.Int32" /></param>
		/// <returns>Page data as <see cref="T:System.String" /> array</returns>
		public String Page(int publicationId, int pageId)
		{
			Logger.Debug("Page: publicationId \"{0}\", pageId \"{1}\".", publicationId, pageId);

			return Cache.Get<String>(
				String.Format("Page-{0}-{1}", publicationId, pageId),
				() =>
				{
					using (CharacterData characterData = ContentDelivery.DynamicContent.PageContentCache.GetPageContent(publicationId, pageId))
					{
						return characterData.String;
					}
				},
				CacheRegion.ItemMeta | CacheRegion.PageMeta,
				new TcmUri(publicationId, pageId));
		}

		/// <summary>
		/// Retrieves the binary data for a given binary tcm uri
		/// </summary>
		/// <param name="binaryUri">Binary uri</param>
		/// <returns>Binary data as <see cref="T:System.Byte" /> array</returns>
		public Byte[] Binary(String binaryUri)
		{
			Logger.Debug("Binary: binaryUri \"{0}\".", binaryUri);

			return Cache.Get<Byte[]>(
				String.Format("Binary-{0}", binaryUri),
				() =>
				{
					using (BinaryData binaryData = ContentDelivery.DynamicContent.BinaryCache.GetBinaryData(binaryUri))
					{
						return binaryData.Bytes;
					}
				},
				CacheRegion.ItemMeta | CacheRegion.BinaryContent | CacheRegion.BinaryMeta,
				binaryUri);
		}

		/// <summary>
		/// Retrieves the binary data for a given binary tcm uri and variant id
		/// </summary>
		/// <param name="binaryUri">Binary uri</param>
		/// <param name="variantId">Binary variant id</param>
		/// <returns>Binary data as <see cref="T:System.Byte" /> array</returns>
		public Byte[] Binary(String binaryUri, String variantId)
		{
			Logger.Debug("Binary: binaryUri \"{0}\", variantId \"{1}\".", binaryUri, variantId);
			return Cache.Get<Byte[]>(
				String.Format("Binary-{0}-{1}", binaryUri, variantId),
				() =>
				{
					using (BinaryData binaryData = ContentDelivery.DynamicContent.BinaryCache.GetBinaryData(binaryUri, variantId))
					{
						return binaryData.Bytes;
					}
				},
				CacheRegion.ItemMeta | CacheRegion.BinaryContent | CacheRegion.BinaryMeta,
				binaryUri);
		}

		/// <summary>
		/// Retrieves the binary data for a given binary id variant id
		/// </summary>
		/// <param name="publicationId">Publication id as <see cref="T:System.Int32" /></param>
		/// <param name="binaryId">Binary id as <see cref="T:System.Int32" /></param>
		/// <returns>Binary data as <see cref="T:System.Byte" /> array</returns>
		public Byte[] Binary(int publicationId, int binaryId)
		{
			Logger.Debug("Binary: publicationId \"{0}\", binaryId \"{1}\".", publicationId, binaryId);

			return Cache.Get<Byte[]>(
				String.Format("Binary-{0}-{1}", publicationId, binaryId),
				() =>
				{
					using (BinaryData binaryData = ContentDelivery.DynamicContent.BinaryCache.GetBinaryData(publicationId, binaryId))
					{
						return binaryData.Bytes;
					}
				},
				CacheRegion.ItemMeta | CacheRegion.BinaryContent | CacheRegion.BinaryMeta,
				new TcmUri(publicationId, binaryId));
		}

		/// <summary>
		/// Retrieves the binary data for a given binary id variant id
		/// </summary>
		/// <param name="publicationId">Publication id as <see cref="T:System.Int32" /></param>
		/// <param name="binaryId">Binary id as <see cref="T:System.Int32" /></param>
		/// <param name="variantId">Binary variant id</param>
		/// <returns>Binary data as <see cref="T:System.Byte" /> array</returns>
		public Byte[] Binary(int publicationId, int binaryId, String variantId)
		{
			Logger.Debug("Binary: publicationId \"{0}\", binaryId \"{1}\", variantId \"{2}\".", publicationId, binaryId, variantId);

			return Cache.Get<Byte[]>(
				String.Format("Binary-{0}-{1}-{2}", publicationId, binaryId, variantId),
				() =>
				{
					using (BinaryData binaryData = ContentDelivery.DynamicContent.BinaryCache.GetBinaryData(publicationId, binaryId, variantId))
					{
						return binaryData.Bytes;
					}
				},
				CacheRegion.ItemMeta | CacheRegion.BinaryContent | CacheRegion.BinaryMeta,
				new TcmUri(publicationId, binaryId));
		}

		/// <summary>
		/// Gets the <see cref="T:TcmCDService.Contracts.ComponentPresentation" /> with highest priority.
		/// </summary>
		/// <param name="componentUri">Component uri</param>
		/// <returns>
		///   <see cref="T:TcmCDService.Contracts.ComponentPresentation" />
		/// </returns>
		public Contracts.ComponentPresentation ComponentPresentationWithHighestPriority(String componentUri)
		{
			Logger.Debug("ComponentPresentationWithHighestPriority: componentUri \"{0}\".", componentUri);

			return Cache.Get<Contracts.ComponentPresentation>(
				String.Format("ComponentPresentationWithHighestPriority-{0}", componentUri),
				() =>
				{
					using (ComponentPresentation componentPresentation = ContentDelivery.DynamicContent.ComponentPresentationCache.GetComponentPresentationWithHighestPriority(componentUri))
					{
						return componentPresentation.ToContract();
					}
				},
				CacheRegion.ItemMeta | CacheRegion.ComponentPresentation | CacheRegion.ComponentPresentationMeta,
				componentUri);
		}

		/// <summary>
		/// Gets the <see cref="T:TcmCDService.Contracts.ComponentPresentation" /> with highest priority.
		/// </summary>
		/// <param name="publicationId">Publication id as <see cref="T:System.Int32" /></param>
		/// <param name="componentId">Component id as <see cref="T:System.Int32" /></param>
		/// <returns>
		///   <see cref="T:TcmCDService.Contracts.ComponentPresentation" />
		/// </returns>
		public Contracts.ComponentPresentation ComponentPresentationWithHighestPriority(int publicationId, int componentId)
		{
			Logger.Debug("ComponentPresentationWithHighestPriority: publicationId \"{0}\", componentId \"{1}\".", publicationId, componentId);

			return Cache.Get<Contracts.ComponentPresentation>(
				String.Format("ComponentPresentationWithHighestPriority-{0}-{1}", publicationId, componentId),
				() =>
				{
					using (ComponentPresentation componentPresentation = ContentDelivery.DynamicContent.ComponentPresentationCache.GetComponentPresentationWithHighestPriority(publicationId, componentId))
					{
						return componentPresentation.ToContract();
					}
				},
				CacheRegion.ItemMeta | CacheRegion.ComponentPresentation | CacheRegion.ComponentPresentationMeta,
				new TcmUri(publicationId, componentId));
		}

		/// <summary>
		/// Gets the <see cref="T:TcmCDService.Contracts.ComponentPresentation" /> with the specified templateUri
		/// </summary>
		/// <param name="componentUri">Component uri</param>
		/// <param name="templateUri">Component template uri</param>
		/// <returns>
		///   <see cref="T:TcmCDService.Contracts.ComponentPresentation" />
		/// </returns>
		public Contracts.ComponentPresentation ComponentPresentation(String componentUri, String templateUri)
		{
			Logger.Debug("ComponentPresentation: componentUri \"{0}\", templateUri \"{1}\".", componentUri, templateUri);

			return Cache.Get<Contracts.ComponentPresentation>(
				String.Format("ComponentPresentation-{0}-{1}", componentUri, templateUri),
				() =>
				{
					using (ComponentPresentation componentPresentation = ContentDelivery.DynamicContent.ComponentPresentationCache.GetComponentPresentation(componentUri, templateUri))
					{
						return componentPresentation.ToContract();
					}
				},
				CacheRegion.ItemMeta | CacheRegion.ComponentPresentation | CacheRegion.ComponentPresentationMeta,
				new TcmUri(componentUri));
		}

		/// <summary>
		/// Gets the <see cref="T:TcmCDService.Contracts.ComponentPresentation" /> with the specified template id
		/// </summary>
		/// <param name="componentUri">Component uri</param>
		/// <param name="templateId">Component template id as <see cref="T:System.Int32" /></param>
		/// <returns>
		///   <see cref="T:TcmCDService.Contracts.ComponentPresentation" />
		/// </returns>
		public Contracts.ComponentPresentation ComponentPresentation(String componentUri, int templateId)
		{
			Logger.Debug("ComponentPresentation: componentUri \"{0}\", templateId \"{1}\".", componentUri, templateId);

			return Cache.Get<Contracts.ComponentPresentation>(
				String.Format("ComponentPresentation-{0}-{1}", componentUri, templateId),
				() =>
				{
					using (ComponentPresentation componentPresentation = ContentDelivery.DynamicContent.ComponentPresentationCache.GetComponentPresentation(componentUri, templateId))
					{
						return componentPresentation.ToContract();
					}
				},
				CacheRegion.ItemMeta | CacheRegion.ComponentPresentation | CacheRegion.ComponentPresentationMeta,
				componentUri);
		}

		/// <summary>
		/// Gets the <see cref="T:TcmCDService.Contracts.ComponentPresentation" /> with the specified templateUri
		/// </summary>
		/// <param name="publicationId">Publication id as <see cref="T:System.Int32" /></param>
		/// <param name="componentId">Component id as <see cref="T:System.Int32" /></param>
		/// <param name="templateId">Component template id as <see cref="T:System.Int32" /></param>
		/// <returns>
		///   <see cref="T:TcmCDService.Contracts.ComponentPresentation" /> or null
		/// </returns>
		public Contracts.ComponentPresentation ComponentPresentation(int publicationId, int componentId, int templateId)
		{
			Logger.Debug("ComponentPresentation: publicationId \"{0}\", componentId \"{1}\", templateId \"{2}\".", publicationId, componentId, templateId);

			return Cache.Get<Contracts.ComponentPresentation>(
				String.Format("ComponentPresentation-{0}-{1}-{2}", publicationId, componentId, templateId),
				() =>
				{
					using (ComponentPresentation componentPresentation = ContentDelivery.DynamicContent.ComponentPresentationCache.GetComponentPresentation(publicationId, componentId, templateId))
					{
						return componentPresentation.ToContract();
					}
				},
				CacheRegion.ItemMeta | CacheRegion.ComponentPresentation | CacheRegion.ComponentPresentationMeta,
				new TcmUri(publicationId, componentId));
		}

		/// <summary>
		/// Assembles the component presentation with the given componentUri and componentTemplateUri
		/// </summary>
		/// <param name="componentUri">Component uri</param>
		/// <param name="componentTemplateUri">Component template uri</param>
		/// <returns>Assembled component presentation content</returns>
		/// <remarks>Note this only works for XML based dynamic component presentations</remarks>
		public String AssembleComponentPresentation(String componentUri, String componentTemplateUri)
		{
			Logger.Debug("AssembleComponentPresentation: componentUri \"{0}\", componentTemplateUri \"{1}\".", componentUri, componentTemplateUri);

			return Cache.Get<String>(
				String.Format("ComponentPresentationAssembly-{0}-{1}", componentUri, componentTemplateUri),
				() =>
				{
					return ContentDelivery.DynamicContent.ComponentPresentationAssemblyCache.AssembleComponentPresentation(componentUri, componentTemplateUri);
				},
				CacheRegion.ItemMeta | CacheRegion.ComponentPresentation | CacheRegion.ComponentPresentationMeta,
				componentUri);
		}

		/// <summary>
		/// Gets the <see cref="I:System.Collections.Generic.IEnumerable{System.String}" /> of matched component uris
		/// </summary>
		/// <param name="brokerQuery"><see cref="T:TcmCDService.Contracts.BrokerQuery" /></param>
		/// <returns>
		///   <see cref="I:System.Collections.Generic.IEnumerable{System.String}" />
		/// </returns>
		public IEnumerable<String> BrokerQuery(Contracts.BrokerQuery brokerQuery)
		{
			Logger.Debug("BrokerQuery: brokerQuery \"{0}\".", brokerQuery != null ? "<Object>" : String.Empty);

			return ContentDelivery.DynamicContent.Query.QueryCache.Execute(brokerQuery);		
		}

		/// <summary>
		/// Gets the <see cref="I:System.Collections.Generic.IEnumerable{System.String}" /> of matched component uris
		/// </summary>
		/// <param name="brokerQueries"><see cref="I:System.Collections.Generic.IEnumerable{TcmCDService.Contracts.BrokerQuery}" /></param>
		/// <returns>
		///   <see cref="I:System.Collections.Generic.IEnumerable{System.String}" />
		/// </returns>
		public IEnumerable<String> BrokerQuery(IEnumerable<Contracts.BrokerQuery> brokerQueries)
		{
			Logger.Debug("BrokerQuery: brokerQueries \"{0}\".", brokerQueries != null ? String.Format("<Object>[{0}]", brokerQueries.Count()) : String.Empty);

			List<String> results = new List<String>();

			foreach (Contracts.BrokerQuery brokerQuery in brokerQueries)
				results.AddRange(ContentDelivery.DynamicContent.Query.QueryCache.Execute(brokerQuery));

			return results;
		}

		/// <summary>
		/// Gets the <see cref="I:System.Collections.Generic.IEnumerable{TcmCDService.Contracts.ComponentPresentation}" /> of matched components
		/// </summary>
		/// <param name="brokerQuery"><see cref="T:TcmCDService.Contracts.BrokerQuery" /></param>
		/// <returns>
		///   <see cref="I:System.Collections.Generic.IEnumerable{TcmCDService.Contracts.ComponentPresentation}" />
		/// </returns>
		public IEnumerable<Contracts.ComponentPresentation> BrokerQueryPresentations(Contracts.BrokerQuery brokerQuery)
		{
			Logger.Debug("BrokerQueryPresentations: brokerQueries \"{0}\".", brokerQuery != null ? "<Object>" : String.Empty);

			return ContentDelivery.DynamicContent.Query.QueryCache.Execute(brokerQuery).Select(u => this.ComponentPresentationWithHighestPriority(u));
		}

		/// <summary>
		/// Gets the <see cref="I:System.Collections.Generic.IEnumerable{TcmCDService.Contracts.ComponentPresentation}" /> of matched components
		/// </summary>
		/// <param name="brokerQueries"><see cref="I:System.Collections.Generic.IEnumerable{TcmCDService.Contracts.BrokerQuery}" /></param>
		/// <returns>
		///   <see cref="I:System.Collections.Generic.IEnumerable{TcmCDService.Contracts.ComponentPresentation}" />
		/// </returns>
		public IEnumerable<Contracts.ComponentPresentation> BrokerQueryPresentations(IEnumerable<Contracts.BrokerQuery> brokerQueries)
		{
			Logger.Debug("BrokerQueriesPresentations: brokerQueries \"{0}\".", brokerQueries != null ? String.Format("<Object>[{0}]", brokerQueries.Count()) : String.Empty);
			
			List<Contracts.ComponentPresentation> results = new List<Contracts.ComponentPresentation>();

			foreach (Contracts.BrokerQuery brokerQuery in brokerQueries)
				results.AddRange(ContentDelivery.DynamicContent.Query.QueryCache.Execute(brokerQuery).Select(u => this.ComponentPresentationWithHighestPriority(u)));

			return results;
		}
		
		/// <summary>
		/// Gets the <see cref="I:System.Collections.Generic.IEnumerable{System.String}" /> taxonomy uris for a <see cref="P:publicationUri" />
		/// </summary>
		/// <param name="publicationUri">Publication Uri</param>
		/// <returns>
		///   <see cref="I:System.Collections.Generic.IEnumerable{System.String}" />
		/// </returns>
		public IEnumerable<String> Taxonomies(String publicationUri)
		{
			Logger.Debug("Taxonomies: publicationUri \"{0}\".", publicationUri);

			return Cache.Get<IEnumerable<String>>(
				String.Format("Taxonomies-{0}", publicationUri),
				() =>
				{
					return ContentDelivery.Taxonomies.KeywordCache.GetTaxonomies(publicationUri);
				},
				CacheRegion.Unknown,
				TcmUri.NullUri);
		}

		/// <summary>
		/// Gets the <see cref="T:TcmCDService.Contracts.Keyword" /> for a <see cref="P:keywordUri" />
		/// </summary>
		/// <param name="keywordUri">Keyword Uri</param>
		/// <returns>
		///   <see cref="T:TcmCDService.Contracts.Keyword" />
		/// </returns>
		public Contracts.Keyword Keyword(String keywordUri)
		{
			Logger.Debug("Keyword: keywordUri \"{0}\".", keywordUri);

			return Cache.Get<Contracts.Keyword>(
				String.Format("Keyword-{0}",
				keywordUri),
				() =>
				{
					using (Keyword keyword = ContentDelivery.Taxonomies.KeywordCache.GetKeyword(keywordUri))
					{
						return keyword.ToContract();
					}
				},
				CacheRegion.Taxonomy | CacheRegion.TanonomyKeywordCount | CacheRegion.TanonomyKeywordRelations,
				keywordUri);
		}

		/// <summary>
		/// Gets the root keyword for the given <see cref="P:taxonomyUri" />
		/// </summary>
		/// <param name="taxonomyUri">Taxonomy <see cref="T:TcmCDService.Tridion.TcmUri" /></param>
		/// <returns>
		///   <see cref="T:TcmCDService.Contracts.Keyword" />
		/// </returns>
		public Contracts.Keyword Keywords(String taxonomyUri)
		{
			Logger.Debug("Keywords: taxonomyUri \"{0}\".", taxonomyUri);

			return Keywords(taxonomyUri, null);
		}

		/// <summary>
		/// Gets the root keyword for the given <see cref="P:taxonomyUri" />
		/// </summary>
		/// <param name="taxonomyUri">Taxonomy <see cref="T:TcmCDService.Tridion.TcmUri" /></param>
		/// <param name="taxonomyFilter"><see cref="T:TcmCDService.Contracts.TaxonomyFilter" /> to apply.</param>
		/// <returns>
		///   <see cref="T:TcmCDService.Contracts.Keyword" />
		/// </returns>
		public Contracts.Keyword Keywords(String taxonomyUri, Contracts.TaxonomyFilter taxonomyFilter)
		{
			return Keywords(taxonomyUri, taxonomyFilter, Contracts.TaxonomyFormatter.Hierarchy);
		}

		/// <summary>
		/// Gets the root keyword for the given <see cref="P:taxonomyUri" />
		/// </summary>
		/// <param name="taxonomyUri">Taxonomy <see cref="T:TcmCDService.Tridion.TcmUri" /></param>
		/// <param name="taxonomyFilter"><see cref="T:TcmCDService.Contracts.TaxonomyFilter" /> to apply.</param>
		/// <returns>
		///   <see cref="T:TcmCDService.Contracts.Keyword" />
		/// </returns>
		public Contracts.Keyword Keywords(String taxonomyUri, Contracts.TaxonomyFilter taxonomyFilter, Contracts.TaxonomyFormatter taxonomyFormatter)
		{
			Logger.Debug("Keywords: taxonomyUri \"{0}\", taxonomyFilter \"{1}\".", taxonomyUri, taxonomyFilter != null ? "<Object>" : String.Empty);

			return Cache.Get<Contracts.Keyword>(
				String.Format("Keywords-{0}-{1}-{2}-{3}-{4}-{5}-{6}-{7}", 
				taxonomyUri,
				taxonomyFilter != null ? taxonomyFilter.DepthFilteringDirection.ToString() : String.Empty,
				taxonomyFilter != null ? taxonomyFilter.DepthFilteringLevel.ToString() : String.Empty,
				taxonomyFilter != null ? taxonomyFilter.FilterAbstract.ToString() : String.Empty,
				taxonomyFilter != null ? taxonomyFilter.FilterConcrete.ToString() : String.Empty,
				taxonomyFilter != null ? taxonomyFilter.FilterHasChildren.ToString() : String.Empty,
				taxonomyFilter != null ? taxonomyFilter.FilterNavigable.ToString() : String.Empty,
				taxonomyFormatter.ToString()),
				() =>
				{
					using (Keyword keyword = ContentDelivery.Taxonomies.KeywordCache.GetKeywords(taxonomyUri, taxonomyFilter, taxonomyFormatter))
					{
						return keyword.ToContract();
					}
				},
				CacheRegion.Taxonomy | CacheRegion.TanonomyKeywordCount | CacheRegion.TanonomyKeywordRelations,
				taxonomyUri);
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources.
		/// </summary>
		/// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
		protected virtual void Dispose(Boolean disposing)
		{
			if (disposing)
			{
				if (mCacheType != null)
					mCacheType.Dispose();

				foreach (HealthChecks.HealthCheckType healthCheckType in mHealthCheckTypes)
					healthCheckType.Dispose();
			}
		}
	}
}
