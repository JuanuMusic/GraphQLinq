using System;
using GraphQLinq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace GraphQLinq
{
	public interface IGraphContext : IDisposable
    {
        GraphCollectionQuery<T> BuildCollectionQuery<T>(object[] parameterValues, [CallerMemberName] string queryName = null);

        GraphItemQuery<T> BuildItemQuery<T>(object[] parameterValues, [CallerMemberName] string queryName = null);

        IQueryExecutor<T, TSource> BuildExecutor<T, TSource>(string query, QueryType queryType, Func<TSource, T> mapper);
    }
}

