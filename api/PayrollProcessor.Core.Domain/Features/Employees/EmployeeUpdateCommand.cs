using System;
using Ardalis.GuardClauses;
using PayrollProcessor.Core.Domain.Intrastructure.Operations.Commands;

namespace PayrollProcessor.Core.Domain.Features.Employees;

public class EmployeeUpdateCommand : ICommand<Employee>
{
    public string Email { get; } = string.Empty;
    public DateTimeOffset EmploymentStartedOn { get; }
    public string FirstName { get; } = string.Empty;
    public string LastName { get; } = string.Empty;
    public string Phone { get; } = string.Empty;
    public string Status { get; } = string.Empty;
    public string Title { get; } = string.Empty;
    public string Version { get; } = string.Empty;
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
