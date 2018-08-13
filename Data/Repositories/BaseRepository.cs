using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Domain;
using LinqKit;


namespace Data.Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : Entity
    {

        protected readonly EmpleadoDbContext Database;

        protected BaseRepository(EmpleadoDbContext database)
        {
            Database = database;
        }

        public int CommitChanges()
        {
            return Database.SaveChanges();
        }

        public T GetById(int id)
        {
            return Database.Set<T>().Find(id);
        }

        public IQueryable<T> GetAll()
        {
            return Database.Set<T>();
        }

        public IQueryable<T> Get(Expression<Func<T, bool>> where, string includeProperties = "")
        {
            var query = Database.Set<T>().AsQueryable();

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (where != null)
                query = query.AsExpandable().Where(where);

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
                query = query.AsExpandable().Where(where);

            return query;
        }

        public IQueryable<T> Get(params Expression<Func<T, object>>[] include)
        {
            var query = Database.Set<T>().AsQueryable().AsExpandable();

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
                throw new Exception("This object does not exists");

            entity.DeletedAt = DateTime.UtcNow;

            Update(entity);
        }

        public virtual void Delete(T entity)
        {
            Database.Set<T>().Remove(entity);
        }

        public virtual void Delete(int id)
        {
            var entity = GetById(id);
            Delete(entity);

        }

        public virtual int Count()
        {
            return Database.Set<T>().Count();
        }
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
        void Delete(T entity);
        void Delete(int id);
        int Count();
    }
}