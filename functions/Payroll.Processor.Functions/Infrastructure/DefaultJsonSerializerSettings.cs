using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Payroll.Processor.Functions.Infrastructure
{
    static class DefaultJsonSerializerSettings
    {
        public static JsonSerializerSettings JsonSerializerSettings =>
            new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
    }
}
