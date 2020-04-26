namespace PayrollProcessor.Functions.Features.Resources
{
    public static class Resource
    {
        public static class Table
        {
            public const string Employees = "employees";
            public const string Payrolls = "payrolls";
            public const string EmployeePayrolls = "employeePayrolls";
        }

        public static class Queue
        {
            public const string PayrollUpdates = "payroll-updates";
        }
    }
}
