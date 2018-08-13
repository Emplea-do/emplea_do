using System;
using Domain;

namespace AppService.Services
{
    public class UserService : BaseService<User>, IUserService
    {
    }

    public interface IUserService
    {
    }
}
