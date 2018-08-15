using Data.Repositories;
using Domain.Framework;
using AppService.Framework.Extensions;
using System.Linq;
using AppService.Services.Social;
using System;
using System.Threading.Tasks;
using AppService.Framework;
using Domain.Framework.Dto;
using Domain;
using System.Collections.Generic;

namespace AppService.Services
{
    public class SecurityService : ISecurityService
    {
        readonly IUserService _userService;
        readonly IFacebookService _facebookService;
        readonly ILinkedinService _linkedInService;
        readonly IGoogleService _googleService;
        readonly ILoginService _loginService;

        public SecurityService(IUserService userService, IFacebookService facebookService, ILoginService loginService, ILinkedinService linkedInService, IGoogleService googleService)
        {
            _userService = userService;
            _facebookService = facebookService;
            _loginService = loginService;
            _linkedInService = linkedInService;
            _googleService = googleService;
        }

        public LoginResult Login(string username, string password)
        {
            var loginResult = new LoginResult();
           /* 
            var user = _userService.Get(x => x.Email == username && x.Password == (password + x.Salt).GetHashSha256())
                                      .FirstOrDefault();
            if (user != null)
                return loginResult;
*/
            return loginResult;
        }

        private UserLimited SocialSignup(string provider, string key, string email)
        {
            var result = new UserLimited();
            var socialLogin = _loginService.Get(provider, key);

            if(socialLogin == null)//If this login doesn't exist create the new user
            {
                var newUser = new User
                {
                    Email = email
                };
                _userService.Create(newUser);
                var newLogin = new Login
                {
                    LoginProvider = provider,
                    ProviderKey = key,
                    UserId = newUser.Id
                };
                _loginService.Create(newLogin);
            }
            else //else get it from the database
            {
                var user = _userService.GetById(socialLogin.UserId);
                result.Id = user.Id;
                result.Email = user.Email;
            }
            return result;
        }
        public async Task<LoginResult> FacebookLogin(string code, string redirectUrl)
        {
            var loginResult = new LoginResult();
            try
            {
                var codeValidationResult = await _facebookService.ValidateCode(code, redirectUrl);
                if (codeValidationResult.ExecutedSuccesfully)
                {
                    var userInfoResult = await _facebookService.GetUserInformation(codeValidationResult.Data.access_token);
                    if (userInfoResult.ExecutedSuccesfully)
                    {
                        var socialInfo = userInfoResult.Data;
                        loginResult.ExecutedSuccesfully = true;
                        loginResult.User = SocialSignup("Facebook", socialInfo.id, socialInfo.email);
                    }
                    else
                        loginResult.AddErrorMessage(userInfoResult.Message);
                }
                else
                    loginResult.AddErrorMessage(codeValidationResult.Message);
            }
            catch(Exception ex)
            {
                loginResult.Exception = ex;
                loginResult.AddErrorMessage(ex.Message);
            }
            return loginResult;
        }
        public async Task<LoginResult> LinkedInLogin(string code, string redirectUrl)
        {
            var loginResult = new LoginResult();
            try
            {
                var codeValidationResult = await _linkedInService.RequestTokenAsync(code, redirectUrl);
                if (codeValidationResult.ExecutedSuccesfully)
                {
                    var userInfoResult = await _linkedInService.GetUserInformation(codeValidationResult.Data.access_token);
                    if (userInfoResult.ExecutedSuccesfully)
                    {
                        var socialInfo = userInfoResult.Data;
                        loginResult.ExecutedSuccesfully = true;
                        loginResult.User = SocialSignup("LinkedIn", socialInfo.id, socialInfo.email);
                    }
                    else
                        loginResult.AddErrorMessage(userInfoResult.Message);
                }
                else
                    loginResult.AddErrorMessage(codeValidationResult.Message);
            }
            catch (Exception ex)
            {
                loginResult.Exception = ex;
                loginResult.AddErrorMessage(ex.Message);
            }
            return loginResult;
        }

        public async Task<LoginResult> GoogleLogin(string code, string redirectUrl)
        {
            var loginResult = new LoginResult();
            try
            {
                var codeValidationResult = await _googleService.RequestTokenAsync(code, redirectUrl);
                if (codeValidationResult.ExecutedSuccesfully)
                {
                    var userInfoResult = await _googleService.GetUserInformation(codeValidationResult.Data.access_token);
                    if (userInfoResult.ExecutedSuccesfully)
                    {
                        var socialInfo = userInfoResult.Data;
                        loginResult.ExecutedSuccesfully = true;
                        loginResult.User = SocialSignup("Google", socialInfo.id, socialInfo.email);
                    }
                    else
                        loginResult.AddErrorMessage(userInfoResult.Message);
                }
                else
                    loginResult.AddErrorMessage(codeValidationResult.Message);
            }
            catch (Exception ex)
            {
                loginResult.Exception = ex;
                loginResult.AddErrorMessage(ex.Message);
            }
            return loginResult;
        }
    }

    public interface ISecurityService
    {
        LoginResult Login(string username, string password);
        Task<LoginResult> FacebookLogin(string code, string redirectUrl);
        Task<LoginResult> LinkedInLogin(string code, string redirectUrl);
        Task<LoginResult> GoogleLogin(string code, string redirectUrl);
    }
}
