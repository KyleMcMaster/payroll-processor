using System;
using Ardalis.GuardClauses;
using PayrollProcessor.Core.Domain.Intrastructure.Operations.Commands;

namespace PayrollProcessor.Core.Domain.Features.Users
{
    public class UserCreateCommand : ICommand<User>
    {
        public Guid NewId { get; set; }
        public UserNew User { get; }

        public UserCreateCommand(Guid newId, UserNew user)
        {
            Guard.Against.Default(newId, nameof(newId));
            Guard.Against.Null(user, nameof(user));

            NewId = newId;
            User = user;
        }
    }
}
