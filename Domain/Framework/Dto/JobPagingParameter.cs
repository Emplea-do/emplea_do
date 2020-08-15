using Domain.Entities;
using System;
using System.Collections.Generic;

namespace Domain.Framework.Dto
{
    public class JobPagingParameter
    {
        public string Keyword { get; set; }
        public string SelectedLocationPlaceId { get; set; } = string.Empty;
        public string SelectedLocationName { get; set; } = string.Empty;
        public double SelectedLocationLatitude { get; set; }
        public double SelectedLocationLongitude { get; set; }
        public double LocationDistance { get; set; } = 15;
        public int PageSize { get; set; } = 15;
        public int Page { get; set; } = 1;
        public int? CategoryId { get; set; }
        public bool IsRemote { get; set; }
        public IEnumerable<Job> Result { get; set; }

        public bool HasFilters
        {
            get
            {
                var result = !string.IsNullOrWhiteSpace(Keyword) || IsRemote;
                return result;
            }
        }
    }
}
