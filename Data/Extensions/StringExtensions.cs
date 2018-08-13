using System;
using System.Text.RegularExpressions;

namespace Data.Extensions
{
    public static class StringExtensions
    {
        public static string RemoveHtml(this string str)
        {
            var content = str.Replace("<p>", String.Empty).Replace("</p>", "\n")
                      .Replace("<br>", "\n").Replace("<br/>", "\n")
                      .Replace("<ul>", String.Empty).Replace("</ul>", "\n")
                      .Replace("<li>", String.Empty).Replace("</li>", "\n")
                      .Replace("<b>", String.Empty).Replace("</b>", String.Empty)
                      .Replace("&nbsp;", " ");

            content = Regex.Replace(content, @"<[^>]*>", String.Empty);
            return content;
        }
    }
}
