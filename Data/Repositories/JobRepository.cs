using System;
using System.Collections.Generic;
using System.Linq;
using Data.Extensions;
using Domain;
using Domain.Framework.Dto;
using Microsoft.EntityFrameworkCore;
using GeoCoordinatePortable;

namespace Data.Repositories
{
    public class JobRepository : BaseRepository<Job>, IJobRepository
    {
        ICategoryRepository _categoryRepository;
        IHireTypeRepository _hireTypeRepository;
        
        public JobRepository(EmpleadoDbContext database,
                             ICategoryRepository categoryRepository,
                             IHireTypeRepository hireTypeRepository) : base(database)
        {
            _categoryRepository = categoryRepository;
            _hireTypeRepository = hireTypeRepository;
        }

        public IQueryable<JobLimited> GetWithPagination(Func<Job, bool> where, int page, int itemsPerPage)
        {
            var query = Get(x => x.IsActive, "Company, Location").Where(where);

            var queryResult = query
                .Skip(page * itemsPerPage)
                .Take(itemsPerPage).Select(x => ConvertToJobLimited(x))
                .AsQueryable();

            return queryResult;
        }

        public JobLimited GetJobLimitedById(int id)
        {
            var x = Get(j => j.Id == id && j.IsActive, y => y.Company, z => z.Location)
                                   .FirstOrDefault();

            if (x == null) return null;

            return ConvertToJobLimited(x);
        }

        private JobLimited ConvertToJobLimited(Job job)
        {
            return new JobLimited
            {
                Id = job.Id,
                Title = job.Title,
                Description = string.IsNullOrWhiteSpace(job.Description) ? string.Empty : job.Description.RemoveHtml(),
                HowToApply = string.IsNullOrWhiteSpace(job.HowToApply) ? string.Empty : job.HowToApply.RemoveHtml(),

                CategoryId = job.CategoryId,
                CategoryName = job.CategoryId == 0 ? "No disponible" : _categoryRepository.GetById(job.CategoryId).Name,

                CompanyName = job.Company?.Name ?? string.Empty,
                CompanyUrl = job.Company?.Url ?? string.Empty,
                CompanyEmail = job.Company?.Email ?? string.Empty,
                CompanyLogoUrl = job.Company?.LogoUrl ?? string.Empty,

                HireTypeId = job.HireTypeId,
                //The legacy enum starts at 0, so we need to add some correction
                // TODO: Remove this correction after migration
                HireTypeName = job.HireTypeId == 0 ? "No disponible" : _hireTypeRepository.GetById(job.HireTypeId).Name,

                LocationName = job.Location?.Name ?? string.Empty,
                LocationLatitude = job.Location.Latitude,
                LocationLongitude = job.Location.Longitude,

                IsRemote = job.IsRemote,
                ViewCount = job.ViewCount,
                PublishedDateRaw = job.PublishedDate
            };
        }

        public IEnumerable<CategoryCountDto> GetJobCountByCategory()
        {
            return GetAll(x => x.Category)
                .GroupBy(x => new { x.Category.Name, x.CategoryId })
                    .Select(x => new CategoryCountDto()
                    {
                        Category = new Category() 
                        { 
                            Id = x.Key.CategoryId,
                            Name = x.Key.Name 
                        },
                        Count = x.Count()
                    })
                    .OrderBy(x => x.Name);
        }

        public IEnumerable<Job> GetLatestJobs(int quantity)
        {
            return GetAll(x => x.Category).Where(x => x.IsHidden == false && x.Approved == true)
                                                          .OrderByDescending(m => m.PublishedDate)
                                                          .ThenByDescending(x => x.Likes)
                                                          .Take(quantity)
                                                          .ToList();
        }

        public IEnumerable<Job> GetAllJobsPagedByFilters(JobPagingParameter parameter)
        {
            IEnumerable<Job> result = new List<Job>();
            
            if (parameter.Page <= 0)
                parameter.Page = 1;

            if (parameter.PageSize <= 0)
                parameter.PageSize = 15;

            var jobs = GetAll().Include(x => x.Location)
                               .Include(x => x.Company)
                               .OrderByDescending(x => x.PublishedDate)
                               .Where(x => x.IsHidden == false && x.Approved == true);

            //Filter by JobCategory
            if (parameter.CategoryId.HasValue)
                jobs = jobs.Where(x => x.Category.Id == parameter.CategoryId);

            if (parameter.IsRemote)
                jobs = jobs.Where(x => x.IsRemote);

            //Filter using FTS if keyword is not empty
            if (!string.IsNullOrWhiteSpace(parameter.Keyword))
                jobs = jobs.FullTextSearch(parameter.Keyword);

            //if no location selected just return pagination 
            if (string.IsNullOrWhiteSpace(parameter.SelectedLocationPlaceId))
            {
                result = jobs;

                return result;
            }

            //Query using Haversine formula ref.: http://www.wikiwand.com/en/Haversine_formula

            var locations = GetNearbyJobsLocations(parameter.SelectedLocationLatitude,
                parameter.SelectedLocationLongitude, parameter.LocationDistance);

            if (!locations.Any())
                return result;

            result = (from jo in jobs
                      where locations.Contains(jo.LocationId.Value)
                      select jo);

            return result;
        }

        private List<int> GetNearbyJobsLocations(double latitude, double longitude, double distance)
        {
            var coord = new GeoCoordinate(latitude, longitude);
            var query = Database.Set<Location>()
                                .Select(x => new
                                {
                                    Id = x.Id,
                                    Location = new GeoCoordinate(x.Latitude, x.Longitude)
                                })
                                .Where(x => x.Location.GetDistanceTo(coord) < distance)
                                .OrderBy(x => coord.GetDistanceTo(coord))
                                .Select(x => x.Id)
                                .ToList();
            return query;
        }
    }

    public interface IJobRepository : IBaseRepository<Job>
    {
        IQueryable<JobLimited>GetWithPagination(Func<Job,bool> where, int page, int itemsPerPage);
        JobLimited GetJobLimitedById(int id);
        IEnumerable<CategoryCountDto> GetJobCountByCategory();
        IEnumerable<Job> GetLatestJobs(int quantity);
        IEnumerable<Job> GetAllJobsPagedByFilters(JobPagingParameter parameter);
    }
}
