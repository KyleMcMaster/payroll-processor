using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Mvc;
using PayrollProcessor.Api.Infrastructure.Responses;
using PayrollProcessor.Core.Domain.Features.Departments;
using PayrollProcessor.Core.Domain.Intrastructure.Operations.Queries;

namespace PayrollProcessor.Api.Features.Departments
{
    public class DepartmentPayrollsGet : BaseAsyncEndpoint<DepartmentPayrollsRequest, DepartmentPayrollsResponse>
    {
        private readonly IQueryDispatcher dispatcher;

        public DepartmentPayrollsGet(IQueryDispatcher dispatcher)
        {
            Guard.Against.Null(dispatcher, nameof(dispatcher));

            this.dispatcher = dispatcher;
        }

        public override Task<ActionResult<DepartmentPayrollsResponse>> HandleAsync(DepartmentPayrollsRequest request) =>
            dispatcher
                .Dispatch(new DepartmentPayrollsQuery(request.Count, request.Department, request.CheckDateFrom, request.CheckDateTo))
                .Match<IEnumerable<DepartmentPayroll>, ActionResult<DepartmentPayrollsResponse>>(
                    e => new DepartmentPayrollsResponse(e),
                    () => NotFound(),
                    ex => new APIErrorResult(ex.Message)
                );
    }

    public class DepartmentPayrollsRequest
    {
        public int Count { get; set; }
        public string Department { get; set; } = "";
        public DateTime? CheckDateFrom { get; set; }
        public DateTime? CheckDateTo { get; set; }
    }

    public class DepartmentPayrollsResponse : IListResponse<DepartmentPayroll>
    {
        public IEnumerable<DepartmentPayroll> Data { get; }

        public DepartmentPayrollsResponse(IEnumerable<DepartmentPayroll> data)
        {
            Guard.Against.Null(data, nameof(data));

            Data = data;
        }
    }
}
