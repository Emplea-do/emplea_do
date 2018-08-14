using System;
using System.Collections.Generic;
using Domain;
using Domain.Framework.Dto;
using Sakura.AspNetCore;

namespace Web.ViewModels
{
    public class JobSearchViewModel
    {
        public string SelectedLocationPlaceId { get; set; }
        public string SelectedLocationName { get; set; }
        public double SelectedLocationLatitude { get; set; }
        public double SelectedLocationLongitude { get; set; }
        public decimal LocationDistance { get; set; } = 15M;
        public IPagedList<Job> Jobs { get; set; }
        public string Keyword { get; set; }
        public int? CategoryId { get; set; }
        public bool IsRemote { get; set; }
        public IEnumerable<CategoryCountDto> CategoriesCount { get; set; }
    }
}
