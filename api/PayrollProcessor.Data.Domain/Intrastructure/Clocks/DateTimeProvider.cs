using System;

namespace PayrollProcessor.Data.Domain.Intrastructure.Clocks
{
    public interface IDateTimeProvider
    {
        DateTime UtcNow();
        DateTime Now();
    }

    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now() => DateTime.Now;

        public DateTime UtcNow() => DateTime.UtcNow;
    }
}
