using System;
using System.Collections.Generic;
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
                new Job
                {
                    Id=7,
                    Title = "Trabajo de prueba 55",
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

        public List<Job> GetByUserProfile(int id)
        {
            return new List<Job>
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
            };
        }
    }

    public interface IJobsService : IBaseService<Job>
    {
        List<Job> GetByUserProfile(int id);
    }
}
