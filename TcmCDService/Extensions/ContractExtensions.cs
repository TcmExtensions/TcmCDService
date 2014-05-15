#region Header
////////////////////////////////////////////////////////////////////////////////////
//
//	File Description: Contract Extensions
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
using TcmCDService.ContentDelivery;
using TcmCDService.Logging;
using Tridion.ContentDelivery.DynamicContent;
using Tridion.ContentDelivery.Meta;
using Tridion.ContentDelivery.Taxonomies;

namespace TcmCDService.Extensions
{
	/// <summary>
	/// <see cref="ContractExtensions" /> provides functionality to quickly convert Tridion JVM classes
	/// to data contracts.
	/// </summary>
	internal static class ContractExtensions
	{
		/// <summary>
		/// Converts a <see cref="TcmCDService.Contracts.TaxonomyFilter" /> to <see cref="T:Tridion.ContentDelivery.Taxonomies.TaxonomyFilter" />
		/// </summary>
		/// <param name="taxonomyFilter"><see cref="TcmCDService.Contracts.TaxonomyFilter" /></param>
		/// <returns><see cref="T:Tridion.ContentDelivery.Taxonomies.TaxonomyFilter" /> or null</returns>
		internal static TaxonomyFilter ToTaxonomyFilter(this Contracts.TaxonomyFilter taxonomyFilter)
		{
			if (taxonomyFilter != null)
			{
				CompositeFilter filter = new CompositeFilter();

				if (taxonomyFilter.FilterConcrete.HasValue || taxonomyFilter.FilterAbstract.HasValue)
					filter.AbstractKeywordFiltering(taxonomyFilter.FilterConcrete.GetValueOrDefault(false),
						taxonomyFilter.FilterAbstract.GetValueOrDefault(false));

				if (taxonomyFilter.DepthFilteringLevel.HasValue)
					filter.DepthFiltering(taxonomyFilter.DepthFilteringLevel.GetValueOrDefault(-1), (int)taxonomyFilter.DepthFilteringDirection);

				if (taxonomyFilter.FilterHasChildren.HasValue || taxonomyFilter.FilterNavigable.HasValue)
					filter.PropertyKeywordFiltering(taxonomyFilter.FilterHasChildren.GetValueOrDefault(false),
						taxonomyFilter.FilterNavigable.GetValueOrDefault(false));

				return filter;
			}

			return null;
		}

		/// <summary>
		/// Converts a <see cref="T:Tridion.ContentDelivery.Meta.CustomMeta" /> to a <see cref="T:TcmCDService.Contracts.CustomMeta" /> data contract.
		/// </summary>
		/// <param name="customMeta"><see cref="T:Tridion.ContentDelivery.Meta.CustomMeta" /></param>
		/// <returns><see cref="I:System.Collections.Generic.IEnumerable{TcmCDService.Contracts.CustomMeta}" /></returns>
		internal static IEnumerable<Contracts.CustomMeta> ToContract(this CustomMeta customMeta)
		{
			if (customMeta != null)
			{
				List<Contracts.CustomMeta> result = new List<Contracts.CustomMeta>();
				
				foreach (NameValuePair metaValue in customMeta.NameValues.Values)
				{
					try
					{
						Contracts.CustomMeta contract = new Contracts.CustomMeta()
						{
							Name = metaValue.Name,
							DataType = (Contracts.CustomMetaType)metaValue.ValueType
						};

						switch (metaValue.ValueType)
						{
							case (int)Contracts.CustomMetaType.String:
								contract.StringValues = metaValue.MultipleValues.Cast<String>();
								break;
							case (int)Contracts.CustomMetaType.Date:
								contract.DateTimeValues = metaValue.MultipleValues.Cast<DateTime>().Select(d => DateTime.SpecifyKind(d, DateTimeKind.Unspecified));
								break;
							case (int)Contracts.CustomMetaType.Float:
								contract.FloatValues = metaValue.MultipleValues.Cast<float>();
								break;
						}

						result.Add(contract);
					}
					catch (Exception ex)
					{
						Logger.Error("CustomMeta: Error parsing value {0}.", ex, metaValue.Name);
					}
				}

				return result;
			}

			return null;
		}

		/// <summary>
		/// Converts a <see cref="T:Tridion.ContentDelivery.Meta.IComponentMeta" /> to a <see cref="T:TcmCDService.Contracts.ComponentMeta" /> data contract.
		/// </summary>
		/// <param name="componentMeta"><see cref="T:Tridion.ContentDelivery.Meta.IComponentMeta" /></param>
		/// <returns><see cref="T:TcmCDService.Contracts.ComponentMeta" /></returns>
		internal static Contracts.ComponentMeta ToContract(this IComponentMeta componentMeta)
		{
			if (componentMeta != null)
				return new Contracts.ComponentMeta()
				{
					Author = componentMeta.Author,
					CreationDate = componentMeta.CreationDate,
					CustomMeta = componentMeta.CustomMeta.ToContract(),
					Id = componentMeta.Id,
					InitialPublicationDate = componentMeta.InitialPublicationDate,
					IsMultimedia = componentMeta.IsMultimedia,
					LastPublicationDate = componentMeta.LastPublicationDate,
					MajorVersion = componentMeta.MajorVersion,
					MinorVersion = componentMeta.MinorVersion,
					ModificationDate = componentMeta.ModificationDate,
					OwningPublicationId = componentMeta.OwningPublicationId,
					PublicationId = componentMeta.PublicationId,
					SchemaId = componentMeta.SchemaId,
					Title = componentMeta.Title
				};

			return null;
		}

		/// <summary>
		/// Converts a <see cref="T:Tridion.ContentDelivery.Meta.ComponentPresentationMeta" /> to a <see cref="T:TcmCDService.Contracts.ComponentPresentationMeta" /> data contract.
		/// </summary>
		/// <param name="componentPresentationMeta"><see cref="T:Tridion.ContentDelivery.Meta.ComponentPresentationMeta" /></param>
		/// <returns><see cref="T:TcmCDService.Contracts.ComponentPresentationMeta" /></returns>
		internal static Contracts.ComponentPresentationMeta ToContract(this ComponentPresentationMeta componentPresentationMeta)
		{
			if (componentPresentationMeta != null)
				return new Contracts.ComponentPresentationMeta()
				{
					ComponentId = componentPresentationMeta.ComponentId,
					ContentType = componentPresentationMeta.ContentType,
					PublicationId = componentPresentationMeta.PublicationId,
					TemplateId = componentPresentationMeta.TemplateId,
					TemplatePriority = componentPresentationMeta.TemplatePriority
				};

			return null;
		}

		/// <summary>
		/// Converts a <see cref="T:Tridion.ContentDelivery.DynamicContent.ComponentPresentation" /> to a <see cref="T:TcmCDService.Contracts.ComponentPresentation" /> data contract.
		/// </summary>
		/// <param name="componentPresentation"><see cref="T:Tridion.ContentDelivery.DynamicContent.ComponentPresentation" /></param>
		/// <returns><see cref="T:TcmCDService.Contracts.ComponentPresentation" /></returns>
		internal static Contracts.ComponentPresentation ToContract(this ComponentPresentation componentPresentation)
		{
			if (componentPresentation != null)
				return new Contracts.ComponentPresentation()
				{
					ComponentId = componentPresentation.ComponentId,
					ComponentTemplateId = componentPresentation.ComponentTemplateId,
					Content = componentPresentation.Content,
					FileLocation = componentPresentation.FileLocation,
					Meta = componentPresentation.Meta.ToContract(),
					PublicationId = componentPresentation.PublicationId
				};

			return null;
		}

		/// <summary>
		/// Determines whether the specified <see cref="T:Tridion.ContentDelivery.Taxonomies.Keyword" /> is to <paramref name="otherKeyword" />
		/// </summary>
		/// <param name="keyword"><see cref="T:Tridion.ContentDelivery.Taxonomies.Keyword" /></param>
		/// <param name="otherKeyword"><see cref="T:Tridion.ContentDelivery.Taxonomies.Keyword" /></param>
		/// <returns><c>true</c> if the <see cref="T:TcmCDService.ContentDelivery.TcmUri" /> is equal, otherwise <c>false</c>.</returns>
		internal static Boolean IsEqual(this Keyword keyword, Contracts.Keyword otherKeyword)
		{
			if (keyword != null && otherKeyword != null)
				return (TcmUri)keyword.KeywordUri == (TcmUri)otherKeyword.Uri;

			return false;
		}

		/// <summary>
		/// Converts a <see cref="T:Tridion.ContentDelivery.Taxonomies.Keyword" /> to a <see cref="T:TcmCDService.Contracts.Keyword" /> data contract.
		/// </summary>
		/// <param name="keyword"><see cref="T:Tridion.ContentDelivery.Taxonomies.Keyword" /></param>
		/// <returns><see cref="T:TcmCDService.Contracts.Keyword" /></returns>
		internal static Contracts.Keyword ToContract(this Keyword keyword)
		{
			return keyword.ToContract(null);
		}

		/// <summary>
		/// Converts a <see cref="T:Tridion.ContentDelivery.Taxonomies.Keyword" /> to a <see cref="T:TcmCDService.Contracts.Keyword" /> data contract.
		/// </summary>
		/// <param name="keyword"><see cref="T:Tridion.ContentDelivery.Taxonomies.Keyword" /></param>
		/// <param name="linkingKeyword"><see cref="T:TcmCDService.Contracts.Keyword" /> from where the conversion is called.</param>
		/// <returns>
		///   <see cref="T:TcmCDService.Contracts.Keyword" />
		/// </returns>
		internal static Contracts.Keyword ToContract(this Keyword keyword, Contracts.Keyword linkingKeyword)
		{
			if (keyword != null)
			{
				Contracts.Keyword result = new Contracts.Keyword()
				{
					HasChildren = keyword.HasChildren,
					IsAbstract = keyword.IsAbstract,
					IsNavigable = keyword.IsNavigable,
					IsUsedForNavigation = keyword.IsUsedForIdentification,					
					Depth = keyword.KeywordDepth,
					Description = keyword.KeywordDescription,
					Key = keyword.KeywordKey,
					Left = keyword.KeywordLeft,
					Meta = ToContract(keyword.KeywordMeta),
					Name = keyword.KeywordName,
					Right = keyword.KeywordRight,
					Uri = keyword.KeywordUri,
					ReferencedContentCount = keyword.ReferencedContentCount,
					TaxonomyUri = keyword.TaxonomyUri,
					RelatedKeywordUris = keyword.GetRelatedKeywordUris()
				};

				// If the parent keyword is equal to the linkingKeyword, do not recursive, but assign it manually
				result.ParentKeyword = keyword
										.ParentKeyword
										.IsEqual(linkingKeyword) ? linkingKeyword : ToContract(keyword.ParentKeyword, result);

				// Retrieve all children which are not the same as the "linking keyword"
				IEnumerable<Keyword> children = keyword.KeywordChildren.OfType<Keyword>();
				result.Children = children.Where(k => !k.IsEqual(linkingKeyword)).Select(k => ToContract(k, result));

				// If the linking keyword is part of the children, add it manually to the children collection (To prevent recursing endlessly)
				if (children.Any(k => k.IsEqual(linkingKeyword)))
					result.Children = result.Children.Concat(new Contracts.Keyword[] { linkingKeyword });

				IEnumerable<Keyword> parents = keyword.ParentKeywords.OfType<Keyword>();
				result.ParentKeywords = parents.Where(k => !k.IsEqual(linkingKeyword)).Select(k => ToContract(k, linkingKeyword));

				// If the linking keyword is part of the parents, add it manually to the parents collection (To prevent recursing endlessly)
				if (parents.Any(k => k.IsEqual(linkingKeyword)))
					result.ParentKeywords = result.ParentKeywords.Concat(new Contracts.Keyword[] { linkingKeyword });

				return result;
			}

			return null;
		}

	}
}
