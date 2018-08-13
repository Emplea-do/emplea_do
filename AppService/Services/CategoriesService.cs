using System;
using System.Linq;
using AppService.Framework;
using Data.Repositories;
using Domain;
using Domain.Framework;

namespace AppService.Services
{
    public class CategoriesService : ICategoriesService
    {
        private readonly ICategoryRepository _categoriesRepository;

        public CategoriesService(ICategoryRepository categoriesRepository)
        {
            _categoriesRepository = categoriesRepository;
        }

        public PagingResult<Category> GetByPagination(PaginationFilter paginationFilter)
        {
            return new PagingResult<Category>
            {
                Data = _categoriesRepository.GetAll(),
                ItemsPerPage = paginationFilter.ItemsPerPage,
                Page = paginationFilter.Page,
                TotalItems = _categoriesRepository.Count()
            };
        }

        public Category GetById(int id)
        {
            return _categoriesRepository.GetById(id);
        }
    }

    public interface ICategoriesService
    {
        PagingResult<Category> GetByPagination(PaginationFilter paginationFilter);

        Category GetById(int id);
    }
}
