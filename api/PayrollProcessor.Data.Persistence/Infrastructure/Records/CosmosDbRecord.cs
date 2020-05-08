using System;
using Newtonsoft.Json;

namespace PayrollProcessor.Data.Persistence.Infrastructure.Records
{
    /// <summary>
    /// https://github.com/Azure/azure-cosmos-dotnet-v3/issues/165#issuecomment-489112642
    /// </summary>
    public abstract class CosmosDBRecord
    {
        /// <summary>
        /// The unique identifier of the entity
        /// </summary>
        /// <value></value>
        public Guid Id { get; set; }

        /// <summary>
        /// The partition key of the entity in its collection
        /// </summary>
        /// <value></value>
        public string PartitionKey { get; set; } = "";

        [JsonProperty(PropertyName = "_etag")]
        public string ETag { internal get; set; } = "";
        public string Type { get; set; } = "";
    }
}
