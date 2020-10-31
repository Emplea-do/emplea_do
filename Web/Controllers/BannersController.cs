using AppServices.Services;
using AppServices.Framework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Web.Framework.Helpers.Alerts;
using Web.ViewModels;
using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using Domain.Entities;
using ElmahCore;
using Web.Services.Slack;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using Web.Models.Slack;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace Web.Controllers
{
    public class BannersController : BaseController
    {
        private readonly IBannersService _bannersService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ISlackService _slackService;
         private readonly IConfiguration _configuration;

        public  BannersController(IBannersService bannersService, IWebHostEnvironment webHostEnvironment, ISlackService slackService, IConfiguration configuration)
        {
            _bannersService = bannersService;
            _webHostEnvironment = webHostEnvironment;
            _slackService = slackService;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            var companies = _bannersService.GetAll();
            return View(companies);
        }
        
        [Authorize]
        public IActionResult Wizard(int? id)
        {
            var model = new BannerViewModel();

            if (id.HasValue && id > 0)
            {
                var banner = _bannersService.GetById(id.Value);

                if(banner.UserId == _currentUser.UserId)
                {
                    model.Id = banner.Id;
                    model.DestinationUrl = banner.DestinationUrl;
                    model.UserId = _currentUser.UserId;
                    model.DisplayImageUrl = banner.ImageUrl;
                }
                else
                { 
                    return RedirectToAction("Index", "UserProfile").WithError("No tienes permiso para editar este Banner");
                }
            }

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Wizard(BannerViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            
            try
            {
                var fileName = "";

                if (HttpContext.Request.Form.Files != null && HttpContext.Request.Form.Files[0] != null)
                {
                    fileName = UploadedFile(HttpContext.Request.Form.Files[0]);
                }

                if(model.Id > 0) 
                {
                    var bannerToUpdate = _bannersService.GetById(model.Id);

                    if(bannerToUpdate.UserId == _currentUser.UserId)
                    {
                        bannerToUpdate.DestinationUrl = model.DestinationUrl;
                        bannerToUpdate.ImageUrl = fileName;

                        var result = _bannersService.Update(bannerToUpdate);

                        if(result.Success)
                        {
                            return RedirectToAction("Index", "UserProfile").WithSuccess("Banner editado exitosamente");
                        }

                        return View(model).WithError(result.Messages);
                    }
                    else
                    { 
                        return RedirectToAction("Index", "UserProfile").WithError("No tienes permiso para editar este Banner");
                    }
                } else 
                {
                    var banner = new Banner
                    {
                        ImageUrl = fileName,
                        DestinationUrl = model.DestinationUrl,
                        UserId = _currentUser.UserId,
                    };

                    var result = _bannersService.Create(banner);

                    if(result.Success)
                    {
                        try
                        {
                            var newBanner = _bannersService.GetById(banner.Id);

                            await _slackService.PostBanner(newBanner, Url).ConfigureAwait(false);
                        }
                        catch (Exception ex)
                        {
                            HttpContext.RiseError(ex);
                        }

                        return RedirectToAction("Index", "UserProfile").WithSuccess("Banner creada exitosamente");
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
                var banner = _bannersService.GetById(id);

                if(banner == null)
                {
                    result.AddErrorMessage("No puedes eliminar una Banner que no existe.");
                }
                else if(banner.UserId != _currentUser.UserId)
                {
                    result.AddErrorMessage("No puedes eliminar un Banner que no creaste.");
                }
                
                result = _bannersService.Delete(banner);
            }
            catch(Exception ex)
            {
                HttpContext.RiseError(ex);
                result.AddErrorMessage(ex.Message);
            }
            
            return Json(result);
        }

        private string UploadedFile(IFormFile file)  
        {  
            string uniqueFileName = null;  
  
            if (file != null)  
            {  
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "img/banners");  
                
                uniqueFileName = $"{Guid.NewGuid().ToString()}_{file.FileName}";  
                
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);  
                
                using (var fileStream = new FileStream(filePath, FileMode.Create))  
                {  
                    file.CopyTo(fileStream);  
                }  
            }

            return "https://emplea.do//img/banners/" + uniqueFileName;  
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
                    throw new Exception($"Payload is null, Body: {payload}");

                int bannerId = Convert.ToInt32(data.callback_id);
                var banner = _bannersService.GetById(bannerId);
                var isJobApproved = data.actions.FirstOrDefault()?.value == "approve";
                var isJobRejected = data.actions.FirstOrDefault()?.value == "reject";
                var isTokenValid = data.token == _configuration["Slack:BannersVerificationToken"];

                if (isTokenValid && isJobApproved)
                {
                    banner.IsApproved = true;

                    _bannersService.Update(banner);

                    await _slackService.PostBannerResponse(banner, Url, data.response_url, data?.user?.id, true);
                }
                else if (isTokenValid && isJobRejected)
                {
                    if (banner == null)
                    {
                        await _slackService.PostBannerErrorResponse(banner, Url, data.response_url);
                    }
                    else
                    {
                        await _slackService.PostBannerResponse(banner, Url, data.response_url, data?.user?.id, false);
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