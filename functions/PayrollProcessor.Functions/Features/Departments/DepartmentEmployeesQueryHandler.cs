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
    public interface IDepartmentEmployeesQueryHandler
    {
        Task<IEnumerable<DepartmentEmployee>> GetMany(int count, string department);
    }

    public class DepartmentEmployeesQueryHandler : IDepartmentEmployeesQueryHandler
    {
        private readonly CosmosClient client;

        public DepartmentEmployeesQueryHandler(CosmosClient client) =>
            this.client = client ?? throw new ArgumentNullException(nameof(client));

        public async Task<IEnumerable<DepartmentEmployee>> GetMany(int count, string department)
        {
            var requestOptions = string.IsNullOrWhiteSpace(department)
                ? null
                : new QueryRequestOptions { PartitionKey = new PartitionKey(department) };

            var query = client
                .GetContainer(Databases.PayrollProcessor.Name, Databases.PayrollProcessor.Containers.Departments)
                .GetItemLinqQueryable<DepartmentEmployeeEntity>(requestOptions: requestOptions)
                .Where(e => e.Type == nameof(DepartmentEmployeeEntity));

            if (count > 0)
            {
                query = query.Take(count);
            }

            var iterator = query.ToFeedIterator();

            var models = new List<DepartmentEmployee>();

            while (iterator.HasMoreResults)
            {
                foreach (var result in await iterator.ReadNextAsync())
                {
                    models.Add(DepartmentEmployeeEntity.Map.ToDepartmentEmployee(result));
                }
            }

            return models;
        }
    }
}
