using Newtonsoft.Json;

namespace PayrollProcessor.Functions.Infrastructure
{
    /// <summary>
    /// https://github.com/Azure/azure-cosmos-dotnet-v3/issues/165#issuecomment-489112642
    /// </summary>
    public class CosmosDBEntity
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; } = "";

        [JsonProperty(PropertyName = "_etag")]
        public string ETag { internal get; set; } = "";

        public string Type { get; set; } = "";
    }
}
