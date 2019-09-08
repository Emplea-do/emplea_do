using System;
using System.Collections.Generic;
using Domain;
using Domain.Entities;

namespace AppServices.Services
{
    public abstract class BaseService<T> : IBaseService<T> where T : Entity
    {
        public abstract List<T> GetAll();
    }

    public interface IBaseService<T> where T:Entity
    {
        List<T> GetAll();
    }
}
