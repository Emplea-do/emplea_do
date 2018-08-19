using System;
using System.Collections.Generic;
using Api.Framework;
using AppService.Services;
using Domain.Framework;
using Microsoft.AspNetCore.Mvc;
using Domain;
using Domain.Framework.Dto;
using AppService.Queries;
using Microsoft.Extensions.Logging;

namespace Api.Controllers
{
    [Route("v1.0/[controller]")]
    public class JobsController : BaseController
    {
        IJobService _jobsService;
        public JobsController(IJobService jobsService, ILogger<JobsController> logger) : base(logger)
        {
            _jobsService = jobsService;   
        }

        [HttpGet]
        public ActionResult<PagingResult<JobLimited>> Get(PaginationParameters parameters, JobsQueryParameter queryParameters)
        {
            try
            {
                var result = _jobsService.GetByPagination(new PaginationFilter
                {
                    Search = "",//parameters.search["value"],
                    ItemsPerPage = parameters.Length,
                    Page = parameters.Start / parameters.Length,
                    ColumnToOrder = "Id",
                    Ascending = false
                }, queryParameters);

                return result;
            }
            catch(Exception ex)
            {
                //Do the loggin
                _logger.LogError($"Threw exception while getting jobs {ex}");
            }

            return new BadRequestResult();
        }

        [HttpGet("{id}")]
        public ActionResult<JobLimited> Get(int id)
        {
            try
            {
                var result = _jobsService.GetLimitedById(id);

                if(result == null)
                    return new NotFoundResult();
                return result;
            }
            catch (Exception ex)
            {
                //Do the loggin
            }
            return new BadRequestResult();
        }
    }
}
