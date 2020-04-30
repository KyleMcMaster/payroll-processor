using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LanguageExt;
using Microsoft.WindowsAzure.Storage.Table;

namespace Payroll.Processor.Functions.Infrastructure
{
    public class TableQuerier
    {
        private readonly CloudTable table;

        public TableQuerier(CloudTable table)
        {
            if (table is null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            this.table = table;
        }

        public async Task<IEnumerable<TModel>> GetAllData<TModel, TEntity>(
            Func<TEntity, TModel> mapper
        ) where TEntity : ITableEntity, new()
        {
            var query = new TableQuery<TEntity>();
            var segment = await table.ExecuteQuerySegmentedAsync(query, null);

            return segment.Select(mapper);
        }

        public async Task<IEnumerable<TModel>> GetAllDataByPartitionKey<TModel, TEntity>(
            Func<TEntity, TModel> mapper,
            string partitionKey
        ) where TEntity : ITableEntity, new()
        {
            var filter = TableQuery.GenerateFilterCondition(
                "PartitionKey",
                QueryComparisons.Equal,
                partitionKey);

            var query = new TableQuery<TEntity>().Where(filter);

            var segment = await table.ExecuteQuerySegmentedAsync(query, null);

            return segment.Select(mapper);
        }

        public async Task<Option<TModel>> GetEntity<TEntity, TModel>(
            string partitionKey,
            string rowKey,
            Func<TEntity, TModel> mapper
        ) where TEntity : ITableEntity, new()
        {
            var tableOperation = TableOperation.Retrieve<TEntity>(partitionKey, rowKey);

            var tableResult = await table.ExecuteAsync(tableOperation);

            return tableResult.Result is TEntity entity
                ? Option<TModel>.Some(mapper(entity))
                : Option<TModel>.None;
        }
    }
}
