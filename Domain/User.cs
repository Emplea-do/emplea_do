using System;
using System.Collections.Generic;

namespace Domain
{
    public class User : Entity
    {
        public string LegacyId { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public string Salt { get; set; }

        public IList<Job> Jobs { get; set; }
        public IList<Company> Companies { get; set; }
        public IList<Role> Roles { get; set; }
    }
}
