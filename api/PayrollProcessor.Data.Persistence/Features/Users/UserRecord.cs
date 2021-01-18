using System;
using PayrollProcessor.Core.Domain.Features.Users;
using PayrollProcessor.Data.Persistence.Infrastructure.Records;

namespace PayrollProcessor.Data.Persistence.Features.Users
{
    public class UserRecord : CosmosDBRecord
    {
        public Guid AccountId { get; set; }
        public string Email { get; set; } = "";
        public string EmailLower { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string FirstNameLower { get; set; } = "";
        public string LastName { get; set; } = "";
        public string LastNameLower { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Status { get; set; } = "";
        public string Version { get; set; } = "";

        public UserRecord() => Type = nameof(UserRecord);

        public static class Map
        {
            public static User ToUser(UserRecord entity) =>
                new User(entity.Id)
                {
                    AccountId = entity.AccountId,
                    Email = entity.Email,
                    FirstName = entity.FirstName,
                    LastName = entity.LastName,
                    Phone = entity.Phone,
                    Status = entity.Status,
                    Version = entity.ETag
                };

            public static UserRecord From(Guid newId, UserNew user) =>
                new UserRecord
                {
                    Id = newId,
                    AccountId = user.AccountId,
                    PartitionKey = user.AccountId.ToString(),
                    FirstName = user.FirstName,
                    FirstNameLower = user.FirstName.ToLowerInvariant(),
                    LastName = user.LastName,
                    LastNameLower = user.LastName.ToLowerInvariant(),
                    Email = user.Email,
                    EmailLower = user.Email.ToLowerInvariant(),
                    Phone = user.Phone,
                    Status = user.Status,
                };
        }
    }
}
