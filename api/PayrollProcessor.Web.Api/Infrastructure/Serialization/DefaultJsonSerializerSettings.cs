using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace PayrollProcessor.Web.Api.Infrastructure.Serialization
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
