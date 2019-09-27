using System;
using System.Collections.Generic;
using System.Security.Claims;
using Domain;
using Domain.Framework.Dto;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Web.Framework
{
    public class ApplicationUser
    {
        private ClaimsPrincipal _user;
        public ApplicationUser(ClaimsPrincipal user)
        {
            _user = user;
        }

        public int UserId { get { return Int32.Parse(_user.FindFirst("UserId")?.Value??"0");  } }
        public string SocialId { get { return _user.FindFirst(ClaimTypes.NameIdentifier).Value; } }
        public string Email { get { return _user.FindFirst(ClaimTypes.Email).Value; } }
        public string Name { get { return _user.FindFirst(ClaimTypes.Name).Value; } }

    }
}
