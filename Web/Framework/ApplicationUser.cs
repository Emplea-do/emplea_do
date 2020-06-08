using System;
using System.Collections.Generic;
using System.Security.Claims;
using Domain;
using Domain.Framework.Dto;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace Web.Framework
{
    public class ApplicationUser
    {
        ISession _session;
        private ClaimsPrincipal _user;
        public ApplicationUser(ClaimsPrincipal user, ISession session)
        {
            _user = user;
            _session= session;
        }

        public void SetUserId(int value)
        {
            var userIdClaim = _user.FindFirst("UserId");
            var claimIdentity = ((ClaimsIdentity)_user.Identity);
            if (userIdClaim != null)
                claimIdentity.RemoveClaim(userIdClaim);
            claimIdentity.AddClaim(new Claim("UserId", value.ToString()));
        }
        public int UserId {
            get { return _session.GetInt32("UserId")??0;  }
        }
        public string SocialId { get { return _user.FindFirst(ClaimTypes.NameIdentifier).Value; } }
        public string Email { get { return _user.FindFirst(ClaimTypes.Email).Value; } }
        public string Name { get { return _user.FindFirst(ClaimTypes.Name).Value; } }

    }
}
