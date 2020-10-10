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

namespace Web.Controllers
{
    public class BannersController : BaseController
    {
        private readonly IBannersService _bannersService;
        private readonly IWebHostEnvironment _webHostEnvironment;  

        public  BannersController(IBannersService bannersService, IWebHostEnvironment webHostEnvironment)
        {
            _bannersService = bannersService;
            _webHostEnvironment = webHostEnvironment;  
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
        public IActionResult Wizard(BannerViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var fileName = UploadedFile(model.ImageUrl);

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
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "img\banners");  
                
                uniqueFileName = $"{Guid.NewGuid().ToString()}_{file.FileName}";  
                
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);  
                
                using (var fileStream = new FileStream(filePath, FileMode.Create))  
                {  
                    file.CopyTo(fileStream);  
                }  
            }

            return uniqueFileName;  
        }  
    }
}