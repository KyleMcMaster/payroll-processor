using System.Threading;
using Ardalis.GuardClauses;
using LanguageExt;
using Microsoft.Azure.Cosmos;
using PayrollProcessor.Core.Domain.Features.Users;
using PayrollProcessor.Core.Domain.Intrastructure.Operations.Commands;
using PayrollProcessor.Data.Persistence.Features.Users;

using static LanguageExt.Prelude;
using User = PayrollProcessor.Core.Domain.Features.Users.User;

namespace PayrollProcessor.Data.Persistence.Features.Employees
{
    public class UserCreateCommandHandler : ICommandHandler<UserCreateCommand, User>
    {
        private readonly CosmosClient client;

        public UserCreateCommandHandler(CosmosClient client)
        {
            Guard.Against.Null(client, nameof(client));

            this.client = client;
        }

        public TryAsync<User> Execute(UserCreateCommand command, CancellationToken token) =>
            UserRecord
                .Map
                .From(command.NewId, command.User)
                .Apply(record => client
                    .GetUsersContainer()
                    .CreateItemAsync(record, cancellationToken: token))
                .Apply(TryAsync)
                .Map(CosmosResponse.Unwrap)
                .Map(UserRecord.Map.ToUser);
    }
}
