using System;
using System.Collections.Generic;
using System.Linq;

namespace PayrollProcessor.Core.Domain.Features.Employees;

public class EmployeeDepartment
{
    public string CodeName { get; set; }
    public string DisplayName { get; set; }

    protected EmployeeDepartment(string codeName, string displayName)
    {
        CodeName = codeName;
        DisplayName = displayName;
    }

    public static readonly EmployeeDepartment Human_Resources = new EmployeeDepartment(nameof(Human_Resources), "Human Resources");
    public static readonly EmployeeDepartment IT = new EmployeeDepartment(nameof(IT), "IT");
    public static readonly EmployeeDepartment Sales = new EmployeeDepartment(nameof(Sales), "Sales");
    public static readonly EmployeeDepartment Building_Services = new EmployeeDepartment(nameof(Building_Services), "Building Services");
    public static readonly EmployeeDepartment Marketing = new EmployeeDepartment(nameof(Marketing), "Marketing");
    public static readonly EmployeeDepartment Warehouse = new EmployeeDepartment(nameof(Warehouse), "Warehouse");

    public static readonly EmployeeDepartment Unknown = new EmployeeDepartment(nameof(Unknown), "Unknown");

    public static readonly IEnumerable<EmployeeDepartment> All = new[]
    {
            Human_Resources,
            IT,
            Sales,
            Building_Services,
            Marketing,
            Warehouse
        };

    public static EmployeeDepartment Find(string codeName) =>
        All.FirstOrDefault(s => s.CodeName.Equals(codeName, StringComparison.OrdinalIgnoreCase)) ?? Unknown;
}
