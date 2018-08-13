using Data.Repositories;
using Domain.Framework;
using AppService.Framework.Extensions;
using System.Linq;

namespace AppService.Services
{
    public class SecurityService : ISecurityService
    {
        readonly IUserRepository _userRepository;

        public SecurityService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public LoginResult Login(string username, string password)
        {
            var loginResult = new LoginResult();
            var user = _userRepository.Get(x => x.Email == username && x.Password == (password + x.Salt).GetHashSha256())
                                      .FirstOrDefault();
            if (user != null)
                return loginResult;

            loginResult.User = user;
            loginResult.ExecutedSuccessfully = false;
            return loginResult;
        }

        public LoginResult SocialLogin(string socialId, string socialNetwork)
        {
            throw new System.NotImplementedException();
        }
    }

    public interface ISecurityService
    {
        LoginResult Login(string username, string password);
        LoginResult SocialLogin(string socialId, string socialNetwork);
    }
}
