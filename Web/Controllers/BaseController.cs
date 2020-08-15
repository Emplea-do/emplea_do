using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Web.Framework;
using Web.Framework.Extensions;

namespace Web.Controllers
{
    public class BaseController : Controller
    {
        protected ApplicationUser _currentUser => new ApplicationUser(HttpContext.User);


        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // TODO Review this
            if (TempData.ContainsKey("ModelErrors"))
            {
                var messages = TempData.Get<string>("ModelErrors");
                foreach (var error in messages.Split(','))
                {
                    ModelState.AddModelError("", error);
                }
            }
            base.OnActionExecuting(context);
        }
        protected IActionResult RedirectToLocal(string action, object model, object routeValues = null)
        {
            TempData.Put("Model", model);
            if (ModelState.ErrorCount > 0)
            {
                var modelErrors = string.Join(",", ModelState[""].Errors.Select(x => x.ErrorMessage).ToArray());
                TempData.Put("ModelErrors", modelErrors);
            }
            return RedirectToAction(action, routeValues);
        }
    }
}
