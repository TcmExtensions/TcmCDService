#region Header
////////////////////////////////////////////////////////////////////////////////////
//
//	File Description: Cache Region
// ---------------------------------------------------------------------------------
//	Date Created	: April 5, 2014
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
using System.Text;
using TcmCDService.Logging;

namespace TcmCDService.CacheTypes
{
	/// <summary>
	/// <see cref="CacheRegion" /> defines all the different cache regions
	/// </summary>
	[Flags]
	public enum CacheRegion : ulong
	{
		Unknown = 0UL << 0,
		/// <summary>
		/// com.tridion.broker.binaries.meta.BinaryMeta
		/// </summary>
		BinaryMeta = 1UL << 0,
		/// <summary>
		/// com.tridion.broker.Taxonomies.Taxonomy
		/// </summary>
		Taxonomy = 1UL << 1,
		/// <summary>
		/// com.tridion.broker.Taxonomies.Meta
		/// </summary>
		TaxonomyMeta = 1UL << 2,
		/// <summary>
		/// com.tridion.broker.Taxonomy.KeywordCount
		/// </summary>
		TanonomyKeywordCount = 1UL << 3,
		/// <summary>
		/// com.tridion.broker.Taxonomy.KeywordRelations
		/// </summary>
		TanonomyKeywordRelations = 1UL << 4,
		/// <summary>
		/// com.tridion.broker.meta.categorization.Category
		/// </summary>
		Category = 1UL << 5,
		/// <summary>
		/// com.tridion.linking.ComponentLink
		/// </summary>
		ComponentLink = 1UL << 6,
		/// <summary>
		/// com.tridion.broker.components.meta.ComponentMeta
		/// </summary>
		ComponentMeta = 1UL << 7,
		/// <summary>
		/// com.tridion.storage.ItemMeta
		/// </summary>
		ItemMeta = 1UL << 8,
		/// <summary>
		/// com.tridion.storage.ComponentPresentation
		/// </summary>
		ComponentPresentation = 1UL << 9,
		/// <summary>
		/// com.tridion.storage.Schema
		/// </summary>
		Schema = 1UL << 10,
		/// <summary>
		/// com.tridion.storage.ReferenceEntry
		/// </summary>
		ReferenceEntry = 1UL << 11,
		/// <summary>
		/// com.tridion.broker.componentpresentations.meta.ComponentPresentationMeta
		/// </summary>
		ComponentPresentationMeta = 1UL << 12,
		/// <summary>
		/// com.tridion.personalization.CustomerCharacteristic
		/// </summary>
		CustomerCharacteristic = 1UL << 13,
		/// <summary>
		/// com.tridion.linking.PageLink
		/// </summary>
		PageLink = 1UL << 14,
		/// <summary>
		/// com.tridion.broker.pages.meta.PageMeta
		/// </summary>
		PageMeta = 1UL << 15,
		/// <summary>
		/// com.tridion.timeframes.Timeframe
		/// </summary>
		TimeFrame = 1UL << 16,
		/// <summary>
		/// com.tridion.tracking.components.TrackedComponent
		/// </summary>
		TrackedComponent = 1UL << 17,
		/// <summary>
		/// com.tridion.tracking.componentlinks.TrackedComponentLink
		/// </summary>
		TrackedComponentLink = 1UL << 18,
		/// <summary>
		/// com.tridion.tracking.pages.TrackedPage
		/// </summary>
		TrackedPage = 1UL << 19,
		/// <summary>
		/// com.tridion.personalization.TrackingKey
		/// </summary>
		TrackingKey = 1UL << 20,
		/// <summary>
		/// com.tridion.user.User
		/// </summary>
		User = 1UL << 21,
		/// <summary>
		/// com.tridion.storage.XSLT
		/// </summary>
		XSLT = 1UL << 22,
		/// <summary>
		/// com.tridion.storage.QueryPlan
		/// </summary>
		QueryPlan = 1UL << 23,
		/// <summary>
		/// com.tridion.storage.QueryResult
		/// </summary>
		QueryResult = 1UL << 24,
		/// <summary>
		/// com.tridion.storage.BinaryContent
		/// </summary>
		BinaryContent = 1UL << 25,
		/// <summary>
		/// com.tridion.storage.ComponentLinkClick
		/// </summary>
		ComponentLinkClick = 1UL << 26,
		/// <summary>
		/// com.tridion.storage.ComponentVisit
		/// </summary>
		ComponentVisit = 1UL << 27,
		/// <summary>
		/// com.tridion.linking.PageLinkInfo
		/// </summary>
		PageLinkInfo = 1UL << 28,
		/// <summary>
		/// com.tridion.linking.ComponentLinkInfo
		/// </summary>
		ComponentLinkInfo = 1UL << 29,
		/// <summary>
		/// com.tridion.Transformer
		/// </summary>
		Transformer = 1UL << 30,
		/// <summary>
		/// com.tridion.TransformerResults
		/// </summary>
		TransformerResults = 1UL << 31,
		/// <summary>
		/// com.tridion.broker.components.meta.componentmeta.QueryResults
		/// </summary>
		ComponentMetaQuery = 1UL << 32,
		/// <summary>
		/// com.tridion.storage.publication
		/// </summary>
		Publication = 1UL << 33
	}

	/// <summary>
	/// <see cref="CacheRegionExtenions" /> provides <see cref="T:TcmCDService.CacheTypes.CacheRegion" /> conversion.
	/// </summary>
	public static class CacheRegionExtensions
	{
		private static Dictionary<String, CacheRegion> mLookup = new Dictionary<String, CacheRegion>(StringComparer.OrdinalIgnoreCase)
		{
			 { "/com.tridion.broker.binaries.meta.BinaryMeta", CacheRegion.BinaryMeta },
			 { "/com.tridion.broker.Taxonomies.Taxonomy", CacheRegion.Taxonomy },
			 { "/com.tridion.broker.Taxonomies.Meta", CacheRegion.TaxonomyMeta },
			 { "/com.tridion.broker.Taxonomy.KeywordCount", CacheRegion.TanonomyKeywordCount },
			 { "/com.tridion.broker.Taxonomy.KeywordRelations", CacheRegion.TanonomyKeywordRelations },
			 { "/com.tridion.broker.meta.categorization.Category", CacheRegion.Category },
			 { "/com.tridion.linking.ComponentLink", CacheRegion.ComponentLink },
			 { "/com.tridion.broker.components.meta.ComponentMeta", CacheRegion.ComponentMeta },
			 { "/com.tridion.storage.ItemMeta", CacheRegion.ItemMeta },
			 { "/com.tridion.storage.ComponentPresentation", CacheRegion.ComponentPresentation },
			 { "/com.tridion.storage.Schema", CacheRegion.Schema },
			 { "/com.tridion.storage.ReferenceEntry", CacheRegion.ReferenceEntry },
			 { "/com.tridion.broker.componentpresentations.meta.ComponentPresentationMeta", CacheRegion.ComponentPresentationMeta },
			 { "/com.tridion.personalization.CustomerCharacteristic", CacheRegion.CustomerCharacteristic },
			 { "/com.tridion.linking.PageLink", CacheRegion.PageLink },
			 { "/com.tridion.broker.pages.meta.PageMeta", CacheRegion.PageMeta },
			 { "/com.tridion.timeframes.Timeframe", CacheRegion.TimeFrame },
			 { "/com.tridion.tracking.components.TrackedComponent", CacheRegion.TrackedComponent },
			 { "/com.tridion.tracking.componentlinks.TrackedComponentLink", CacheRegion.TrackedComponentLink },
			 { "/com.tridion.tracking.pages.TrackedPage", CacheRegion.TrackedPage },
			 { "/com.tridion.personalization.TrackingKey", CacheRegion.TrackingKey },
			 { "/com.tridion.user.User", CacheRegion.User },
			 { "/com.tridion.storage.XSLT", CacheRegion.XSLT },
			 { "/com.tridion.storage.QueryPlan", CacheRegion.QueryPlan },
			 { "/com.tridion.storage.QueryResult", CacheRegion.QueryResult },
			 { "/com.tridion.storage.BinaryContent", CacheRegion.BinaryContent },
			 { "/com.tridion.storage.ComponentLinkClick", CacheRegion.ComponentLinkClick },
			 { "/com.tridion.storage.ComponentVisit", CacheRegion.ComponentVisit },
			 { "/com_tridion_linking_PageLinkInfo", CacheRegion.PageLinkInfo },
			 { "/com_tridion_linking_ComponentLinkInfo", CacheRegion.ComponentLinkInfo },
			 { "/com_tridion_Transformer", CacheRegion.Transformer },
			 { "/com_tridion_TransformerResults", CacheRegion.TransformerResults },
			 { "/com_tridion_broker_components_meta_componentmeta_QueryResults", CacheRegion.ComponentMetaQuery },
			 { "/com.tridion.storage.publication", CacheRegion.Publication }			 			 
		};

		private static Dictionary<CacheRegion, String> mReverse = new Dictionary<CacheRegion, String>();

		/// <summary>
		/// Initializes the <see cref="CacheRegionExtensions" /> class.
		/// </summary>
		static CacheRegionExtensions()
		{
			foreach (KeyValuePair<String, CacheRegion> entry in mLookup)
			{
				mReverse.Add(entry.Value, entry.Key);
			}
		}
		
		/// <summary>
		/// Convert a <see cref="T:System.String" /> value to a <see cref="T:TcmCDService.CacheTypes.CacheRegion" />
		/// </summary>
		/// <param name="value">Cache region as <see cref="T:System.String" /></param>
		/// <returns><see cref="T:TcmCDService.CacheTypes.CacheRegion" /></returns>
		public static CacheRegion ToCacheRegion(String value)
		{
			CacheRegion result;

			if (mLookup.TryGetValue(value, out result))
				return result;

			Logger.Warning("Unknown CacheRegion \"{0}\" encountered.", value);

			return CacheRegion.Unknown;
		}

		/// <summary>
		/// Convert a <see cref="T:TcmCDService.CacheTypes.CacheRegion" /> value to a <see cref="T:System.String" />
		/// </summary>
		/// <param name="value"><see cref="T:TcmCDService.CacheTypes.CacheRegion" /></param>
		/// <returns>Cache region as <see cref="T:System.String" /></returns>
		public static String FromCacheRegion(CacheRegion value)
		{
			String result;

			if (mReverse.TryGetValue(value, out result))
				return result;

			return null;
		}
	}
}
