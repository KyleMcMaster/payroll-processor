using System;

namespace PayrollProcessor.Core.Domain.Features.Users
{
    public class UserNew
    {
        public Guid AccountId { get; set; }
        public string Email { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Status { get; set; } = "";
    }
}
