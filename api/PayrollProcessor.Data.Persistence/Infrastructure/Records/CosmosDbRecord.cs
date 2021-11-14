using System;
using Newtonsoft.Json;

namespace PayrollProcessor.Data.Persistence.Infrastructure.Records;

/// <summary>
/// https://github.com/Azure/azure-cosmos-dotnet-v3/issues/165#issuecomment-489112642
/// </summary>
public abstract class CosmosDBRecord
{
    /// <summary>
    /// The unique identifier of the record
    /// </summary>
    /// <value></value>
    public Guid Id { get; set; }

    /// <summary>
    /// The partition key of the record in its collection
    /// </summary>
    /// <value></value>
    public string PartitionKey { get; set; } = "";

    /// <summary>
    /// The 'Version' of the record auto-managed by Cosmos and used
    /// for optimistic concurrency
    /// </summary>
    /// <value></value>
    [JsonProperty(PropertyName = "_etag")]
    public string ETag { internal get; set; } = "";

    /// <summary>
    /// The type discrimniator of the record, used when multiple
    /// different record types are stored in the same collection
    /// This is the name of the Record class.
    /// </summary>
    /// <value></value>
    public string Type { get; set; } = "";
}
