using System;
using LanguageExt;

namespace Payroll.Processor.Functions.Infrastructure
{
    public static class EnvironmentSettings
    {
        public static Option<string> Get(string name)
        {
            string? envVal = Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);

            return string.IsNullOrWhiteSpace(envVal)
                ? Option<string>.None
                : Option<string>.Some(envVal);
        }
    }
}
