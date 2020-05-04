namespace PayrollProcessor.Functions.Infrastructure
{
    public static class AppResources
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
                        public const string Payrolls = "Payrolls";
                    }
                }
            }
        }
    }
}
