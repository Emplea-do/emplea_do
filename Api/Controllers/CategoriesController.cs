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
        ICategoriesService _categoriesService;

        public CategoriesController(ICategoriesService categoriesService)
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
