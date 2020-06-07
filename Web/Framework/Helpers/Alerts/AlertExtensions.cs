using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;

namespace Web.Framework.Helpers.Alerts
{
    public static class AlertExtensions
    {
        public const string Alerts = "_Alerts";

        public static List<Alert> GetAlerts(this ITempDataDictionary tempData)
        {
            if (!tempData.ContainsKey(Alerts))
            {
                return new List<Alert>();
            }

            return JsonConvert.DeserializeObject<List<Alert>>(tempData[Alerts].ToString());
        }

        public static IActionResult WithSuccess(this IActionResult result, string message)
        {
            return Alert(result, "alert-success", message);
        }

        public static IActionResult WithInfo(this IActionResult result, string message)
        {
            return Alert(result, "alert-info", message);
        }

        public static IActionResult WithWarning(this IActionResult result, string message)
        {
            return Alert(result, "alert-warning", message);
        }

        public static IActionResult WithError(this IActionResult result, string message)
        {
            return Alert(result, "alert-danger", message);
        }

        private static IActionResult Alert(IActionResult result, string type, string body)
        {
            return new AlertDecoratorResult(result, type, body);
        }
    }
}