using Newtonsoft.Json;

namespace PayrollProcessor.Functions.Infrastructure
{
    public static class Response
    {
        public static string Generate<T>(T value)
        {
            return JsonConvert.SerializeObject(value: value, settings: DefaultJsonSerializerSettings.JsonSerializerSettings);
        }
    }
}
