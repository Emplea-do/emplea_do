using Domain;
using Domain.Entities;
using Data.Repositories;
using System.Collections.Generic;
using System.Linq;
using AppServices.Services;
using AppServices.Framework;

namespace AppServices.Services
{
    public class JobsService : BaseService<Job, IJobsRepository>, IJobsService
    {
        public JobsService(IJobsRepository jobsRepository) : base(jobsRepository)
        {
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
        /*
public override List<Job> GetAll()
{
   return _jobsRepository.GetAll().ToList();
   /*
   return new List<Job>
   {
       new Job
       {
           Id=1,
           Title = "Trabajo de prueba",
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
       },
   };
    }*/

        public List<Job> GetByUser(int userId)
        {
            return _mainRepository.Get(x=>x.UserId == userId).OrderByDescending(x=>x.PublishedDate).ToList();
           /* return new List<Job>
            {
                new Job
                {
                    Id=1,
                    Title = "Trabajo de prueba",
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
                    Id=2,
                    Title = "Trabajo de prueba 2",
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
            };*/
        }

        public Job GetDetails(int id, bool isPreview = false)
        {
            var job = _mainRepository.Get(j => j.Id == id && j.Approved == !isPreview)
                .FirstOrDefault();

            return job;
        }

        public IEnumerable<Job> GetRecentJobs()
        {
            return _mainRepository.Get(x=>x.IsActive && !x.IsHidden).OrderByDescending(x => x.PublishedDate).Take(10).ToList();
            /*
            List<Job> jobsList = new List<Job>
            {
                new Job
                {
                    Id=1,
                    Title = "No hay trabajo",
                    Description="Esta no es una descripción para un trabajo",
                    HowToApply="No apliquen por favor",
                    Company= new Company
                    {
                        Url="https://megsoftconsulting.com/",
                        LogoUrl = "https://localhost:5001/img/logo.png"
                    }
                },
                new Job
                {
                    Id=2,
                    Title = "Trabajo de prueba 2",
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
            };
            var recentJobs = jobsList.OrderByDescending(x => x.CreatedAt).Take(10);
            return recentJobs;*/
        }
    }

    public interface IJobsService : IBaseService<Job, IJobsRepository>
    {
        List<Job> GetByUser(int id);

        IEnumerable<Job> GetRecentJobs();

        Job GetDetails(int id, bool isPreview = false);
    }
}