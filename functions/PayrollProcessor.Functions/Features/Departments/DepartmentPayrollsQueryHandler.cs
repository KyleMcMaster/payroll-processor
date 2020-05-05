using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LanguageExt;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using PayrollProcessor.Core.Domain.Features.Departments;

using static PayrollProcessor.Functions.Infrastructure.AppResources.CosmosDb;

namespace PayrollProcessor.Functions.Features.Departments
{
    public interface IDepartmentPayrollsQueryHandler
    {
        Task<IEnumerable<DepartmentPayroll>> GetMany(int count, string department, DateTime? startDate, DateTime? endDate);
    }

    public class DepartmentPayrollsQueryHandler : IDepartmentPayrollsQueryHandler
    {
        private readonly CosmosClient client;

        public DepartmentPayrollsQueryHandler(CosmosClient client) =>
            this.client = client ?? throw new ArgumentNullException(nameof(client));

        public async Task<IEnumerable<DepartmentPayroll>> GetMany(int count, string department, DateTime? startDate, DateTime? endDate)
        {
            var requestOptions = string.IsNullOrWhiteSpace(department)
                ? null
                : new QueryRequestOptions { PartitionKey = new PartitionKey(department) };

            var query = client
                .GetContainer(Databases.PayrollProcessor.Name, Databases.PayrollProcessor.Containers.Departments)
                .GetItemLinqQueryable<DepartmentPayrollEntity>(requestOptions: requestOptions)
                .Where(e => e.Type == nameof(DepartmentPayrollEntity));

            if (startDate is DateTime start)
            {
                query = query.Where(e => e.CheckDate >= start);
            }

            if (endDate is DateTime end)
            {
                query = query.Where(e => e.CheckDate < end);
            }

            if (count > 0)
            {
                query = query.Take(count);
            }

            var iterator = query.ToFeedIterator();

            var models = new List<DepartmentPayroll>();

            while (iterator.HasMoreResults)
            {
                foreach (var result in await iterator.ReadNextAsync())
                {
                    models.Add(DepartmentPayrollEntity.Map.ToDepartmentPayroll(result));
                }
            }

            return models;
        }
    }
}
