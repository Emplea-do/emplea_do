using System;
using System.Text.RegularExpressions;

namespace Web.Framework.Extensions
{
    public static class StringExtensions
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

        public static string SeoUrl(this string urlString, int id)
        {
            return string.IsNullOrEmpty(urlString) ? id.ToString() : $"{id}-{SanitizeUrl(urlString)}";
        }
    }
}
