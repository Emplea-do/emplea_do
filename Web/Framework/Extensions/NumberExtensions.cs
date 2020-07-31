using System;
namespace Web.Framework.Extensions
{
    public static class NumberExtensions
    {
        public static string FormatThousand(this int number)
        {
            return string.Format(number.ToString(), "{1:#,0}");
        }
    }
}
