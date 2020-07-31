using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Role : Entity
    {
        public string Name { get; set; }

        public IList<Permission> Permissions { get; set; }
    }
}
