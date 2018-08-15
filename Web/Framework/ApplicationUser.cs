using System;
using System.Collections.Generic;
using System.Security.Claims;
using Domain;
using Domain.Framework.Dto;

namespace Web.Framework
{
    public class ApplicationUser : ClaimsPrincipal
    {
        public string Id { get { return FindFirst("Id").Value; } }
        public int RawId { get { return Convert.ToInt32(FindFirst("Id").Value); } }
        public string Email { get { return FindFirst("Email").Value; } }

        public void Init(UserLimited userLimited)
        {
            var claims = new List<Claim> {
                new Claim("Email", userLimited.Email, ClaimValueTypes.String),
                new Claim("Id", userLimited.Id.ToString(), ClaimValueTypes.String),
            };

            AddIdentity(new ClaimsIdentity(claims));
        }
    }
}
