using Domain;
using Domain.Entities;
using Data.Repositories;
using System.Collections.Generic;
using System.Linq;
using AppServices.Services;
using AppServices.Framework;
using Microsoft.Extensions.Configuration;
using System.Configuration;

namespace AppServices.Services
{
    public class JobsService : BaseService<Job, IJobsRepository>, IJobsService
    {
        private readonly IConfiguration _config;

        public JobsService(IJobsRepository jobsRepository, IConfiguration config) : base(jobsRepository)
        {
            this._config = config;
        }

        protected override TaskResult<Job> ValidateOnCreate(Job entity)
        {
            return new TaskResult<Job>();
        }

        protected override TaskResult<Job> ValidateOnDelete(Job entity)
        {
            return new TaskResult<Job>();
        }

        protected override TaskResult<Job> ValidateOnUpdate(Job entity)
        {
            return new TaskResult<Job>();
        }
        public List<Job> GetByUser(int userId)
        {
            return _mainRepository.Get(x=>x.UserId == userId).OrderByDescending(x=>x.PublishedDate).ToList();
        }

        public Job GetDetails(int id, bool isPreview = false)
        {
            var job = _mainRepository.Get(j => j.Id == id && j.Approved == !isPreview, "Company")
                .FirstOrDefault();

            return job;
        }

        public override List<Job> GetAll()
        {
                return _mainRepository.Get(x => x.IsActive, "Company").ToList();
        }

        public IEnumerable<Job> GetRecentJobs()
        {
            return _mainRepository.Get(x=>x.IsActive && !x.IsHidden, "Company").OrderByDescending(x => x.PublishedDate).Take(10).ToList();
        }
    }

    public interface IJobsService : IBaseService<Job, IJobsRepository>
    {
        List<Job> GetByUser(int id);

        IEnumerable<Job> GetRecentJobs();

        Job GetDetails(int id, bool isPreview = false);
    }
}