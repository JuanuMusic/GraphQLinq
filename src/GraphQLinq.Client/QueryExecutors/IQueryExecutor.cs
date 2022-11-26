using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraphQLinq
{
    public interface IQueryExecutor<T, TSource>
    {
        Task<(T Item, IEnumerable<T> Enumerable)> Execute();
    }
}

