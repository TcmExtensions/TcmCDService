#region Header
////////////////////////////////////////////////////////////////////////////////////
//
//	File Description: Keyword Cache
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
using System.Linq;
using System.Text;
using TcmCDService.Logging;
using Tridion.ContentDelivery.Taxonomies;
using TcmCDService.Extensions;

namespace TcmCDService.ContentDelivery.Taxonomies
{
	/// <summary>
	///   <see cref="KeywordCache" /> handles internal caching of Tridion API objects.
	/// </summary>
	/// <remarks>Internally caching the expensive Java CodeMesh proxy classes significantly enhances performance.</remarks>
	internal class KeywordCache : TcmCDService.ContentDelivery.APICache<TaxonomyFactory, Keyword>
	{
		private static KeywordCache mInstance = new KeywordCache();

		/// <summary>
		/// Creates the cached <see cref="T:Tridion.ContentDelivery.Taxonomies.TaxonomyFactory" />
		/// </summary>
		/// <param name="publicationId">Publication id as <see cref="T:System.Int32" /></param>
		/// <returns>Cached<see cref="T:Tridion.ContentDelivery.Taxonomies.TaxonomyFactory" /></returns>
		protected override TaxonomyFactory CreateCachedItem(int publicationId)
		{
			return new TaxonomyFactory();
		}

		/// <summary>
		/// Gets the taxonomies for the given <see cref="P:publicationUri" />
		/// </summary>
		/// <param name="publicationUri">Publication <see cref="T:TcmCDService.Tridion.TcmUri" /></param>
		/// <returns><see cref="I:System.Collections.Generic.IEnumerable{System.String}" /></returns>
		internal static IEnumerable<String> GetTaxonomies(TcmUri publicationUri)
		{
			return mInstance.ProcessItem<IEnumerable<String>>(0, (factory) =>
			{
				return factory.GetTaxonomies(publicationUri);
			});
		}

		/// <summary>
		/// Gets the taxonomies for the given <see cref="P:keywordUri" />
		/// </summary>
		/// <param name="keywordUri">Keyword <see cref="T:TcmCDService.Tridion.TcmUri" /></param>
		/// <returns>
		///   <see cref="T:Tridion.ContentDelivery.Taxonomies.Keyword" />
		/// </returns>
		internal static Keyword GetKeyword(TcmUri keywordUri)
		{
			return mInstance.ProcessItem(0, (factory) =>
			{
				return factory.GetTaxonomyKeyword(keywordUri);
			});
		}

		/// <summary>
		/// Gets the root keyword for the given <see cref="P:taxonomyUri" />
		/// </summary>
		/// <param name="taxonomyUri">Taxonomy <see cref="T:TcmCDService.Tridion.TcmUri" /></param>
		/// <param name="taxonomyFilter"><see cref="T:TcmCDService.Contracts.TaxonomyFilter" /></param>
		/// <param name="taxonomyFilter"><see cref="T:TcmCDService.Contracts.TaxonomyFilter" /> to apply.</param>
		/// <returns><see cref="T:Tridion.ContentDelivery.Taxonomies.Keyword" /></returns>
		internal static Keyword GetKeywords(TcmUri taxonomyUri, Contracts.TaxonomyFilter taxonomyFilter, Contracts.TaxonomyFormatter taxonomyFormatter)
		{
			return mInstance.ProcessItem(0, (factory) =>
			{
				TaxonomyFormatter formatter;

				switch (taxonomyFormatter)
				{
					case Contracts.TaxonomyFormatter.List:
						formatter = new TaxonomyListFormatter();
						break;
					case Contracts.TaxonomyFormatter.HierarchyRelink:
						formatter = new TaxonomyHierarchyFormatter(true);
						break;
					default:
						formatter = new TaxonomyHierarchyFormatter();
						break;
				}

				if (taxonomyFilter == null)
					return factory.GetTaxonomyKeywords(taxonomyUri, formatter);
				else
				{
					using (TaxonomyFilter filter = taxonomyFilter.ToTaxonomyFilter())
					{
						return factory.GetTaxonomyKeywords(taxonomyUri, filter, formatter);
					}
				}
			});
		}
	}
}
