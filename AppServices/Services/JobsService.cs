﻿using System;
using System.Collections.Generic;
using System.Linq;
using Domain;

namespace AppServices.Services
{
    public class JobsService : BaseService<Job>, IJobsService
    {
        public override List<Job> GetAll()
        {
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
        }

        public Job GetDetails(int id, bool isPreview = false)
        {
            var jobList = this.GetAll();
            var job = jobList
                .Where(j => j.Id == id && j.Approved == !isPreview)
                .FirstOrDefault();

            return job;
        }
    }

    public interface IJobsService : IBaseService<Job>
    {
         Job GetDetails(int id, bool isPreview = false);
    }
}
