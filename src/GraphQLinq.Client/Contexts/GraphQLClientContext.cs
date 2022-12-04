using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.SystemTextJson;
using GraphQL.Query.Builder;
using GraphQLinq.QueryExecutors;
using static GraphQL.Instrumentation.Metrics;

namespace GraphQLinq
{
    public class GraphQLClientContext : IGraphContext<GraphQLHttpClient, IContextualizedQuery<GraphQLClientContext>> { 
        private readonly bool ownsHttpClient = false;

        public GraphQLHttpClient Client { get; }

        protected GraphQLClientContext(GraphQLHttpClient httpClient)
        {
            if (httpClient == null)
            {
                throw new ArgumentNullException($"{nameof(httpClient)} cannot be empty");
            }

            if (httpClient.HttpClient.BaseAddress == null)
            {
                throw new ArgumentException($"{nameof(httpClient.HttpClient.BaseAddress)} cannot be empty");
            }

            Client = httpClient;
        }

        protected GraphQLClientContext(string baseUrl, string authorization)
        {
            if (string.IsNullOrEmpty(baseUrl))
            {
                throw new ArgumentException($"{nameof(baseUrl)} cannot be empty");
            }

            ownsHttpClient = true;
            Client = new GraphQLHttpClient(baseUrl, new SystemTextJsonSerializer());

            if (!string.IsNullOrEmpty(authorization))
            {
                Client.HttpClient.DefaultRequestHeaders.Add("Authorization", authorization);
            }
        }

        public JsonSerializerOptions JsonSerializerOptions { get; set; } = new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            Converters = { new JsonStringEnumConverter() },
        };

        public IContextualizedQuery<GraphQLClientContext> BuildCollectionQuery<T>(object[] parameterValues, [CallerMemberName] string queryName = null)
        {
            var arguments = BuildDictionary(parameterValues, queryName);
            return (IContextualizedQuery<GraphQLClientContext>)new ContextualizedQuery<IEnumerable<T>>(queryName, this).AddArguments(arguments);
        }

        public IContextualizedQuery<GraphQLClientContext> BuildItemQuery<T>(object[] parameterValues, [CallerMemberName] string queryName = null)
        {
            var arguments = BuildDictionary(parameterValues, queryName);
            return (IContextualizedQuery<GraphQLClientContext>)new ContextualizedQuery<T>(queryName, this).AddArguments(arguments);
        }

        private Dictionary<string, object> BuildDictionary(object[] parameterValues, string queryName)
        {
            var parameters = GetType().GetMethod(queryName, BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance).GetParameters();
            var arguments = parameters.Zip(parameterValues, (info, value) => new { info.Name, Value = value }).ToDictionary(arg => arg.Name, arg => arg.Value);
            return arguments;
        }



        public void Dispose()
        {
            if (ownsHttpClient)
            {
                Client.Dispose();
            }
        }

        public IQueryExecutor<T, TSource> BuildExecutor<T, TSource>(GraphQLQuery query, QueryType queryType, Func<TSource, T> mapper)
            => new GraphQLClientExecutor<T, TSource>(this, query, queryType, mapper);
    }   
}