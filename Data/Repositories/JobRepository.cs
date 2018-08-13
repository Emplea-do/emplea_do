using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Data.Extensions;
using Domain;
using Domain.Framework.Dto;

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
                LocationLatitude = job.Location?.Latitude ?? string.Empty,
                LocationLongitude = job.Location?.Longitude ?? string.Empty,

                IsRemote = job.IsRemote,
                ViewCount = job.ViewCount,
                PublishedDateRaw = job.PublishedDate
            };
        }
    }

    public interface IJobRepository : IBaseRepository<Job>
    {
        IQueryable<JobLimited>GetWithPagination(Func<Job,bool> where, int page, int itemsPerPage);
        JobLimited GetJobLimitedById(int id);
    }
}
