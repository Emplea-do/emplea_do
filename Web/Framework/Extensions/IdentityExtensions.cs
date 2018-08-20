using System;
using System.Security.Claims;
namespace Web.Framework.Extensions
{
    public static class IdentityExtensions
    {
        public static int GetUserIdFromClaims(this ClaimsPrincipal user)
        {
            if (!user.Identity.IsAuthenticated)
                return 0;

            ClaimsPrincipal currentUser = user;
            return Convert.ToInt32(currentUser.FindFirst("Id")?.Value);
        }
    }
}
