using System;

namespace Payroll.Processor.Data.Domain.Features
{
    public class Risk
    {
        public Guid Id { get; }
        public string CodeName { get; }
        public string DisplayName { get; }

        public Risk(
            Guid id,
            string codeName,
            string displayName)
        {
            Id = id;
            CodeName = codeName;
            DisplayName = displayName;
        }
    }
}
