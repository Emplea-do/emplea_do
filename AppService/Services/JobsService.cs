using System;
using AppService.Framework;
using Domain;
using Domain.Framework;
using Domain.Framework.Dto;
using System.Linq;
using Data.Repositories;
using AppService.Framework.Extensions;
using AppService.Queries;

namespace AppService.Services
{
    public class JobsService : BaseService<Job>, IJobsService
    {
        readonly IJobRepository _jobsRepository;
        readonly ICategoriesService _categoriesService;
        readonly IHireTypesService _hireTypesService;
        readonly IUserRepository _userRepository;

        public JobsService(IJobRepository jobsRepository, ICategoriesService categoriesService, 
                           IHireTypesService hireTypesService, IUserRepository userRepository)
        {
            _jobsRepository = jobsRepository;
            _categoriesService = categoriesService;
            _hireTypesService = hireTypesService;
            _userRepository = userRepository;
        }

        public PagingResult<JobLimited> GetByPagination(PaginationFilter paginationFilter, JobsQueryParameter queryParameters)
        {
            var queryExpression = new JobsQuery().Build(queryParameters);

            var query = _jobsRepository.GetWithPagination(queryExpression.Compile(),
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

        public JobLimited GetById(int id)
        {
            return _jobsRepository.GetJobLimitedById(id);
        }



        public TaskResult ValidateOnUpdate(Job entity)
        {
            if (entity == null)
                TaskResult.AddErrorMessage("La posición de trabajo que intentas eliminar no existe");
            
            return TaskResult;
        }
        public TaskResult Update(Job updatedJob)
        {
            var oldEntity= _jobsRepository.GetById(updatedJob.Id);
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
                        oldEntity.Location = oldEntity.Location == null
                            ? new Location()
                            : oldEntity.Location;

                        oldEntity.Location.Latitude = updatedJob.Location.Latitude;
                        oldEntity.Location.Longitude = updatedJob.Location.Longitude;
                        oldEntity.Location.Name = updatedJob.Location.Name;
                        oldEntity.Location.PlaceId = updatedJob.Location.PlaceId;
                    }

                    oldEntity.HireType = updatedJob.HireType;
                    if (updatedJob.JoelTest != null)
                    {
                        oldEntity.JoelTest = oldEntity.JoelTest == null
                            ? new JoelTest()
                            : oldEntity.JoelTest;

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
                    _jobsRepository.Update(oldEntity);
                    _jobsRepository.CommitChanges();
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
            var entity = _jobsRepository.GetById(entityId);
            ValidateOnDelete(entity);
            if (TaskResult.ExecutedSuccesfully)
            {
                try
                {
                    _jobsRepository.SoftDelete(entityId);
                    _jobsRepository.CommitChanges();
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
            ValidateOnCreate(entity);
            if(TaskResult.ExecutedSuccesfully)
            {
                try
                {
                    _jobsRepository.Insert(entity);
                    _jobsRepository.CommitChanges();
                }
                catch(Exception ex)
                {
                    TaskResult.Exception = ex;
                    TaskResult.AddErrorMessage(ex.Message);
                }
            }
            return TaskResult;
        }
    }
    public interface IJobsService : IMutableService<Job>
    {
        PagingResult<JobLimited> GetByPagination(PaginationFilter paginationFilter, JobsQueryParameter queryParameters);
        JobLimited GetById(int id);
    }
}
