using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace PayrollProcessor.Core.Domain.Infrastructure.Serialization
{
    public static class DefaultJsonSerializerSettings
    {
        public static JsonSerializerSettings JsonSerializerSettings { get; } =
            new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
    }
}
