using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace AppService.Framework.Extensions
{

    public static class StringExtensions
    {
        public static string ToCamelCase(this string str)
        {
            var str2 = String.Empty;
            for (int i = 1; i < str.Length; ++i)
            {
                if (str[i] == ' ') continue;
                if (i == 1) { str2 += str[0]; }
                if (str[i - 1] == ' ')
                {
                    str2 += char.ToUpper(str[i]);
                }
                else
                {
                    str2 += str[i];
                }
            }
            return str2;
        }

        public static string FromCamelCaseToNormalForm(this string str)
        {
            var result = String.Empty;

            for (int i = 1; i < str.Length; ++i)
            {
                if (i > 0 && Char.IsUpper(str[i]))
                {
                    result += ' ';
                }
                if (i == 1) { result += str[0]; }
                result += str[i];
            }
            return result;
        }

        public static string SplitCamelCase(this string str)
        {

            return Regex.Replace(str,
              String.Format("{0}|{1}|{2}",
                 "(?<=[A-Z])(?=[A-Z][a-z])",
                 "(?<=[^A-Z])(?=[A-Z])",
                 "(?<=[A-Za-z])(?=[^A-Za-z])"
              ),
              " "
           );
        }
        public static string GetHashSha256(this string str)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(str);
            using (SHA256Managed hashstring = new SHA256Managed())
            {
                byte[] hash = hashstring.ComputeHash(bytes);
                string hashString = string.Empty;
                foreach (byte x in hash)
                {
                    hashString += String.Format("{0:x2}", x);
                }
                return hashString;
            }
        }
    }
}
