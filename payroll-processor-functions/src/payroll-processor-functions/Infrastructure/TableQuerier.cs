using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace PayrollProcessor.Functions.Infrastructure
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

        public async Task<IEnumerable<TModel>> GetAllData<TModel, TEntity>(Func<TEntity, TModel> mapper) where TEntity : ITableEntity, new()
        {
            var query = new TableQuery<TEntity>();
            var segment = await table.ExecuteQuerySegmentedAsync(query, null);

            return segment.Select(mapper);
        }
    }
}
