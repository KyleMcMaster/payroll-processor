using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LanguageExt;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using PayrollProcessor.Core.Domain.Features.Payrolls;

using static PayrollProcessor.Functions.Infrastructure.AppResources.CosmosDb;

namespace PayrollProcessor.Functions.Features.Payrolls
{
    public interface IPayrollsQueryHandler
    {
        Task<IEnumerable<Payroll>> GetMany(int count);
    }

    public class PayrollsQueryHandler : IPayrollsQueryHandler
    {
        private readonly CosmosClient client;

        public PayrollsQueryHandler(CosmosClient client) =>
            this.client = client ?? throw new ArgumentNullException(nameof(client));

        public async Task<IEnumerable<Payroll>> GetMany(
            int count)
        {
            var query = client
                .GetContainer(Databases.PayrollProcessor.Name, Databases.PayrollProcessor.Containers.Payrolls)
                .GetItemLinqQueryable<PayrollEntity>()
                .Where(e => e.Type == nameof(Payroll));

            if (count > 0)
            {
                query = query.Take(count);
            }

            var iterator = query.ToFeedIterator();

            var models = new List<Payroll>();

            while (iterator.HasMoreResults)
            {
                foreach (var result in await iterator.ReadNextAsync())
                {
                    models.Add(PayrollEntity.Map.ToPayroll(result));
                }
            }

            return models;
        }
    }
}
