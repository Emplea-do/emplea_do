using System;
using Api.Framework;
using AppService.Services;
using Domain;
using Domain.Framework;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("v1.0/[controller]")]
    public class CategoriesController
    {
        ICategoryService _categoriesService;

        public CategoriesController(ICategoryService categoriesService)
        {
            _categoriesService = categoriesService;
        }


        [HttpGet]
        public PagingResult<Category> Get(PaginationParameters parameters)
        {
            return _categoriesService.GetByPagination(new PaginationFilter{
                ItemsPerPage = 10,
                Page = 0
            });
        }
    }
}
