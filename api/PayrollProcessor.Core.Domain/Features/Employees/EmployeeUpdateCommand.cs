using System;
using Ardalis.GuardClauses;
using PayrollProcessor.Core.Domain.Intrastructure.Operations.Commands;

namespace PayrollProcessor.Core.Domain.Features.Employees
{
    public class EmployeeUpdateCommand : ICommand<Employee>
    {
        public string Email { get; } = "";
        public DateTimeOffset EmploymentStartedOn { get; }
        public string FirstName { get; } = "";
        public string LastName { get; } = "";
        public string Phone { get; } = "";
        public string Status { get; } = "";
        public string Title { get; } = "";
        public string Version { get; } = "";
        public Employee EntityToUpdate { get; }

        public EmployeeUpdateCommand(
            string email,
            DateTimeOffset employmentStartedOn,
            string firstName,
            string lastName,
            string phone,
            string status,
            string title,
            string version,
            Employee entityToUpdate)
        {
            Guard.Against.NullOrWhiteSpace(version, nameof(version));
            Guard.Against.Null(entityToUpdate, nameof(entityToUpdate));

            Email = email;
            EmploymentStartedOn = employmentStartedOn;
            FirstName = firstName;
            LastName = lastName;
            Phone = phone;
            Status = status;
            Title = title;
            Version = version;
            EntityToUpdate = entityToUpdate;
        }
    }
}
