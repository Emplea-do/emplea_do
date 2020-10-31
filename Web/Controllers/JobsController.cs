using System;
using Microsoft.AspNetCore.Mvc;
using AppServices;
using AppServices.Services;
using Web.ViewModels;
using Domain;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Web.Framework.Helpers.Alerts;
using Domain.Entities;
using AppServices.Framework;
using System.Linq;
using System.Collections.Generic;
using Web.Services.Slack;
using Newtonsoft.Json;
using Web.Models.Slack;
using System.Net;
using System.IO;
using System.Text;
using Web.Framework;
using ElmahCore;
using Web.Framework.Extensions;

namespace Web.Controllers
{
    public class JobsController : BaseController
    {
        private readonly IJobsService _jobsService;
        private readonly ICategoriesService _categoriesService;
        private readonly IHireTypesService _hiretypesService;
        private readonly ITwitterService _twitterService;
        private readonly LegacyApiClient _apiClient;
        private readonly IConfiguration _configuration;
        private readonly ICompaniesService _companiesService;
        private readonly ISlackService _slackService;

        public JobsController(IJobsService jobsService, ICategoriesService categoriesService, IHireTypesService hiretypesService,
            ITwitterService twitterService, LegacyApiClient apiClient, IConfiguration configuration,
            ICompaniesService companiesService, ISlackService slackService)
        {
            _jobsService = jobsService;
            _categoriesService = categoriesService;
            _hiretypesService = hiretypesService;
            _twitterService = twitterService;
            _apiClient = apiClient;
            _configuration = configuration;
            _companiesService = companiesService;
            _slackService = slackService;
        }

        public IActionResult Index(JobSeachViewModel model)
        {
            if (model == null)
            {
                model = new JobSeachViewModel();
            }

            bool? isOnlyRemotes = null;
            if (model.IsRemote)
                isOnlyRemotes = model.IsRemote;

            var jobs = _jobsService.Search(model.Keyword, model.CategoryId, model.HireTypeId, isOnlyRemotes);

            model.Jobs = jobs;

            model.Categories = _categoriesService.GetAll();
            model.HireTypes = _hiretypesService.GetAll();

            return View(model);
        }

        [Authorize]
        public IActionResult Wizard(int? id)
        {
            var model = new WizardViewModel
            {
                Categories = _categoriesService.GetAll(),
                JobTypes = _hiretypesService.GetAll(),
                Companies = _companiesService.GetByUserId(_currentUser.UserId)
            };

            if (id.HasValue)
            {
                var originalJob = _jobsService.GetById(id.Value);
                if (originalJob.UserId == _currentUser.UserId)
                {
                    model.Id = originalJob.Id;
                    model.CompanyId = originalJob.Company.Id;
                    model.CreateNewCompany = false;
                    model.Title = originalJob.Title;
                    model.Description = originalJob.Description;
                    model.HowToApply = originalJob.HowToApply;
                    model.CategoryId = originalJob.CategoryId;
                    model.JobTypeId = originalJob.HireTypeId;
                    model.IsRemote = originalJob.IsRemote;
                    model.LocationName = originalJob.Location.Name;
                    model.LocationPlaceId = originalJob.Location.PlaceId;
                    model.LocationLatitude = originalJob.Location.Latitude;
                    model.LocationLongitude = originalJob.Location.Longitude;
                }
                else
                {
                    return RedirectToAction("Index", "Home").WithError("No tienes permiso para editar esta posición");
                }
            }

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Wizard(WizardViewModel model)
        {
            model.Categories = _categoriesService.GetAll();
            model.JobTypes = _hiretypesService.GetAll();
            model.Companies = _companiesService.GetByUserId(_currentUser.UserId);

            if (ModelState.IsValid)
            {
                try
                {
                    var companyId = model.CompanyId;
                    if (model.CreateNewCompany)
                    {

                        var company = new Company
                        {
                            Name = model.CompanyName,
                            Url = model.CompanyUrl,
                            LogoUrl = model.CompanyLogoUrl,
                            UserId = _currentUser.UserId,
                            Email = model.CompanyEmail
                        };

                        if (string.IsNullOrWhiteSpace(company.LogoUrl) ||
                            !company.LogoUrl.StartsWith("https") ||
                            (!company.LogoUrl.EndsWith(".jpg") &&
                             !company.LogoUrl.EndsWith(".jpeg") &&
                             !company.LogoUrl.EndsWith(".png")))
                        {
                            company.LogoUrl = $"{this.Request.Scheme}://{this.Request.Host}{Constants.DefaultLogoUrl}";
                        }
                        _companiesService.Create(company);
                        companyId = company.Id;
                    }

                    if (model.Id.HasValue)
                    {
                        var originalJob = _jobsService.GetById(model.Id.Value);
                        if (originalJob.UserId == _currentUser.UserId)
                        {

                            originalJob.CategoryId = model.CategoryId;
                            originalJob.HireTypeId = model.JobTypeId;
                            originalJob.CompanyId = companyId.Value;
                            originalJob.HowToApply = model.HowToApply;
                            originalJob.Description = model.Description;
                            originalJob.Title = model.Title;
                            originalJob.IsRemote = model.IsRemote;
                            originalJob.IsApproved = false;
                            if (originalJob.Location.PlaceId != model.LocationPlaceId)
                            {
                                originalJob.Location = new Location
                                {
                                    PlaceId = model.LocationPlaceId,
                                    Name = model.LocationName,
                                    Longitude = model.LocationLongitude,
                                    Latitude = model.LocationLatitude
                                };
                            }
                            var result = _jobsService.Update(originalJob);
                            if (result.Success)
                            {
                                try
                                {
                                    await _slackService.PostJob(originalJob, Url);
                                }
                                catch (Exception ex)
                                {
                                    HttpContext.RiseError(ex);
                                }
                                return RedirectToAction("Wizard", new { Id = model.Id.Value }).WithSuccess("Posición editada exitosamente");
                            }

                            return View(model).WithError(result.Messages);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home").WithError("No tienes permiso para editar esta posición");
                        }
                    }
                    else
                    {
                        var newJob = new Job
                        {
                            CategoryId = model.CategoryId,
                            HireTypeId = model.JobTypeId,
                            CompanyId = companyId.Value,
                            HowToApply = model.HowToApply,
                            Description = model.Description,
                            Title = model.Title,
                            IsRemote = model.IsRemote,
                            Location = new Location
                            {
                                PlaceId = model.LocationPlaceId,
                                Name = model.LocationName,
                                Longitude = model.LocationLongitude,
                                Latitude = model.LocationLatitude
                            },
                            UserId = _currentUser.UserId,
                            IsHidden = false,
                            IsApproved = false,
                            PublishedDate = DateTime.Now
                        };
                        var result = _jobsService.Create(newJob);
                        if (result.Success)
                        {
                            try
                            {
                                await _slackService.PostJob(newJob, Url).ConfigureAwait(false);
                            }
                            catch (Exception ex)
                            {
                                HttpContext.RiseError(ex);
                            }

                            return RedirectToAction("Details", new { newJob.Id, isPreview = true }).WithInfo(result.Messages);
                        }

                        throw new Exception(result.Messages);
                    }

                }
                catch (Exception ex)
                {
                    HttpContext.RiseError(ex);
                    return View(model).WithError(ex.Message);
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Details(string Id, bool isPreview = false, bool isLegacy = false)
        {
            if (String.IsNullOrEmpty(Id))
                return RedirectToAction(nameof(this.Index));

            int jobId = Int32.Parse(Id);
            Job job = new Job();
            if (isLegacy)
            {
                var legacyJob = await _apiClient.GetJobById(Id);
                if (legacyJob != null)
                {
                    job = new Job()
                    {
                        Company = new Company()
                        {
                            Name = legacyJob.CompanyName,
                            LogoUrl = legacyJob.CompanyLogoUrl,
                            Url = legacyJob.Link,
                            Email = legacyJob.Email
                        },
                        Title = legacyJob.Title,
                        PublishedDate = legacyJob.PublishedDate,
                        Description = legacyJob.Description,
                        HowToApply = legacyJob.HowToApply,
                        IsRemote = legacyJob.IsRemote,
                        ViewCount = legacyJob.ViewCount,
                        Likes = legacyJob.Likes,
                        Location = new Location
                        {
                            Name = legacyJob.Location
                        },
                        HireType = new HireType
                        {
                            Description = legacyJob.JobType
                        }
                    };
                }
            }
            else
            {
                job = this._jobsService.GetDetails(jobId, isPreview);
            }

            if (job == null)
                return RedirectToAction(nameof(this.Index)).WithError("El puesto que buscas no existe.");

            ViewBag.Title = job.Title;
            ViewBag.Description = job.Description;
            var viewModel = new JobDetailsViewModel
            {
                Job = job,
                IsJobOwner = _currentUser.IsAuthenticated && job.UserId == _currentUser.UserId
            };

            if (!isLegacy)
            {
                //Get the list of jobs visited in the cookie
                //Format: comma separated jobs Id
                //Naming: appname_meanfulname
                var visitedJobs = Request.Cookies["empleado_visitedjobs"];

                //If cookie value is null (not set) use empty string to avoid NullReferenceException
                var visitedJobsList = (visitedJobs ?? string.Empty).Split(',', StringSplitOptions.RemoveEmptyEntries);

                //If jobs has not be visited update ViewCount & add job Id to cookie
                if (!visitedJobsList.Contains(Id))
                {
                    job.ViewCount++;
                    _jobsService.Update(job);

                    visitedJobs = string.Join(",", visitedJobsList.Append(Id));
                }

                Response.Cookies.Append("empleado_visitedjobs", visitedJobs);
            }

            if (isPreview)
            {
                viewModel.IsPreview = isPreview;
                return View(viewModel);
            }
            return View(viewModel);
        }

        private int GetJobIdFromTitle(string title)
        {
            var url = title.Split('-');
            if (String.IsNullOrEmpty(title) || title.Length == 0 || !int.TryParse(url[0], out int id))
                return 0;
            return id;
        }

        [Authorize]
        [HttpPost]
        public JsonResult Hide(int id)
        {
            var result = new TaskResult();
            try
            {
                var job = _jobsService.GetById(id);
                if (job == null)
                {
                    result.AddErrorMessage("No puedes esconder un puesto que no existe.");
                }
                else if (job.UserId == _currentUser.UserId)
                {
                    job.IsHidden = !job.IsHidden;
                    result = _jobsService.Update(job);
                }
                else
                {
                    result.AddErrorMessage("No puedes esconder un puesto que no creaste.");
                }
            }
            catch (Exception ex)
            {
                HttpContext.RiseError(ex);
                result.AddErrorMessage(ex.Message);
            }
            return Json(result);
        }

        [Authorize]
        [HttpPost]
        public JsonResult Delete(int id)
        {
            var result = new TaskResult();
            try
            {
                var job = _jobsService.GetById(id);

                if (job == null)
                {
                    result.AddErrorMessage("No puedes eliminar un puesto que no existe.");
                }
                else if (job.UserId == _currentUser.UserId)
                {
                    if (!job.IsActive)
                    {
                        result.AddErrorMessage("El puesto que intentas eliminar ya está eliminado.");
                    }
                    else
                    {
                        result = _jobsService.Delete(job);
                    }
                }
                else
                {
                    result.AddErrorMessage("No puedes eliminar un puesto que no creaste.");
                }
            }
            catch (Exception ex)
            {
                HttpContext.RiseError(ex);
                result.AddErrorMessage(ex.Message);
            }
            return Json(result);
        }


        /// <summary>
        /// Validates the payload response that comes from the Slack interactive message actions
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>s
        [HttpPost]
        [AllowAnonymous]
        public async Task Validate([FromForm] string payload)
        {
            try
            {
                var data = JsonConvert.DeserializeObject<PayloadResponseDto>(payload);

                if (data == null)
                {
                    throw new Exception($"Payload is null, Body: {payload}");
                }

                int jobOpportunityId = Convert.ToInt32(data.callback_id);
                var jobOpportunity = _jobsService.GetById(jobOpportunityId);
                var isJobApproved = data.actions.FirstOrDefault()?.value == "approve";
                var isJobRejected = data.actions.FirstOrDefault()?.value == "reject";
                var isTokenValid = data.token == _configuration["Slack:VerificationToken"];

                if (isTokenValid && isJobApproved)
                {
                    jobOpportunity.IsApproved = true;
                    jobOpportunity.PublishedDate = DateTime.UtcNow;
                    _jobsService.Update(jobOpportunity);
                    await _slackService.PostJobResponse(jobOpportunity, Url, data.response_url, data?.user?.id, true);

                    try
                    {
                        var tweetText = jobOpportunity.Title + " " + Url.AbsoluteUrl("Details", "Jobs", new { Id = jobOpportunityId });
                        await _twitterService.Tweet(tweetText);
                    }
                    catch (Exception tweetException)
                    {
                        HttpContext.RiseError(tweetException);
                        if (tweetException.InnerException != null)
                            HttpContext.RiseError(tweetException.InnerException);
                    }
                }
                else if (isTokenValid && isJobRejected)
                {
                    // Jobs are rejected by default, so there's no need to update the DB
                    if (jobOpportunity == null)
                    {
                        await _slackService.PostJobErrorResponse(jobOpportunity, Url, data.response_url);
                    }
                    else
                    {
                        await _slackService.PostJobResponse(jobOpportunity, Url, data.response_url, data?.user?.id, false);
                    }
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                }
            }
            catch (Exception ex)
            {
                HttpContext.RiseError(ex);
                if (ex.InnerException != null)
                    HttpContext.RiseError(ex.InnerException);
            }
        }
    }
}
