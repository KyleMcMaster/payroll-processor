using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace PayrollProcessor.Functions.Api.Infrastructure
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
