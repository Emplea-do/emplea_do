using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Api.Models;
using AppService.Services;
using Domain.Framework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Api.Controllers
{
    public class AuthController : BaseController
    {
        ISecurityService _securityService;

        public AuthController(ISecurityService securityService, ILogger<AuthController> logger) : base(logger)
        {
            _securityService = securityService;
        }

        [HttpPost("v1.0/[controller]/token")]
        public IActionResult CreateToken([FromBody] CredentialModel model)
        {
            try
            {
                var result = _securityService.Login(model.Username, model.Password);

                if (result.ExecutedSuccesfully)
                {
                    var claims = new[]{
                        new Claim(JwtRegisteredClaimNames.Sub, result.User.Email),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    };

                    //Move this to configurations
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("THISISALONGTEST!!!"));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    return new JsonResult(new JwtSecurityToken(
                        issuer: "https://emplea.do",
                        audience: "https://emplea.do",
                        claims: claims,
                        expires: DateTime.UtcNow.AddHours(24),
                        signingCredentials: creds
                    ));
                }
            }
            catch(Exception ex)
            {
                _logger.LogError($"Exception thrown while creating: {ex}");
            }

            return new BadRequestResult();
        }
    }
}
