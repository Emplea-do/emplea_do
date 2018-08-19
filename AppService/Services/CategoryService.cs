using System;
using System.Collections.Generic;
using System.Linq;
using AppService.Framework;
using Data.Repositories;
using Domain;
using Domain.Framework;

namespace AppService.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoriesRepository;

        public CategoryService(ICategoryRepository categoriesRepository)
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

        public IEnumerable<Category> GetCategories()
        {
            return _categoriesRepository.GetAll();
        }
    }

    public interface ICategoryService
    {
        PagingResult<Category> GetByPagination(PaginationFilter paginationFilter);

        Category GetById(int id);

        IEnumerable<Category> GetCategories();
    }
}
