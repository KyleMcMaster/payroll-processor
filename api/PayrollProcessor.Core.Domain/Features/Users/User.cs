using System;
using PayrollProcessor.Core.Domain.Features.UserSessions;

namespace PayrollProcessor.Core.Domain.Features.Users
{
    public class User
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public string Email { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Status { get; set; } = "";
        public string Version { get; set; } = "";

        public User(Guid id) => Id = id;
    }

    public static class UserExtensions
    {
        public static UserSession ToUserSession(this User user) =>
            new UserSession(user.Id)
            {
                Id = user.Id,
                AccountId = user.AccountId,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.Phone,
                Status = user.Status,
                Version = user.Version
            };
    }
}
