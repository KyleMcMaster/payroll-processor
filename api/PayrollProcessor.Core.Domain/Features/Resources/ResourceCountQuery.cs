using PayrollProcessor.Core.Domain.Intrastructure.Operations.Queries;

namespace PayrollProcessor.Core.Domain.Features.Employees
{
    public class ResourceCountQuery : IQuery<ResourceCountQueryResponse> { }

    public class ResourceCountQueryResponse
    {
        public ResourceCountQueryResponse(int totalEmployees, int totalPayrolls)
        {
            TotalEmployees = totalEmployees;
            TotalPayrolls = totalPayrolls;
        }

        public int TotalEmployees { get; }
        public int TotalPayrolls { get; }
    }
}
