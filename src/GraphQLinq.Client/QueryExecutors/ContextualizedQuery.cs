using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Query.Builder;

namespace GraphQLinq.QueryExecutors
{
    public interface IContextualizedQuery<TContext> : IQuery
    {
        TContext Context { get; }

    }
    /// <summary>
    /// Represents a Query that knows about the context, and can execute queries against the context client.
    /// </summary>
    /// <typeparam name="T"></typeparam>
	public class ContextualizedQuery<T> : Query<T>, IContextualizedQuery<GraphQLClientContext>
	{
        public GraphQLClientContext Context { get; }
        /// <summary>
        /// Creates a new instance of a ContextualizedQuery
        /// </summary>
        /// <param name="queryName">The name of the query</param>
        /// <param name="context">The context on which this query should operate.</param>
		public ContextualizedQuery(string queryName, GraphQLClientContext context) : base(queryName)
		{
            if (context == null) throw new NullReferenceException("Context cannot be null");
            this.Context = context;
        }

        /// <summary>
        /// Converts a <see cref="Query{T}"/> into a <see cref="ContextualixedQuery{T}" />.
        /// </summary>
        /// <param name="query">The query to convert</param>
        /// <param name="context">The context to contextualize the query.</param>
        /// <returns></returns>
        public ContextualizedQuery(Query<T> query, GraphQLClientContext context) : base(query.Name)
        {
            if (context == null) throw new NullReferenceException("Context cannot be null");
            this.Context = context;
            this.Alias(query.AliasName);
            this.SelectList.AddRange(query.SelectList);
            this.AddArguments(query.Arguments);
        }


        public async Task<T> ToItem()
        {
            var req = new GraphQLRequest("{" + this.Build() + "}", null, this.Name);
            var resp = await this.Context.Client.SendQueryAsync<T>(req);
            if (resp.Errors != null && resp.Errors.Length > 0)
                throw new Exception(resp.Errors[0].Message);
            return resp.Data;
        }

        public async Task<IEnumerable<T>> ToEnumerable()
        {
            var req = new GraphQLRequest("{" + this.Build() + "}", null, this.Name);
            var resp = await this.Context.Client.SendQueryAsync<IEnumerable<T>>(req);
            if (resp.Errors != null && resp.Errors.Length > 0)
                throw new Exception(resp.Errors[0].Message);
            return resp.Data;
        }
    }
}

