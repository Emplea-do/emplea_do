﻿using System;
using AppService.Framework;
using Domain;
using Domain.Framework;
using Domain.Framework.Dto;
using System.Linq;
using Data.Repositories;
using AppService.Framework.Extensions;
using AppService.Queries;
using System.Collections.Generic;

namespace AppService.Services
{
    public class JobService : BaseService, IJobService
    {
        readonly IJobRepository _jobRepository;
        readonly ICategoryService _categoryService;
        readonly IHireTypeService _hireTypeService;
        readonly IUserRepository _userRepository;

        public JobService(IJobRepository jobsRepository, ICategoryService categoriesService, 
                           IHireTypeService hireTypesService, IUserRepository userRepository)
        {
            _jobRepository = jobsRepository;
            _categoryService = categoriesService;
            _hireTypeService = hireTypesService;
            _userRepository = userRepository;
        }

        public TaskResult ValidateOnUpdate(Job entity)
        {
            if (entity == null)
                TaskResult.AddErrorMessage("La posición de trabajo que intentas eliminar no existe");
            
            return TaskResult;
        }

        public TaskResult Update(Job updatedJob)
        {
            var oldEntity= _jobRepository.GetById(updatedJob.Id);
            ValidateOnUpdate(updatedJob);
            if (TaskResult.ExecutedSuccesfully)
            {
                try
                {
                    oldEntity.Title = updatedJob.Title;
                    oldEntity.Approved = updatedJob.Approved;
                    oldEntity.Category = updatedJob.Category;
                    oldEntity.Company.Email = updatedJob.Company.Email;
                    oldEntity.Company.LogoUrl = updatedJob.Company.LogoUrl;
                    oldEntity.Company.Name = updatedJob.Company.Name;
                    oldEntity.Company.Url = updatedJob.Company.Url;
                    oldEntity.Description = updatedJob.Description;
                    oldEntity.HowToApply = updatedJob.HowToApply;
                    oldEntity.IsActive = updatedJob.IsActive;
                    oldEntity.IsRemote = updatedJob.IsRemote;

                    if (updatedJob.Location != null)
                    {
                        oldEntity.Location = oldEntity.Location ?? new Location();
                        oldEntity.Location.Latitude = updatedJob.Location.Latitude;
                        oldEntity.Location.Longitude = updatedJob.Location.Longitude;
                        oldEntity.Location.Name = updatedJob.Location.Name;
                        oldEntity.Location.PlaceId = updatedJob.Location.PlaceId;
                    }

                    oldEntity.HireType = updatedJob.HireType;
                    if (updatedJob.JoelTest != null)
                    {
                        oldEntity.JoelTest = oldEntity.JoelTest ?? new JoelTest();

                        oldEntity.JoelTest.HasBestTools = updatedJob.JoelTest.HasBestTools;
                        oldEntity.JoelTest.HasBugDatabase = updatedJob.JoelTest.HasBugDatabase;
                        oldEntity.JoelTest.HasBusFixedBeforeProceding = updatedJob.JoelTest.HasBusFixedBeforeProceding;
                        oldEntity.JoelTest.HasDailyBuilds = updatedJob.JoelTest.HasDailyBuilds;
                        oldEntity.JoelTest.HasHallwayTests = updatedJob.JoelTest.HasHallwayTests;
                        oldEntity.JoelTest.HasOneStepBuilds = updatedJob.JoelTest.HasOneStepBuilds;
                        oldEntity.JoelTest.HasQuiteEnvironment = updatedJob.JoelTest.HasQuiteEnvironment;
                        oldEntity.JoelTest.HasSourceControl = updatedJob.JoelTest.HasSourceControl;
                        oldEntity.JoelTest.HasSpec = updatedJob.JoelTest.HasSpec;
                        oldEntity.JoelTest.HasTesters = updatedJob.JoelTest.HasTesters;
                        oldEntity.JoelTest.HasUpToDateSchedule = updatedJob.JoelTest.HasUpToDateSchedule;
                        oldEntity.JoelTest.HasWrittenTest = updatedJob.JoelTest.HasWrittenTest;
                    }
                    _jobRepository.Update(oldEntity);
                    _jobRepository.CommitChanges();
                }
                catch (Exception ex)
                {
                    TaskResult.Exception = ex;
                    TaskResult.AddErrorMessage(ex.Message);
                }
            }
            return TaskResult;
        }

        public TaskResult ValidateOnDelete(Job entity)
        {
            if (entity == null)
                TaskResult.AddErrorMessage("La posición de trabajo que intentas eliminar no existe");
            
            return TaskResult;
        }

        public TaskResult Delete(int entityId)
        {
            var entity = _jobRepository.GetById(entityId);
            ValidateOnDelete(entity);
            if (TaskResult.ExecutedSuccesfully)
            {
                try
                {
                    _jobRepository.SoftDelete(entityId);
                    _jobRepository.CommitChanges();
                }
                catch (Exception ex)
                {
                    TaskResult.Exception = ex;
                    TaskResult.AddErrorMessage(ex.Message);
                }
            }
            return TaskResult;
        }

        public TaskResult ValidateOnCreate(Job entity)
        {
            if (entity.UserId == null)
                TaskResult.AddErrorMessage("La posición de trabajo debe tener un usuario asignado");

            var user = _userRepository.GetById(entity.UserId.Value);
            if (user == null)
                TaskResult.AddErrorMessage("El usuario especificado no existe!");

            return TaskResult;
        }

        public TaskResult Create(Job entity)
        {
            //TODO removing this for later. Is checking the user login but there is no password column on the DB. 
            // Before removing something from the user model, We should re-check the SecurityService
            //ValidateOnCreate(entity);
            if(TaskResult.ExecutedSuccesfully)
            {
                try
                {
                    _jobRepository.Insert(entity);
                    _jobRepository.CommitChanges();
                }
                catch(Exception ex)
                {
                    TaskResult.Exception = ex;
                    TaskResult.AddErrorMessage(ex.Message);
                }
            }
            return TaskResult;
        }

        public PagingResult<JobLimited> GetByPagination(PaginationFilter paginationFilter, JobsQueryParameter queryParameters)
        {
            var queryExpression = new JobsQuery().Build(queryParameters);

            var query = _jobRepository.GetWithPagination(queryExpression.Compile(),
                                                          paginationFilter.Page,
                                                          paginationFilter.ItemsPerPage);

            if (!string.IsNullOrWhiteSpace(paginationFilter.ColumnToOrder))
            {
                query = paginationFilter.Ascending
                    ? query.OrderBy(paginationFilter.ColumnToOrder)
                    : query.OrderByDescending(paginationFilter.ColumnToOrder);
            }


            return new PagingResult<JobLimited>
            {
                TotalItems = query.Count(),
                Data = query,
                ItemsPerPage = paginationFilter.ItemsPerPage,
                Page = paginationFilter.Page
            };
        }

        public List<Job> GetCompanyRelatedJobs(int id, string name) => _jobRepository.GetRelatedJobs(id, name).ToList<Job>();

        public JobLimited GetLimitedById(int id) => _jobRepository.GetJobLimitedById(id);

        public Job GetById(int id) => _jobRepository.Get(x=>x.IsActive && x.Id == id, x => x.JoelTest, x => x.Category, x => x.Company, x => x.HireType).FirstOrDefault();

        public IEnumerable<CategoryCountDto> GetJobCountByCategory() => _jobRepository.GetJobCountByCategory();

        public IEnumerable<Job> GetLatestJobs(int quantity) => _jobRepository.GetLatestJobs(quantity);

        public IEnumerable<Job> GetAllJobsPagedByFilters(JobPagingParameter parameter) => _jobRepository.GetAllJobsPagedByFilters(parameter);

        public void ToggleHideState(Job jobOpportunity)
        {
            jobOpportunity.IsHidden = !jobOpportunity.IsHidden;
            _jobRepository.Update(jobOpportunity);
            _jobRepository.CommitChanges();
        }

        public void UpdateViewCount(int id)
        {
            var job = _jobRepository.GetById(id);

            if (job == null) return;

            job.ViewCount++;
            _jobRepository.Update(job);
        }
    }
    public interface IJobService : IMutableService<Job>
    {
        PagingResult<JobLimited> GetByPagination(PaginationFilter paginationFilter, JobsQueryParameter queryParameters);
        JobLimited GetLimitedById(int id);
        Job GetById(int id);
        void UpdateViewCount(int id);
        List<Job> GetCompanyRelatedJobs(int id, string companyName); //TODO: Change for just CompanyId
        IEnumerable<CategoryCountDto> GetJobCountByCategory();
        IEnumerable<Job> GetLatestJobs(int quantity);
        IEnumerable<Job> GetAllJobsPagedByFilters(JobPagingParameter parameter);
        void ToggleHideState(Job jobOpportunity);
    }
}
