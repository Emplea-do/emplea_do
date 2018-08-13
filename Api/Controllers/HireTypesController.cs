using System;
using Api.Framework;
using AppService.Services;
using Domain;
using Domain.Framework;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("v1.0/[controller]")]
    public class HireTypesController
    {
        IHireTypesService _hireTypesService;

        public HireTypesController(IHireTypesService hireTypesService)
        {
            _hireTypesService = hireTypesService;
        }


        [HttpGet]
        public PagingResult<HireType> Get(PaginationParameters parameters)
        {
            return _hireTypesService.GetByPagination(new PaginationFilter
            {
                ItemsPerPage = 10,
                Page = 0
            });
        }
    }
}
