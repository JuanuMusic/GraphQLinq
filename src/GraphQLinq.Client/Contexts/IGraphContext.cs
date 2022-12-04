using System;
using GraphQLinq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GraphQL.Query.Builder;

namespace GraphQLinq
{
	public interface IGraphContext<TClient, TQueryOutput> : IDisposable
    {
        TClient Client { get;}
        TQueryOutput BuildCollectionQuery<T>(object[] parameterValues, [CallerMemberName] string queryName = null);

        TQueryOutput BuildItemQuery<T>(object[] parameterValues, [CallerMemberName] string queryName = null);

        IQueryExecutor<T, TSource> BuildExecutor<T, TSource>(GraphQLQuery query, QueryType queryType, Func<TSource, T> mapper);
    }
}

