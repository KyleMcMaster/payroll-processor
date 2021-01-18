using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Mvc;
using PayrollProcessor.Core.Domain.Features.Users;
using PayrollProcessor.Core.Domain.Intrastructure.Identifiers;
using PayrollProcessor.Core.Domain.Intrastructure.Operations.Commands;
using Swashbuckle.AspNetCore.Annotations;

namespace PayrollProcessor.Web.Api.Features.Users
{
    public class UserCreate : BaseAsyncEndpoint<UserCreateRequest, User>
    {
        private readonly ICommandDispatcher dispatcher;
        private readonly IEntityIdGenerator generator;

        public UserCreate(ICommandDispatcher dispatcher, IEntityIdGenerator generator)
        {
            Guard.Against.Null(dispatcher, nameof(dispatcher));
            Guard.Against.Null(generator, nameof(generator));

            this.dispatcher = dispatcher;
            this.generator = generator;
        }

        [HttpPost("users"), MapToApiVersion("1")]
        [SwaggerOperation(
            Summary = "Creates a new user",
            Description = "Creates a new user and returns the entity's id",
            OperationId = "Users.Create",
            Tags = new[] { "Users" })
        ]
        public override Task<ActionResult<User>> HandleAsync([FromBody] UserCreateRequest request, CancellationToken token)
        {
            var command = new UserCreateCommand(
                newId: generator.Generate(),
                user: new UserNew
                {
                    AccountId = request.AccountId,
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Phone = request.Phone,
                    Status = request.Status,
                });

            return dispatcher
                .Dispatch(command)
                .Match<User, ActionResult<User>>(
                    user => user,
                    ex => new APIErrorResult(ex.Message));
        }
    }

    public class UserCreateRequest
    {
        public Guid AccountId { get; set; }
        public string Email { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Status { get; set; } = "";
    }
}
