using System;

namespace PayrollProcessor.Core.Domain.Features.Departments
{
    public class DepartmentEmployee
    {
        public DepartmentEmployee(Guid id) => Id = id;

        public Guid Id { get; }
        public Guid EmployeeId { get; set; }
        public string Department { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Version { get; set; } = "";
    }
}
