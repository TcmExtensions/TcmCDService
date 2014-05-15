#region Header
////////////////////////////////////////////////////////////////////////////////////
//
//	File Description: Keywords
// ---------------------------------------------------------------------------------
//	Date Created	: April 6, 2014
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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TcmCDService.Contracts;

namespace TcmCDService.Remoting.Taxonomies
{
	/// <summary>
	///   <see cref="Keyword" /> connects to TcmCDService to provide Tridion taxonomy keywords retrieval
	/// </summary>
	public static class Keyword
	{
		/// <summary>
		/// Gets the <see cref="T:System.Collections.Generic.IEnumerable{System.String}" /> taxonomy uris for a <see cref="P:publicationUri" />
		/// </summary>
		/// <param name="publicationUri">Publication Uri</param>
		/// <returns>
		///   <see cref="T:System.Collections.Generic.IEnumerable{System.String}" />
		/// </returns>
		public static IEnumerable<String> GetTaxonomies(String publicationUri)
		{
			return RemoteAPI.Execute<IEnumerable<String>>((client) =>
				client.Service.Taxonomies(publicationUri), publicationUri);
		}

		/// <summary>
		/// Gets the <see cref="T:TcmCDService.Contracts.Keyword" /> for a <see cref="P:keywordUri" />
		/// </summary>
		/// <param name="keywordUri">Keyword Uri</param>
		/// <returns>
		///   <see cref="T:TcmCDService.Contracts.Keyword" />
		/// </returns>
		public static Contracts.Keyword Get(String keywordUri)
		{
			return RemoteAPI.Execute<Contracts.Keyword>((client) =>
				client.Service.Keyword(keywordUri), keywordUri);
		}

		/// <summary>
		/// Gets the root keyword for the given <see cref="P:taxonomyUri" />
		/// </summary>
		/// <param name="taxonomyUri">Taxonomy <see cref="T:TcmCDService.Tridion.TcmUri" /></param>
		/// <returns>
		///   <see cref="T:TcmCDService.Contracts.Keyword" />
		/// </returns>
		public static Contracts.Keyword GetKeywords(String taxonomyUri)
		{
			return RemoteAPI.Execute<Contracts.Keyword>((client) =>
				client.Service.Keywords(taxonomyUri), taxonomyUri);
		}

		/// <summary>
		/// Gets the root keyword for the given <see cref="P:taxonomyUri" />
		/// </summary>
		/// <param name="taxonomyUri">Taxonomy <see cref="T:TcmCDService.Tridion.TcmUri" /></param>
		/// <param name="taxonomyFilter"><see cref="T:TcmCDService.Contracts.TaxonomyFilter" /> to apply.</param>
		/// <returns>
		///   <see cref="T:TcmCDService.Contracts.Keyword" />
		/// </returns>
		public static Contracts.Keyword GetKeywords(String taxonomyUri, Contracts.TaxonomyFilter taxonomyFilter)
		{
			return RemoteAPI.Execute<Contracts.Keyword>((client) =>
				client.Service.Keywords(taxonomyUri, taxonomyFilter), taxonomyUri);
		}

		/// <summary>
		/// Gets the root keyword for the given <see cref="P:taxonomyUri" />
		/// </summary>
		/// <param name="taxonomyUri">Taxonomy <see cref="T:TcmCDService.Tridion.TcmUri" /></param>
		/// <param name="taxonomyFilter"><see cref="T:TcmCDService.Contracts.TaxonomyFilter" /> to apply.</param>
		/// <param name="taxonomyFormatter"><see cref="T:TcmCDService.Contracts.TaxonomyFormatter" /> to apply.</param>
		/// <returns>
		///   <see cref="T:TcmCDService.Contracts.Keyword" />
		/// </returns>
		public static Contracts.Keyword GetKeywords(String taxonomyUri, Contracts.TaxonomyFilter taxonomyFilter, Contracts.TaxonomyFormatter taxonomyFormatter)
		{
			return RemoteAPI.Execute<Contracts.Keyword>((client) =>
				client.Service.Keywords(taxonomyUri, taxonomyFilter, taxonomyFormatter), taxonomyUri);
		}
	}
}
