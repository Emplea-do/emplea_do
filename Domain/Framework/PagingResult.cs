using System;
using System.Collections.Generic;

namespace Domain.Framework
{
    public class PagingResult<T>
    {
        public IEnumerable<T> Data { get; set; }
        public int Page { get; set; }
        public int ItemsPerPage { get; set; }
        public int TotalItems { get; set; }
    }
}
