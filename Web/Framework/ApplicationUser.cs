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
        private ClaimsPrincipal _user;
        public ApplicationUser(ClaimsPrincipal user)
        {
            _user = user;
        }

        public bool IsAuthenticated { get {  return _user.Identity.IsAuthenticated; } }
        public int UserId { get { return Convert.ToInt32(_user.FindFirst("UserId").Value); } }
        public string SocialId { get { return _user.FindFirst(ClaimTypes.NameIdentifier).Value; } }
        public string Email { get { return _user.FindFirst(ClaimTypes.Email).Value; } }
        public string Name { get { return _user.FindFirst(ClaimTypes.Name).Value; } }
    }
}
