using AppServices.Data.Repositories;
using AppServices.Framework;
using Domain.Entities;

namespace AppServices.Services
{
    public class BannersService : BaseService<Banner, IBannersRepository>, IBannersService
    {
        public BannersService(IBannersRepository mainRepository) : base(mainRepository)
        { }

        protected override TaskResult<Banner> ValidateOnCreate(Banner entity)
        {
            return new TaskResult<Banner>();
        }

        protected override TaskResult<Banner> ValidateOnDelete(Banner entity)
        {
            return new TaskResult<Banner>();
        }

        protected override TaskResult<Banner> ValidateOnUpdate(Banner entity)
        {
            return new TaskResult<Banner>();
        }
    }

    public interface IBannersService : IBaseService<Banner, IBannersRepository> {}
}