using System;
using System.Collections.Generic;
using System.Linq;
using AppServices.Framework;
using AppServices.Data.Repositories;
using Domain;
using Domain.Entities;

namespace AppServices.Services
{
    public abstract class BaseService<T, U> : IBaseService<T, U> where T : Entity where U : IBaseRepository<T>
    {
        protected U _mainRepository;
        public BaseService(U mainRepository)
        {
            _mainRepository = mainRepository;
        }

        protected abstract TaskResult<T> ValidateOnCreate(T entity);
        protected abstract TaskResult<T> ValidateOnDelete(T entity);
        protected abstract TaskResult<T> ValidateOnUpdate(T entity);

        public TaskResult<T> Create(T entity)
        {
            var taskResult = ValidateOnCreate(entity);
            if (taskResult.Success)
            {
                try
                {
                    _mainRepository.Insert(entity);
                    _mainRepository.CommitChanges();
                    taskResult.AddMessage("Registro agregado exitosamente");
                }
                catch(Exception ex)
                {
                    taskResult.AddErrorMessage(ex.Message);
                    if(ex.InnerException != null)
                        taskResult.AddErrorMessage(ex.InnerException.Message);

                }
            }
            return taskResult;
        }
        public TaskResult<T> Update(T entity)
        {
            var taskResult = ValidateOnUpdate(entity);
            if (taskResult.Success)
            {
                try
                {
                    _mainRepository.Update(entity);
                    _mainRepository.CommitChanges();
                    taskResult.AddMessage("Registro actualizado exitosamente");
                    taskResult.Data = entity;
                }
                catch (Exception ex)
                {
                    taskResult.AddErrorMessage(ex.Message);
                    if (ex.InnerException != null)
                        taskResult.AddErrorMessage(ex.InnerException.Message);
                }
            }
            return taskResult;
        }

        public TaskResult Delete(T entity)
        {
            var taskResult = ValidateOnDelete(entity);
            if (taskResult.Success)
            {
                try
                {
                    _mainRepository.SoftDelete(entity.Id);
                    _mainRepository.CommitChanges();
                    taskResult.AddMessage("Registro eliminado exitosamente");
                    taskResult.Data = entity;
                }
                catch (Exception ex)
                {
                    taskResult.AddErrorMessage(ex.Message);
                    if (ex.InnerException != null)
                        taskResult.AddErrorMessage(ex.InnerException.Message);
                }
            }
            return taskResult;
        }
        public virtual List<T> GetAll()
        {
            return _mainRepository.Get(x => x.IsActive).ToList();
        }
    }

    public interface IBaseService<T, U> where T:Entity where U:IBaseRepository<T>
    {
        List<T> GetAll();
        TaskResult<T> Create(T entity);
        TaskResult<T> Update(T entity);
        TaskResult Delete(T entity);
    }
}
