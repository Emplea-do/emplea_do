using System;
using Domain;

namespace Web.ViewModels
{
    public class JobSearchViewModel
    {
        public string SelectedLocationPlaceId { get; set; }
        public string SelectedLocationName { get; set; }
        public string SelectedLocationLatitude { get; set; }
        public string SelectedLocationLongitude { get; set; }
        public decimal LocationDistance { get; set; } = 15M;
        //public IPagedList<Core.Domain.JobOpportunity> Result { get; set; } = new PagedList<Core.Domain.JobOpportunity>(null, 1, 15);
        public string Keyword { get; set; }
        public Category JobCategory { get; set; }
        public bool IsRemote { get; set; }
        //public List<JobCategoryCountDto> CategoriesCount { get; set; }
    }
}
