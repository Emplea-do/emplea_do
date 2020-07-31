using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;

namespace Web.Framework.Extensions
{
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// Obtiene la versión del assembly actual como un string
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static HtmlString AssemblyVersion(this IHtmlHelper helper)
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            return new HtmlString(version);
        }

        /// <summary>
        /// Agrega la clase active al elemento dependiendo de la ruta en la que este
        /// </summary>
        /// <param name="html"></param>
        /// <param name="controllers"></param>
        /// <param name="actions"></param>
        /// <param name="cssClass"></param>
        /// <returns> string cssClass</returns>
        public static string IsSelected(this IHtmlHelper html, string controllers = "", string actions = "", string cssClass = "active")
        {
            ViewContext viewContext = html.ViewContext;

            RouteValueDictionary routeValues = viewContext.RouteData.Values;
            string currentAction = routeValues["action"].ToString();
            string currentController = routeValues["controller"].ToString();

            if (string.IsNullOrEmpty(actions))
            {
                actions = currentAction;
            }

            if (string.IsNullOrEmpty(controllers))
            {
                controllers = currentController;
            }

            string[] acceptedActions = actions.Trim().Split(',').Distinct().ToArray();
            string[] acceptedControllers = controllers.Trim().Split(',').Distinct().ToArray();

            if (acceptedActions.Contains(currentAction) && acceptedControllers.Contains(currentController))
            {
                return cssClass;
            }

            return string.Empty;
        }

        /// <summary>
        /// Obtener de las dos primemras palabras su primera letra. Sí el texto solo posee una palabra solo se retorna la primera letra de la misma
        /// </summary>
        /// <param name="helper">Variable de extensión</param>
        /// <param name="value">Valor a procesar</param>
        /// <returns>HtmlString</returns>
        public static HtmlString FirstTwoLetters(this IHtmlHelper helper, string value)
        {
            var result = string.Empty;

            if (!string.IsNullOrEmpty(value))
            {
                var splited = Regex.Split(value, @"[_+-.,!@#$%^&*();\/|<> ]|[0-9]");

                foreach (var currentValue in splited)
                {
                    if (string.IsNullOrWhiteSpace(currentValue)) continue;

                    result += currentValue.Substring(0, 1);

                    if (result.Length.Equals(2))
                        break;
                }
            }

            return new HtmlString(result);
        }
    }
}