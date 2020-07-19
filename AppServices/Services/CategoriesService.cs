using System;
using AppServices.Framework;
using AppServices.Data.Repositories;
using Domain.Entities;

namespace AppServices.Services
{
    public class CategoriesService : BaseService<Category, ICategoriesRepository>, ICategoriesService
    {
        public CategoriesService(ICategoriesRepository mainRepository) : base(mainRepository)
        {
        }

        protected override TaskResult<Category> ValidateOnCreate(Category entity)
        {
            return new TaskResult<Category>();
        }

        protected override TaskResult<Category> ValidateOnDelete(Category entity)
        {
            return new TaskResult<Category>();
        }

        protected override TaskResult<Category> ValidateOnUpdate(Category entity)
        {
            return new TaskResult<Category>();
        }
    }

    public interface ICategoriesService : IBaseService<Category, ICategoriesRepository>
    {

    }
}
