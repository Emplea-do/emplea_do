using System.Collections.Generic;
using System.Linq;
using AppServices.Data.Repositories;
using AppServices.Framework;
using Domain.Entities;

namespace AppServices.Services
{
    public class BannersService : BaseService<Banner, IBannersRepository>, IBannersService
    {
        public BannersService(IBannersRepository mainRepository) : base(mainRepository)
        { }

        public List<Banner> GetByUserId(int userId)
        {
            return _mainRepository.Get(x=>x.IsActive && x.UserId == userId).ToList();
        }

        public Banner GetById(int id)
        {
            return _mainRepository.Get(x=>x.IsActive && x.Id == id).FirstOrDefault();
        }

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

    public interface IBannersService : IBaseService<Banner, IBannersRepository> 
    {
        List<Banner> GetByUserId(int userId);
         Banner GetById(int id);
    }
}