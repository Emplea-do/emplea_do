using System;
using AppService.Framework.Social;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Web.Framework.Filters
{
    public class UnderMaintenanceFilterAttribute : ActionFilterAttribute
    {
        IConfiguration _configuration;

        public UnderMaintenanceFilterAttribute(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var controllerName = (context.Controller as Controller).ControllerContext.ActionDescriptor.ControllerName;
            if (controllerName.Contains("Error") ||
                controllerName.Contains("Credits") ||
                controllerName.Contains("UnderMaintenance"))
            {
                base.OnActionExecuting(context);
                return;
            }

            bool underMaintenance;
            bool.TryParse(_configuration.GetSection("UnderMaintenance").Value, out underMaintenance);

            if (underMaintenance)
            {
                context.Result = new RedirectToRouteResult(
                    new RouteValueDictionary
                    {
                        { "controller", "UnderMaintenance"},
                        { "action", "Index"}
                    });
            }

            base.OnActionExecuting(context);
        }
    }
}
