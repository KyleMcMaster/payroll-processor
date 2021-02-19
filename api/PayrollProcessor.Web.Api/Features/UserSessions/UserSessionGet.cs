using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Mvc;
using PayrollProcessor.Core.Domain.Features.Users;
using PayrollProcessor.Core.Domain.Features.UserSessions;
using PayrollProcessor.Core.Domain.Intrastructure.Operations.Queries;
using Swashbuckle.AspNetCore.Annotations;

namespace PayrollProcessor.Web.Api.Features.UserSessions
{
    public class UserSessionGet : BaseAsyncEndpoint<Guid, UserSession>
    {
        private readonly IQueryDispatcher dispatcher;

        public UserSessionGet(IQueryDispatcher dispatcher)
        {
            Guard.Against.Null(dispatcher, nameof(dispatcher));

            this.dispatcher = dispatcher;
        }

        [HttpGet("usersessions/{userId:Guid}"), MapToApiVersion("1")]
        [SwaggerOperation(
            Summary = "Gets a specific UserSession",
            Description = "Gets a specific usersession specified by the route parameter",
            OperationId = "UserSession.Get",
            Tags = new[] { "UserSession" })
        ]
        public override Task<ActionResult<UserSession>> HandleAsync([FromRoute] Guid userId, CancellationToken token) =>
            dispatcher
                .Dispatch(new UserByTokenQuery(userId), token)
                .Match<User, ActionResult<UserSession>>(
                    e => e.ToUserSession(),
                    () => UserSession.CreateUninitialized(),
                    ex => new APIErrorResult(ex.Message));
    }
}
