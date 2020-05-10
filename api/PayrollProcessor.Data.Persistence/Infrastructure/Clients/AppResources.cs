namespace PayrollProcessor.Data.Persistence.Infrastructure.Clients
{
    public static class AppResources
    {
        public static class Queue
        {
            public const string EmployeeUpdates = "employee-updates";
            public const string EmployeePayrollUpdates = "employee-payroll-updates";
        }

        public static class CosmosDb
        {
            public const string ServiceEndpoint = "CosmosDbServiceEndpoint";
            public const string AuthKey = "CosmosDbAuthKey";

            public static class Databases
            {
                public static class PayrollProcessor
                {
                    public const string Name = "PayrollProcessor";

                    public static class Containers
                    {
                        public const string Employees = "Employees";
                        public const string Departments = "Departments";
                    }
                }
            }
        }
    }
}
