using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Web.Framework.Extensions
{
    public static class HttpContextExtensions
    {
        public static async Task<AuthenticationScheme[]> GetExternalProvidersAsync(this HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var schemes = context.RequestServices.GetRequiredService<IAuthenticationSchemeProvider>();

            return (from scheme in await schemes.GetAllSchemesAsync()
                    where !string.IsNullOrEmpty(scheme.DisplayName)
                    select scheme).ToArray();
        }

        public static async Task<bool> IsProviderSupportedAsync(this HttpContext context, string provider)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return (from scheme in await context.GetExternalProvidersAsync()
                    where string.Equals(scheme.Name, provider, StringComparison.OrdinalIgnoreCase)
                    select scheme).Any();
        }
        /*
        public static string GetRawBodyString(this HttpContext httpContext, Encoding encoding)
        {
            var body = "";
            if (httpContext.Request.ContentLength == null || !(httpContext.Request.ContentLength > 0) ||
                !httpContext.Request.Body.CanSeek) return body;
            httpContext.Request.EnableRewind();
            httpContext.Request.Body.Seek(0, SeekOrigin.Begin);
            using (var reader = new StreamReader(httpContext.Request.Body, encoding))
            {
                body = reader.ReadToEnd();
            }
            httpContext.Request.Body.Position = 0;
            return body;
        }*/
    }
}
