using System;
using System.Collections.Generic;

namespace Domain
{
    public class User : Entity
    {
        public int LegacyId { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Salt { get; set; }

        IList<Job> Jobs { get; set; }
        IList<Company> Companies { get; set; }
        IList<Role> Roles { get; set; }
    }
}
