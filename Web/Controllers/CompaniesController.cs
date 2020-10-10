using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppServices.Framework;
using AppServices.Services;
using ElmahCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Framework.Helpers.Alerts;
using Web.ViewModels;
using Web.Framework;
using Domain.Entities;

namespace Web.Controllers
{
    public class CompaniesController : BaseController
    {
        private readonly ICompaniesService _companiesService;
        private readonly IJobsService _jobService;

        public CompaniesController(ICompaniesService companiesService, IJobsService jobsService)
        {
            _companiesService = companiesService;
            _jobService = jobsService;
        }

        public IActionResult Index()
        {
            var companies = _companiesService.GetAll();
            return View(companies);
        }
        
        [Authorize]
        public IActionResult Wizard(int? id)
        {
            var model = new CompanyViewModel();

            if (id.HasValue && id > 0)
            {
                var company = _companiesService.GetById(id.Value);

                if(company.UserId == _currentUser.UserId)
                {
                    model.Id = company.Id;
                    model.Name = company.Name;
                    model.Url = company.Url;
                    model.LogoUrl = model.LogoUrl;
                    model.UserId = _currentUser.UserId;
                    model.Email = company.Email;
                }
                else
                { 
                    return RedirectToAction("Index", "UserProfile").WithError("No tienes permiso para editar esta Compañia");
                }
            }

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Wizard(CompanyViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                if(model.Id > 0) 
                {
                    var companyToUpdate = _companiesService.GetById(model.Id);

                    if(companyToUpdate.UserId == _currentUser.UserId)
                    {
                        companyToUpdate.Name = model.Name;
                        companyToUpdate.Url = model.Url;
                        companyToUpdate.LogoUrl = model.LogoUrl;
                        companyToUpdate.Email = model.Email;

                        var result = _companiesService.Update(companyToUpdate);

                        if(result.Success)
                        {
                            return RedirectToAction("Index", "UserProfile").WithSuccess("Compañia editada exitosamente");
                        }

                        return View(model).WithError(result.Messages);
                    }
                    else
                    { 
                        return RedirectToAction("Index", "UserProfile").WithError("No tienes permiso para editar esta Compañia");
                    }
                } else 
                {
                    var company = new Company
                    {
                        Name = model.Name,
                        Url = model.Url,
                        LogoUrl = model.LogoUrl,
                        UserId = _currentUser.UserId,
                        Email = model.Email
                    };

                    if (string.IsNullOrWhiteSpace(company.LogoUrl) ||
                        !company.LogoUrl.StartsWith("https") ||
                        (!company.LogoUrl.EndsWith(".jpg") &&
                            !company.LogoUrl.EndsWith(".jpeg") &&
                            !company.LogoUrl.EndsWith(".png")))
                    {
                        company.LogoUrl = $"{this.Request.Scheme}://{this.Request.Host}{Constants.DefaultLogoUrl}";
                    }

                    var result = _companiesService.Create(company);

                    if(result.Success)
                    {
                        return RedirectToAction("Index", "UserProfile").WithSuccess("Compañia creada exitosamente");
                    }

                    return View(model).WithError(result.Messages);
                }                  
            }
            catch(Exception ex)
            {
                HttpContext.RiseError(ex);
                return View(model).WithError(ex.Message);
            }
            
        }


        [Authorize]
        [HttpPost]
        public JsonResult Delete(int id)
        {
            var result = new TaskResult();

            try
            {
                var company = _companiesService.GetById(id);

                if(company == null)
                {
                    result.AddErrorMessage("No puedes eliminar una Compañia que no existe.");
                }
                else if(company.UserId != _currentUser.UserId)
                {
                    result.AddErrorMessage("No puedes eliminar una Compañia que no creaste.");
                }
                
                result = _companiesService.Delete(company);
            }
            catch(Exception ex)
            {
                HttpContext.RiseError(ex);
                result.AddErrorMessage(ex.Message);
            }
            
            return Json(result);
        }

        [HttpGet("Company/{Id}")]
        [AllowAnonymous]
        public IActionResult Jobs(int Id)
        {
            var model = new CompanyJobsViewModel
            {
                Jobs = _jobService.GetAllByCompanyId(Id),
                Company = _companiesService.GetById(Id)
            };

            return View(model);
        }
    }
}
