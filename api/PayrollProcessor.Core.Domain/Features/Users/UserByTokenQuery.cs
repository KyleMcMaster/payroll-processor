using System;
using Ardalis.GuardClauses;
using PayrollProcessor.Core.Domain.Intrastructure.Operations.Queries;

namespace PayrollProcessor.Core.Domain.Features.Users
{
    public class UserByTokenQuery : IQuery<User>
    {
        public Guid UserId { get; }

        public UserByTokenQuery(Guid userId)
        {
            Guard.Against.Null(userId, nameof(userId));

            UserId = userId;
        }
    }
}
