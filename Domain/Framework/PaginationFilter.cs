using System;
using System.Collections.Generic;

namespace Domain.Framework
{
    public class PaginationFilter
    {
        public int Page { get; set; } = 1;
        public int ItemsPerPage { get; set; } = 10;
        public string ColumnToOrder { get; set; }
        public bool Ascending { get; set; } = true;
        public string Search { get; set; }
    }
}
