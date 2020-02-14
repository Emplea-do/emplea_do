using Domain;
using Domain.Entities;
using Data.Repositories;
using System.Collections.Generic;
using System.Linq;
using AppServices.Services;
using AppServices.Framework;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using Microsoft.EntityFrameworkCore;

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
            return _mainRepository
                .Get(x=>x.UserId == userId && x.IsActive)
                .Include(x => x.Company)
                .Include(x => x.Category)
                .Include(x => x.HireType)
                .Include(x => x.Location)
                .OrderByDescending(x=>x.PublishedDate).ToList();
        }

        public Job GetDetails(int id, bool isPreview = false)
        {
            var job = _mainRepository.Get(j => j.IsActive && j.Id == id, "Company,Location")
                .FirstOrDefault();

            return job;
        }

        public override List<Job> GetAll()
        {
                return _mainRepository.Get(x => x.IsActive, "Company").ToList();
        }

        public IEnumerable<Job> GetRecentJobs()
        {
            return _mainRepository.Get(x=>x.IsActive && !x.IsHidden)
                .Include(x => x.Company)
                .Include(x => x.Category)
                .Include(x => x.HireType)
                .Include(x => x.Location)
                .OrderByDescending(x => x.PublishedDate).Take(10).ToList();
        }

        public Job GetById(int id)
        {
            return _mainRepository
                .Get(x => x.IsActive && x.Id == id)
                .Include(x => x.Company)
                .Include(x => x.Category)
                .Include(x => x.HireType)
                .Include(x => x.Location)
                .FirstOrDefault();
        }
    }

    public interface IJobsService : IBaseService<Job, IJobsRepository>
    {
        List<Job> GetByUser(int userid);

        Job GetById(int id);

        IEnumerable<Job> GetRecentJobs();

        Job GetDetails(int id, bool isPreview = false);
    }

    public class MockJobsService : IJobsService
    {
        public TaskResult<Job> Create(Job entity)
        {
            throw new System.NotImplementedException();
        }

        public TaskResult Delete(Job entity)
        {
            throw new System.NotImplementedException();
        }

        public List<Job> GetAll() => new List<Job>(getMockRecords());

        public Job GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        public List<Job> GetByUser(int id)
        {
            throw new System.NotImplementedException();
        }

        public Job GetDetails(int id, bool isPreview = false) => getMockRecords().First();

        public IEnumerable<Job> GetRecentJobs() => getMockRecords();

        public TaskResult<Job> Update(Job entity)
        {
            throw new System.NotImplementedException();
        }

        private IEnumerable<Job> getMockRecords() => new List<Job>
        {
            new Job
            {
                Id=1,
                Title = "Full Stack Web Developer",
                Description="Esto es un lorem ipsum",
                HowToApply="Para aplicar mandame un correo plz",
                PublishedDate = new System.DateTime(2019,10,01),
                Approved=true,
                ViewCount=150,
                Likes =34,
                IsRemote = true,
                Company= new Company
                {
                    Name = "Megsoft",
                    Url="https://megsoftconsulting.com/",
                    LogoUrl = "https://megsoftconsulting.com/wp-content/uploads/2018/08/my_business.png"
                },
                Category = new Category(){Name="Categoria", Description="Descripción de esta categoria"},
                Location = new Location(){Name="Remote"}, 
                HireType = new HireType(){Name="Trabajo completo"}
            },
            new Job
            {
                Id=2,
                Title = "Trabajo de prueba 2",
                Description="Esto es un lorem ipsum",
                HowToApply="Para aplicar mandame un correo plz",
                Approved=true,
                Company= new Company
                {
                    Url="https://megsoftconsulting.com/",
                    LogoUrl = "https://localhost:5001/img/logo.png"
                }
            },
            new Job
            {
                Id=14,
                Title = "Trabajo de prueba 4",
                Description="Esto es un lorem ipsum",
                HowToApply="Para aplicar mandame un correo plz",

                Company= new Company
                {
                    Url="https://megsoftconsulting.com/",
                    LogoUrl = "https://localhost:5001/img/logo.png"
                }
            },
            new Job
            {
                Id=7,
                Title = "Trabajo de prueba 55",
                Description="Esto es un lorem ipsum",
                HowToApply="Para aplicar mandame un correo plz",
                UserId=10,
                Approved = false,
                Company= new Company
                {
                    Url="https://megsoftconsulting.com/",
                    LogoUrl = "https://localhost:5001/img/logo.png"
                },
            },
            new Job
            {
                Id=16,
                Title = "Trabajo de prueba",
                Description="Esto es un lorem ipsum",
                HowToApply="Para aplicar mandame un correo plz",

                Company= new Company
                {
                    Url="https://megsoftconsulting.com/",
                    LogoUrl = "https://localhost:5001/img/logo.png"
                }
            }
        };
    }
}