using System;
using System.Linq.Expressions;
using AppService.Framework.Queries;
using Domain;

namespace AppService.Queries
{
    public class JobsQuery : Query<Job, JobsQueryParameter>
    {
        public override Expression<Func<Job, bool>> Build(JobsQueryParameter parameter)
        {
            if (parameter.Category > 0)
                QueryExpression = QueryExpression.And(x => x.CategoryId == parameter.Category);

            if (parameter.HireType > 0)
                QueryExpression = QueryExpression.And(x => x.HireTypeId == parameter.HireType);

            if (parameter.OnlyRemote)
                QueryExpression = QueryExpression.And(x => x.IsRemote);

            if (!string.IsNullOrWhiteSpace(parameter.Title))
                QueryExpression = QueryExpression.And(x => x.Title.Contains(parameter.Title));

            if (!string.IsNullOrWhiteSpace(parameter.Description))
                QueryExpression = QueryExpression.And(x => x.Description.Contains(parameter.Description));

            if (!string.IsNullOrWhiteSpace(parameter.HowToApply))
                QueryExpression = QueryExpression.And(x => x.HowToApply.Contains(parameter.HowToApply));
            
            return QueryExpression;
        }
    }

    public class JobsQueryParameter
    {
        public string Title { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        public string HowToApply { get; set; } = String.Empty;
        public int Category { get; set; } = 0;
        public int HireType { get; set; } = 0;
        public bool OnlyRemote { get; set; } = false;
    }
}
