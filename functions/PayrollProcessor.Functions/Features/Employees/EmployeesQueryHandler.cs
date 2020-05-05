using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LanguageExt;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Newtonsoft.Json.Linq;
using PayrollProcessor.Core.Domain.Features.Employees;

using static PayrollProcessor.Functions.Infrastructure.AppResources.CosmosDb;

namespace PayrollProcessor.Functions.Features.Employees
{
    public interface IEmployeesQueryHandler
    {
        Task<IEnumerable<Employee>> GetMany(int count, string firstName, string lastName, string email);
        Task<Option<Employee>> Get(Guid employeeId);
        Task<Option<EmployeePayroll>> GetPayroll(Guid employeeId, Guid employeePayrollId);
        Task<Option<EmployeeDetails>> GetDetail(Guid employeeId);
    }

    public class EmployeesQueryHandler : IEmployeesQueryHandler
    {
        private readonly CosmosClient client;

        public EmployeesQueryHandler(CosmosClient client) =>
            this.client = client ?? throw new ArgumentNullException(nameof(client));

        public async Task<IEnumerable<Employee>> GetMany(
            int count, string firstName, string lastName, string email)
        {
            var query = client
                .GetContainer(Databases.PayrollProcessor.Name, Databases.PayrollProcessor.Containers.Employees)
                .GetItemLinqQueryable<EmployeeEntity>()
                .Where(e => e.Type == nameof(EmployeeEntity));

            if (!string.IsNullOrWhiteSpace(firstName))
            {
                query = query.Where(e => e.FirstName.Contains(firstName.ToLowerInvariant()));
            }

            if (!string.IsNullOrWhiteSpace(lastName))
            {
                query = query.Where(e => e.LastName.Contains(lastName.ToLowerInvariant()));
            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                query = query.Where(e => e.EmailLower.Contains(email.ToLowerInvariant()));
            }

            if (count > 0)
            {
                query = query.Take(count);
            }

            var iterator = query.ToFeedIterator();

            var models = new List<Employee>();

            while (iterator.HasMoreResults)
            {
                foreach (var result in await iterator.ReadNextAsync())
                {
                    models.Add(EmployeeEntity.Map.ToEmployee(result));
                }
            }

            return models;
        }

        public async Task<Option<Employee>> Get(Guid employeeId)
        {
            string identifier = employeeId.ToString();

            var entity = await client
                .GetContainer(Databases.PayrollProcessor.Name, Databases.PayrollProcessor.Containers.Employees)
                .ReadItemAsync<EmployeeEntity>(identifier, new PartitionKey(identifier));

            return entity is null
                ? Option<Employee>.None
                : Option<Employee>.Some(EmployeeEntity.Map.ToEmployee(entity));
        }

        public async Task<Option<EmployeePayroll>> GetPayroll(Guid employeeId, Guid employeePayrollId)
        {
            var entity = await client
                .GetContainer(Databases.PayrollProcessor.Name, Databases.PayrollProcessor.Containers.Employees)
                .ReadItemAsync<EmployeePayrollEntity>(employeePayrollId.ToString(), new PartitionKey(employeeId.ToString()));

            return entity is null
                ? Option<EmployeePayroll>.None
                : Option<EmployeePayroll>.Some(EmployeePayrollEntity.Map.ToEmployeePayroll(entity));
        }

        public async Task<Option<EmployeeDetails>> GetDetail(Guid employeeId)
        {
            var iterator = client
                .GetContainer(Databases.PayrollProcessor.Name, Databases.PayrollProcessor.Containers.Employees)
                .GetItemQueryIterator<JObject>(requestOptions: new QueryRequestOptions
                {
                    PartitionKey = new PartitionKey(employeeId.ToString())
                });

            EmployeeEntity? employeeEntity = null;
            var payrollEntities = new List<EmployeePayrollEntity>();

            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();

                foreach (var item in response)
                {
                    string type = item.Value<string>("type") ?? "";

                    switch (type)
                    {
                        case nameof(EmployeeEntity):
                            var entity = item.ToObject<EmployeeEntity>();

                            if (entity is EmployeeEntity)
                            {
                                employeeEntity = entity;
                            }

                            continue;

                        case nameof(EmployeePayrollEntity):
                            var payroll = item.ToObject<EmployeePayrollEntity>();

                            if (payroll is EmployeePayrollEntity)
                            {
                                payrollEntities.Add(payroll);
                            }

                            continue;

                        default:
                            continue;
                    }
                }
            }

            return employeeEntity is null
                ? Option<EmployeeDetails>.None
                : Option<EmployeeDetails>.Some(EmployeeEntity.Map.ToEmployeeDetails(employeeEntity, payrollEntities));
        }
    }
}
