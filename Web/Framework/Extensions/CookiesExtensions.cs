using System;
using System.Net;
using Microsoft.AspNetCore.Http;

namespace Web.Framework.Helpers
{
    public static class CookiesExtensions
    {   
        public static void SetCookie(this HttpContext context, string key, string value)
        {
            context.Response.Cookies.Append(key, value);
        }

        public static string GetCookie(this HttpContext context, string key)
        {
            var cookie = context.Request.Cookies[key];
            return !string.IsNullOrWhiteSpace(cookie) ? WebUtility.HtmlEncode(cookie).Trim() : String.Empty;
        }

        public static bool CookieExists(this HttpContext context, string key)
        {
            return context.Request.Cookies[key] != null;
        }

        public static void DeleteCookie(this HttpContext context, string key)
        {
            if (context.CookieExists(key))
                context.Response.Cookies.Append(key, "", new CookieOptions { Expires = DateTime.Now.AddDays(-1) });
        }
    }
}
