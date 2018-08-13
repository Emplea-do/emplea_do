using System;
using System.Linq.Expressions;
using Domain;

namespace AppService.Framework.Queries
{
    public abstract class Query<TEntity, TParameter> where TEntity : Entity
    {
        protected Expression<Func<TEntity, bool>> QueryExpression;

        protected Query()
        {
            Clean();
        }

        public abstract Expression<Func<TEntity, bool>> Build(TParameter parameter);

        protected void Clean()
        {
            QueryExpression = null;
            QueryExpression = PredicateBuilder.True<TEntity>();
        }
    }
}
