using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppService.Framework.Social;
using AppService.Services;
using Domain.Framework.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Web.Framework;
using Web.Framework.Helpers.Alerts;
using Web.ViewModels;
using Web.ViewModels.Jobs;
using Sakura.AspNetCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using AppService.Framework;
using Domain;
using Web.Framework.Extensions;
using Microsoft.AspNetCore.Mvc.Routing;
using AppService.Services.Social;
using Web.Framework.Helpers;

namespace Web.Controllers
{

    [Authorize]
    public class JobsController : BaseController
    {
        private readonly IJobService _jobService;
        private readonly ICategoryService _categoryService;
        private readonly IHireTypeService _hireTypeService;
        private readonly ISlackService _slackService;

        public JobsController(IOptions<SocialKeys> socialKeys,
                              IJobService jobsService,
                              ICategoryService categoryService,
                              IHireTypeService hireTypeService,
                              ISlackService slackServie) : base(socialKeys)
        {
            _jobService = jobsService;
            _categoryService = categoryService;
            _hireTypeService = hireTypeService;
            _slackService = slackServie;
        }

        public IActionResult Index(JobPagingParameter model)
        {
            var viewModel = GetSearchViewModel(model);

            if (!string.IsNullOrWhiteSpace(viewModel.SelectedLocationName) &&
                string.IsNullOrWhiteSpace(viewModel.SelectedLocationPlaceId))
            {
                ModelState.AddModelError("SelectedLocationName", "");
                return View(viewModel).WithError("Debe seleccionar una Localidad para buscar.");
            }

            var jobs = _jobService.GetAllJobsPagedByFilters(model);

            viewModel.Jobs = jobs.ToPagedList(model.PageSize, model.Page);

            return View(viewModel);
        }

        public IActionResult New()
        {
            return RedirectToAction("Wizard");
        }

        [HttpGet]
        public IActionResult Wizard()
        {
            var viewModel = new Wizard();
            var categories = _categoryService.GetCategories().ToList();
            var jobtypes = _hireTypeService.GetHireTypes().ToList();
            viewModel.Categories = categories;
            viewModel.JobTypes = jobtypes;
            viewModel.MapsApiKey = _socialKeys.GoogleMapsApiKey;
            ViewBag.IsEdit = false;
            return View(viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        //[ValidateInput(false)]
        //[CaptchaValidator(RequiredMessage = "Por favor confirma que no eres un robot", ErrorMessage = "El captcha es incorrecto.")]
        public async Task<IActionResult> Wizard(Wizard model)
        {
            var categories = _categoryService.GetCategories().ToList();
            var jobtypes = _hireTypeService.GetHireTypes().ToList();
            model.Categories = categories;
            model.JobTypes = jobtypes;
            model.MapsApiKey = _socialKeys.GoogleMapsApiKey;

            // HACK - For some reason the View.WithError is returning a blank page. I'm fully validating this on javascript.
            // Leaving this code for further fix
            if (!ModelState.IsValid)
                return View("Wizard", model).WithError("Han ocurrido errores de validación que no permiten continuar el proceso");
            
            var job = model.ToEntity();

            job.Approved = false; // new / updated jobs unapproved by default
            job.UserId = _applicationUser.RawId;
            var jobExists = _jobService.GetById(model.Id);
            if (jobExists == null)
            {
                var result = _jobService.Create(job);
                if(!result.ExecutedSuccesfully)
                {
                    return View(model)
                        .WithError("Ha ocurrido un problema al momento de registrar la información. Intentalo más tarde");
                }
            }
            else
            {
                var result = _jobService.Update(job);
                if (!result.ExecutedSuccesfully)
                {
                    return View(model)
                        .WithError("Ha ocurrido un problema al momento de registrar la información. Intentalo más tarde");
                }
            }
            var seoUrl = job.Title.SanitizeUrl().SeoUrl(job.Id);
            var url = Url.AbsoluteAction(seoUrl, "jobs");

            await _slackService.PostNewJob(job, url);

            return RedirectToAction(nameof(Detail), new
            {
                id = job.Title.SanitizeUrl().SeoUrl(job.Id),
                fromWizard = 1
            });
        }

        // GET: /jobs/4-jobtitle
        [AllowAnonymous]
        public IActionResult Detail(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return RedirectToAction(nameof(Index));

            var jobId = GetIdFromTitle(id);

            if (jobId == 0)
                return RedirectToAction(nameof(Index));

            var job = _jobService.GetById(jobId);

            if (job == null)
                return RedirectToAction("Index", "Home")
                    .WithError("La vacante solicitada no existe. Por favor escoge una vacante válida del listado.");

            var expectedUrl = job.Title.SanitizeUrl().SeoUrl(jobId);

            if (!expectedUrl.Equals(id, StringComparison.OrdinalIgnoreCase))
                return RedirectToActionPermanent(nameof(Detail), new { id = expectedUrl });


            ViewBag.RelatedJobs =
                       _jobService.GetCompanyRelatedJobs(jobId, job.Company.Name);
            var cookieLike = $"JobLike{jobId}";

            ViewBag.CanLike = !ControllerContext.HttpContext.CookieExists(cookieLike);

            var cookieView = $"JobView{job.Id}";


            if (IsJobOpportunityOwner(id))
            {
                return !job.Approved
                    ? View(nameof(Detail), job)
                        .WithInfo("Esta vacante no ha sido aprobada. Un miembro del equipo de moderadores la revisará pronto. Intentalo de nuevo más tarde.")
                    : job.IsHidden
                      ? View(nameof(Detail), job).WithInfo("Esta opportunidad de empleo está oculta, solo usted puede verla, para mostrarla en el listado, haga click en el boton \"Mostrar\" en el detalle de la oportunidad o en el su perfil de usuario")
                : View(nameof(Detail), job);
            }

            // If is not the owner, then it should be an applicant
            _jobService.UpdateViewCount(job.Id);
            ControllerContext.HttpContext.SetCookie(cookieView, job.Id.ToString());

            return !job.Approved || job.IsHidden
                       ? RedirectToAction(nameof(Index), "Home")
                       .WithInfo("Esta oportunidad no esta disponible o no existe. Intentalo más tarde o verifica el enlace.")
                           : View(nameof(Detail), job);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Edit(string jobtitle)
        {
            if(string.IsNullOrEmpty(jobtitle))
            {
                return RedirectToAction("Index", "Home").WithError("El id del trabajo a editar es invalido");
            }
            var job = GetJobOpportunityFromTitle(jobtitle);

            if(job == null)
                return RedirectToAction("Index", "Home").WithError("El trabajo que quizo editar no es valido");
            var currentUserId = User.GetUserIdFromClaims();
            if (!IsJobOpportunityOwner(jobtitle))
                return RedirectToAction(nameof(Detail), new { id = jobtitle });
            var viewModel = ViewModels.Jobs.Wizard.FromEntity(job);
            var categories = _categoryService.GetCategories().ToList();
            var jobtypes = _hireTypeService.GetHireTypes().ToList();
            viewModel.Categories = categories;
            viewModel.JobTypes = jobtypes;
            viewModel.MapsApiKey = _socialKeys.GoogleMapsApiKey;
            ViewBag.IsEdit = true;
            return View(nameof(Wizard), viewModel);
        }


        [HttpPost]
        public JsonResult ToggleHide(string title)
        {
            var job = GetJobOpportunityFromTitle(title);
            if (IsJobOpportunityOwner(title))
            {
                job = _jobService.ToggleHideState(job);
            }

            return Json(new { isHidden = job.IsHidden });

        }

        [HttpPost]
        public IActionResult Delete(int? id)
        {
            if (id == null)
                return RedirectToAction("Index", "Home").WithError("La posición solicitada no existe.");
            var job = _jobService.GetById(id.Value);
            if(job == null)
                return RedirectToAction("Index", "Home").WithError("La posición solicitada no existe.");
            var result = _jobService.Delete(id.Value);
            if(result.ExecutedSuccesfully)
            {
                // TODO Success message here but after getting the alerts working
                return RedirectToAction("Index", "Home");
            }

            // Show the real message internally so We can track it and address it
            Console.WriteLine(result.Message);
            var seoid = job.Title.SanitizeUrl().SeoUrl(id.Value);
            return RedirectToAction("Detail", new { id = seoid }).WithError("Hubo un problema al momento de eliminar la posición.");
        }


        private static int GetIdFromTitle(string title)
        {
            int id;
            var url = title.Split('-');

            if (string.IsNullOrEmpty(title) || url.Length == 0 || !int.TryParse(url[0], out id))
                return 0;

            return id;
        }

        private Job GetJobOpportunityFromTitle(string title)
        {
            var jobId = GetIdFromTitle(title);
            return _jobService.GetById(jobId);
        }

        private bool IsJobOpportunityOwner(string title)
        {
            try
            {
                var job = GetJobOpportunityFromTitle(title);
                return (job.UserId == _applicationUser.RawId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPTION on {ex.Source} :: {ex.Message}");
                // Maybe there is no user logged in
                return false;
            }
        }


        /// <summary>
        /// Transform JobOpportunityPagingParameter into JobOpportunitySearchViewModel with Locations
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private JobSearchViewModel GetSearchViewModel(JobPagingParameter model)
        {
            if (string.IsNullOrWhiteSpace(model.SelectedLocationName))
            {
                model.SelectedLocationLatitude = default(double);
                model.SelectedLocationLongitude = default(double);
                model.SelectedLocationPlaceId = string.Empty;
            }

            var viewModel = new JobSearchViewModel
            {
                SelectedLocationPlaceId = model.SelectedLocationPlaceId,
                SelectedLocationName = model.SelectedLocationName,
                SelectedLocationLongitude = model.SelectedLocationLongitude,
                SelectedLocationLatitude = model.SelectedLocationLatitude,
                CategoryId = model.CategoryId,
                Keyword = model.Keyword,
                IsRemote = model.IsRemote,
                CategoriesCount = _jobService.GetJobCountByCategory()
            };

            return viewModel;
        }
    }
}
