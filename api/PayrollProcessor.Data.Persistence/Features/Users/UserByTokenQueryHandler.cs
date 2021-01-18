using System.Threading;
using Ardalis.GuardClauses;
using LanguageExt;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json.Linq;
using PayrollProcessor.Core.Domain.Features.Users;
using PayrollProcessor.Core.Domain.Intrastructure.Operations.Queries;

using static LanguageExt.Prelude;
using User = PayrollProcessor.Core.Domain.Features.Users.User;

namespace PayrollProcessor.Data.Persistence.Features.Users
{
    public class UserByTokenQueryHandler : IQueryHandler<UserByTokenQuery, User>
    {
        private readonly CosmosClient client;

        public UserByTokenQueryHandler(CosmosClient client)
        {
            Guard.Against.Null(client, nameof(client));

            this.client = client;
        }

        public TryOptionAsync<User> Execute(UserByTokenQuery query, CancellationToken token = default)
        {
            string identifier = query.UserId.ToString();

            var iterator = client
                .GetUsersContainer()
                .GetItemQueryIterator<JObject>(requestOptions: new QueryRequestOptions
                {
                    PartitionKey = new PartitionKey(identifier)
                });

            return async () =>
            {
                UserRecord? userEntity = null;

                while (iterator.HasMoreResults)
                {
                    var response = await iterator.ReadNextAsync(token);

                    foreach (var item in response)
                    {
                        string type = item.Value<string>("type") ?? "";

                        if (userEntity is null && type == nameof(UserRecord))
                        {
                            var entity = item.ToObject<UserRecord>();

                            if (entity is UserRecord)
                            {
                                userEntity = entity;
                                break;
                            }
                        }
                    }
                }

                if (userEntity is null)
                {
                    return None;
                }

                return UserRecord.Map.ToUser(userEntity);
            };
        }
    }
}
