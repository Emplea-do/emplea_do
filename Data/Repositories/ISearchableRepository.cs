using System;
using System.Collections.Generic;
using Domain;

namespace Data.Repositories
{
    public interface ISearchableRepository<T> where T : Entity
    {
        IEnumerable<T> GetSearchResults(string search, int? accountId);
    }
}
