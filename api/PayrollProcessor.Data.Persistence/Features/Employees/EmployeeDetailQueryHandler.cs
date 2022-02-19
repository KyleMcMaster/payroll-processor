using System.Collections.Generic;
using System.Threading;
using Ardalis.GuardClauses;
using LanguageExt;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json.Linq;
using PayrollProcessor.Core.Domain.Features.Employees;
using PayrollProcessor.Core.Domain.Intrastructure.Operations.Queries;

using static LanguageExt.Prelude;

namespace PayrollProcessor.Data.Persistence.Features.Employees;

public class EmployeeDetailQueryHandler : IQueryHandler<EmployeeDetailQuery, EmployeeDetail>
{
    private readonly CosmosClient client;

    public EmployeeDetailQueryHandler(CosmosClient client)
    {
        Guard.Against.Null(client, nameof(client));

        this.client = client;
    }

    public TryOptionAsync<EmployeeDetail> Execute(EmployeeDetailQuery query, CancellationToken token = default)
    {
        var iterator = client
            .GetEmployeesContainer()
            .GetItemQueryIterator<JObject>(requestOptions: new QueryRequestOptions
            {
                PartitionKey = new PartitionKey(query.EmployeeId.ToString())
            });

        return async () =>
        {
            EmployeeRecord? employeeEntity = null;
            var payrollEntities = new List<EmployeePayrollRecord>();

            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync(token);

                foreach (var item in response)
                {
                    string type = item.Value<string>("type") ?? string.Empty;

                    switch (type)
                    {
                        case nameof(EmployeeRecord):
                            var entity = item.ToObject<EmployeeRecord>();

                            if (entity is EmployeeRecord)
                            {
                                employeeEntity = entity;
                            }

                            continue;

                        case nameof(EmployeePayrollRecord):
                            var payroll = item.ToObject<EmployeePayrollRecord>();

                            if (payroll is EmployeePayrollRecord)
                            {
                                payrollEntities.Add(payroll);
                            }

                            continue;

                        default:
                            continue;
                    }
                }
            }

            if (employeeEntity is null)
            {
                return None;
            }

            return EmployeeRecord.Map.ToEmployeeDetails(employeeEntity, payrollEntities);
        };
    }
}
