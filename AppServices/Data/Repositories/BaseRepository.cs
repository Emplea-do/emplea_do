using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Domain;
using Domain.Entities;

namespace AppServices.Data.Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : Entity
    {
        protected readonly EmpleaDbContext Database;
        protected DbSet<T> DbSet => Database.Set<T>();

        protected BaseRepository(EmpleaDbContext database)
        {
            Database = database;
        }

        public int CommitChanges() => Database.SaveChanges();

        public T GetById(int id) => DbSet.Find(id);

        public IQueryable<T> GetAll() => DbSet;

        public IQueryable<T> GetAll(params Expression<Func<T, object>>[] include) => Get(null, include);

        public IQueryable<T> Get(Expression<Func<T, bool>> where, string includeProperties = "")
        {
            var query = Database.Set<T>().AsQueryable();

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (where != null)
                query = query.Where(where);

            return query;
        }

        public IQueryable<T> Get(Expression<Func<T, bool>> @where, params Expression<Func<T, object>>[] include)
        {
            var query = Database.Set<T>().AsQueryable();

            foreach (var includeProperty in include)
            {
                query = query.Include(includeProperty);
            }

            if (where != null)
                query = query.Where(where);

            return query;
        }

        public IQueryable<T> Get(params Expression<Func<T, object>>[] include)
        {
            var query = Database.Set<T>().AsQueryable();

            foreach (var includeProperty in include)
            {
                query = query.Include(includeProperty);
            }

            return query;
        }

        public virtual T Insert(T entity)
        {
            Database.Set<T>().Add(entity);
            return entity;
        }

        public virtual T Update(T entity)
        {
            Database.Set<T>().Attach(entity);
            Database.Entry(entity).State = EntityState.Modified;

            return entity;
        }

        public virtual T Update(T entity, int id)
        {
            var entry = Database.Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                var attachedEntity = Database.Set<T>().Find(id);

                if (attachedEntity != null)
                {
                    entity.CreatedAt = attachedEntity.CreatedAt;
                    entity.Id = attachedEntity.Id;

                    var attachedEntry = Database.Entry(attachedEntity);
                    attachedEntry.CurrentValues.SetValues(entity);
                }
                else
                {
                    entry.State = EntityState.Modified;
                }
            }
            return entity;
        }

        public virtual void SoftDelete(int id)
        {
            var entity = GetById(id);

            if (entity == null)
                throw new Exception("Este objeto no existe");

            entity.DeletedAt = DateTime.UtcNow;
            entity.IsActive = false;

            Update(entity);
        }

        public virtual int Count() => Database.Set<T>().Count();
    }

    public interface IBaseRepository<T>
    {
        int CommitChanges();
        T GetById(int id);
        IQueryable<T> GetAll();
        IQueryable<T> Get(Expression<Func<T, bool>> where, string includeProperties = "");
        IQueryable<T> Get(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] include);
        IQueryable<T> Get(params Expression<Func<T, object>>[] include);
        T Insert(T entity);
        T Update(T entity);
        T Update(T entity, int id);
        void SoftDelete(int id);
        int Count();
    }
}