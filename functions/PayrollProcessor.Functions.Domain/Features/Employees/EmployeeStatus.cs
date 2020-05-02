using System;
using System.Collections.Generic;
using System.Linq;

namespace PayrollProcessor.Functions.Domain.Features.Employees
{
    public class EmployeeStatus
    {
        public string CodeName { get; set; }
        public string DisplayName { get; set; }

        protected EmployeeStatus(string codeName, string displayName)
        {
            CodeName = codeName;
            DisplayName = displayName;
        }

        public static readonly EmployeeStatus Enabled = new EmployeeStatus(nameof(Enabled), "Enabled");
        public static readonly EmployeeStatus Disabled = new EmployeeStatus(nameof(Disabled), "Disabled");
        public static readonly EmployeeStatus Unknown = new EmployeeStatus(nameof(Unknown), "Unknown");

        public static readonly IEnumerable<EmployeeStatus> All = new[] { Enabled, Disabled };

        public static EmployeeStatus Find(string codeName) =>
            All.FirstOrDefault(s => s.CodeName.Equals(codeName, StringComparison.OrdinalIgnoreCase)) ?? Unknown;
    }
}
