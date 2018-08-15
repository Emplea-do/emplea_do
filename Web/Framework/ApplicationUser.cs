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
        public string Id { get { return _user.FindFirst("Id").Value; } }
        public int RawId { get { return Convert.ToInt32(_user.FindFirst("Id").Value); } }
        public string Email { get { return _user.FindFirst("Email").Value; } }

    }
}
