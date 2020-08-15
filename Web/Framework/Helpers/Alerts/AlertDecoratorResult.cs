using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Web.Framework.Helpers.Alerts
{
    public class AlertDecoratorResult : IActionResult
    {
        public IActionResult InnerResult { get; set; }
        public string AlertClass { get; set; }
        public string Message { get; set; }

        public AlertDecoratorResult(IActionResult innerResult, string alertClass, string message)
        {
            InnerResult = innerResult;
            AlertClass = alertClass;
            Message = message;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var factory = context.HttpContext.RequestServices.GetService<ITempDataDictionaryFactory>();
            var tempData = factory.GetTempData(context.HttpContext);
            var alerts = tempData.GetAlerts();
            alerts.Add(new Alert(AlertClass, Message));
            tempData[AlertExtensions.Alerts] = JsonConvert.SerializeObject(alerts);
            await InnerResult.ExecuteResultAsync(context);
        }
    }
}