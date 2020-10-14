using System;
using System.Globalization;
namespace Web.Framework.Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Returns the datetime in local format.
        /// </summary>
        /// <param name="dateTime">Date time</param>
        /// <returns></returns>
        public static string ToDominicanFormat(this DateTime dateTime)
        {
            return dateTime.ToString("dd MMMM \\de yyyy", new CultureInfo("es-DO"));
        }
    }
}
