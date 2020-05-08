using System;

namespace PayrollProcessor.Data.Domain.Intrastructure.Identifiers
{
    public interface IEntityIdGenerator
    {
        Guid Generate();
    }

    public class EntityIdGenerator : IEntityIdGenerator
    {
        public Guid Generate() => Guid.NewGuid();
    }
}
