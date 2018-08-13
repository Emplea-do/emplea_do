using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class Job : Entity
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string HowToApply { get; set; }

        public int ViewCount { get; set; }

        public int Likes { get; set; }

        public bool IsRemote { get; set; }

        public bool IsHidden { get; set; }

        public bool Approved { get; set; }

        public DateTime PublishedDate { get; set; }


        // Relationships with other entities

        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public int? CompanyId { get; set; }
        public virtual Company Company { get; set; }

        public int HireTypeId { get; set; }
        public virtual HireType HireType { get; set; }

        public int? JoelTestId { get; set; }
        public virtual JoelTest JoelTest { get; set; }

        public int? LocationId { get; set; }
        public virtual Location Location { get; set; }

        public int? UserId { get; set; }
        public virtual User User { get; set; }



    }
}
