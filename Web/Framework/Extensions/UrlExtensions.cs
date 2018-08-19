using System;
using System.Net;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Web.Framework.Extensions
{
    public static class UrlExtensions
    {        
        /// <summary>
        /// Sanitiza un URL
        /// </summary>
        /// <remarks>http://stackoverflow.com/questions/6716832/sanitizing-string-to-url-safe-format</remarks>
        public static string SanitizeUrl(this string strToSanitize)
        {
            return strToSanitize == null
                ? null
                : Regex.Replace(strToSanitize, @"[^A-Za-z0-9_~]+", "-");
        }

        public static string SeoUrl(int id, string title)
        {
            return string.IsNullOrEmpty(title) ? id.ToString() : $"{id}-{SanitizeUrl(title)}";
        }

        public static bool IsValidImageUrl(string imageUrl)
        {
            var regex = new Regex("^(http|https)://(.+).(png|jpg)$");
            return !string.IsNullOrWhiteSpace(imageUrl) && regex.IsMatch(imageUrl);
        }

        public static bool IsImageAvailable(string imageUrl)
        {
            if (!IsValidImageUrl(imageUrl)) return false;
            var request = WebRequest.Create(imageUrl);
            try
            {
                var response = (HttpWebResponse)request.GetResponse();
                var statusCode = response.StatusCode;
                response.Close();
                return statusCode == HttpStatusCode.OK;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// Generates a fully qualified URL to an action method by using
        /// the specified action name, controller name and route values.
        /// </summary>
        /// <param name="url">The URL helper.</param>
        /// <param name="actionName">The name of the action method.</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <param name="routeValues">The route values.</param>
        /// <returns>The absolute URL.</returns>
        public static string AbsoluteAction(this IUrlHelper url, string actionName, string controllerName, object routeValues = null)
        {
            string scheme = url.ActionContext.HttpContext.Request.Scheme;

            return url.Action(actionName, controllerName, routeValues, scheme);
        }

        public static string AbsoluteUrl(this UrlHelper url, string actionName, string controllerName,
            object routeValues = null)
        {
            var scheme = url.ActionContext.HttpContext.Request.Scheme;
            return url.Action(actionName, controllerName, routeValues, scheme);
        }
    }
}
