using System;

namespace PayrollProcessor.Core.Domain.Features.UserSessions
{
    public class UserSession
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public string Email { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Status { get; set; } = "";
        public string Version { get; set; } = "";

        public UserSession(Guid id) => Id = id;

        public static UserSession CreateUninitialized() => new UserSession();

        private UserSession() { }
    }
}
