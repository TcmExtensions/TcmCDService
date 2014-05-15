#region Header
////////////////////////////////////////////////////////////////////////////////////
//
//	File Description: Query Cache
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
using TcmCDService.ContentDelivery;
using TcmCDService.Contracts;
using Tridion.ContentDelivery.DynamicContent.Query;

namespace TcmCDService.ContentDelivery.DynamicContent.Query
{
	/// <summary>
	///   <see cref="QueryCache" /> handles internal caching of Tridion API objects.
	/// </summary>
	/// <remarks>Internally caching the expensive Java CodeMesh proxy classes significantly enhances performance.</remarks>
	internal class QueryCache
	{
		/// <summary>
		/// Retrieves the <see cref="T:Tridion.ContentDelivery.DynamicContent.Query.FieldOperator" /> for this <see cref="SubQuery" />
		/// </summary>
		/// <returns><see cref="T:Tridion.ContentDelivery.DynamicContent.Query.FieldOperator" /></returns>
		private static FieldOperator GetOperator(QueryOperator queryOperator)
		{
			switch (queryOperator)
			{
				case QueryOperator.Equal:
					return Criteria.Equal;

				case QueryOperator.NotEqual:
					return Criteria.NotEqual;

				case QueryOperator.Like:
					return Criteria.Like;

				case QueryOperator.Less:
					return Criteria.LessThan;

				case QueryOperator.LessEqual:
					return Criteria.LessThanOrEqual;

				case QueryOperator.Greater:
					return Criteria.GreaterThan;

				case QueryOperator.GreaterEqual:
					return Criteria.GreaterThanOrEqual;

				default:
					return Criteria.Equal;
			}
		}

		/// <summary>
		/// Converts this <see cref="KeywordQuery" /> instance to <see cref="T:Tridion.ContentDelivery.DynamicContent.Query.Criteria" />
		/// </summary>
		/// <returns><see cref="T:Tridion.ContentDelivery.DynamicContent.Query.Criteria" /></returns>
		private static Criteria ToCriteria(KeywordQuery keywordQuery)
		{
			return new KeywordCriteria(keywordQuery.Field, keywordQuery.Value, GetOperator(keywordQuery.Operation));
		}

		private static Criteria ToCriteria(MetaQuery metaQuery)
		{
			switch (metaQuery.ValueType)
			{
				case QueryValueType.String:
					return new CustomMetaValueCriteria(new CustomMetaKeyCriteria(metaQuery.Field), metaQuery.Value, GetOperator(metaQuery.Operation));
				case QueryValueType.Date:
					DateTime dateValue;

					if (DateTime.TryParse(metaQuery.Value, out dateValue))
						return new CustomMetaValueCriteria(new CustomMetaKeyCriteria(metaQuery.Field), dateValue, GetOperator(metaQuery.Operation));

					break;
				case QueryValueType.Number:
					float numberValue;

					if (float.TryParse(metaQuery.Value, out numberValue))
						return new CustomMetaValueCriteria(new CustomMetaKeyCriteria(metaQuery.Field), numberValue, GetOperator(metaQuery.Operation));

					break;
			}

			return null;
		}

		/// <summary>
		/// Executes the specified <see cref="T:TcmCDService.Contracts.BrokerQuery" />
		/// </summary>
		/// <param name="brokerQuery"><see cref="T:TcmCDService.Contracts.BrokerQuery" /></param>
		/// <returns><see cref="I:System.Collections.Generic.IEnumerable{System.String}" /></returns>
		public static IEnumerable<String> Execute(BrokerQuery brokerQuery)
		{
			if (brokerQuery != null)
			{
				List<IDisposable> disposableItems = new List<IDisposable>();
				List<Criteria> criteria = new List<Criteria>();

				try
				{
					// Query for ItemType: Component
					if (brokerQuery.ItemType != 0)
						criteria.Add(new ItemTypeCriteria((int)brokerQuery.ItemType));

					// Query for Publication
					if (!String.IsNullOrEmpty(brokerQuery.Publication))
						criteria.Add(new PublicationCriteria(new TcmUri(brokerQuery.Publication).ItemId));

					// Query based on Schema
					if (brokerQuery.SchemaUris != null && brokerQuery.SchemaUris.Any())
						criteria.Add(CriteriaFactory.Or(brokerQuery.SchemaUris.Select((u) => 
						{
							ItemSchemaCriteria itemSchemaCriteria = new ItemSchemaCriteria(new TcmUri(u).ItemId);
							disposableItems.Add(itemSchemaCriteria);

							return itemSchemaCriteria;
						}).ToArray()));

					// Query based on Component Template
					if (!String.IsNullOrEmpty(brokerQuery.ComponentTemplateUri))
						criteria.Add(new ItemTemplateCriteria(new TcmUri(brokerQuery.ComponentTemplateUri).ItemId));

					// Add any SubQuery entries (MetaQuery or KeywordQueries which are specified)
					if (brokerQuery.SubQueries != null && brokerQuery.SubQueries.Any())
						criteria.AddRange(brokerQuery.SubQueries.Where(q => q != null).Select((q) =>
						{
							Criteria subCriteria = q is MetaQuery ? ToCriteria(q as MetaQuery) : ToCriteria(q as KeywordQuery);
							disposableItems.Add(subCriteria);

							return subCriteria;
						}));

					using (Tridion.ContentDelivery.DynamicContent.Query.Query query = new Tridion.ContentDelivery.DynamicContent.Query.Query(CriteriaFactory.And(criteria.ToArray())))
					{
						// Limit the amount of results
						using (LimitFilter filter = new LimitFilter(brokerQuery.ResultLimit.GetValueOrDefault(100)))
						{
							query.SetResultFilter(filter);
							query.AddSorting(new SortParameter(SortParameter.ItemModificationDate, SortParameter.Descending));

							return query.ExecuteQuery();
						}
					}
				}
				finally
				{
					// Ensure all created Java objects are disposed
					foreach (Criteria entry in criteria)
					{
						if (entry != null)
							entry.Dispose();
					}

					foreach (IDisposable entry in disposableItems)
					{
						if (entry != null)
							entry.Dispose();
					}
				}
			}

			return new String[] { };
		}
	}
}
