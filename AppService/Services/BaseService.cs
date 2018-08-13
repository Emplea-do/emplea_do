using System;
using AppService.Framework;

namespace AppService.Services
{
    public abstract class BaseService<T>
    {
        protected TaskResult TaskResult { get; set; }

        protected BaseService()
        {
            TaskResult = new TaskResult { ExecutedSuccesfully = true };
        }
    }
}
