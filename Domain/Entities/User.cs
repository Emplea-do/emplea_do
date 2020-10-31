using System;
using System.Collections.Generic;

namespace Domain.Entities

{
    public class User : Entity
    {
        //public string LegacyId { get; set; }

        public string Email { get; set; }
        public string Name { get; set; }
        public bool IsEnabledForBanners { get; set; }

        public IList<Job> Jobs { get; set; }
        public IList<Company> Companies { get; set; }
        public IList<Role> Roles { get; set; }
        public IList<Banner> Banners { get; set; }
    }
}
