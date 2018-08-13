using System;
using AppService.Framework;
using Domain;

namespace AppService.Services
{
    public interface IMutableService<T> where T : Entity
    {
        TaskResult ValidateOnUpdate(T entity);
        TaskResult ValidateOnDelete(T entity);
        TaskResult ValidateOnCreate(T entity);

        TaskResult Update(T entity);
        TaskResult Delete(int entityId);
        TaskResult Create(T entity);
    }
}
