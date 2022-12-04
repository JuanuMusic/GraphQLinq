using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using GraphQL.Query.Builder;

namespace GraphQLinq
{
    public class GraphContext : IGraphContext<HttpClient, GraphQuery>
    {
        private readonly bool ownsHttpClient = false;

        public HttpClient Client { get; }

        protected GraphContext(HttpClient httpClient)
        {
            if (httpClient == null)
            {
                throw new ArgumentNullException($"{nameof(httpClient)} cannot be empty");
            }

            if (httpClient.BaseAddress == null)
            {
                throw new ArgumentException($"{nameof(httpClient.BaseAddress)} cannot be empty");
            }

            Client = httpClient;
        }

        protected GraphContext(string baseUrl, string authorization)
        {
            if (string.IsNullOrEmpty(baseUrl))
            {
                throw new ArgumentException($"{nameof(baseUrl)} cannot be empty");
            }

            ownsHttpClient = true;
            Client = new HttpClient();

            if (!string.IsNullOrEmpty(baseUrl))
            {
                Client.BaseAddress = new Uri(baseUrl);
            }

            if (!string.IsNullOrEmpty(authorization))
            {
                Client.DefaultRequestHeaders.Add("Authorization", authorization);
            }
        }

        public JsonSerializerOptions JsonSerializerOptions { get; set; } = new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            Converters = { new JsonStringEnumConverter() },
        };

        public GraphQuery BuildCollectionQuery<T>(object[] parameterValues, [CallerMemberName] string queryName = null)
        {
            var arguments = BuildDictionary(parameterValues, queryName);
            return new GraphCollectionQuery<T, T>(this, queryName) { Arguments = arguments };
        }

        public GraphQuery BuildItemQuery<T>(object[] parameterValues, [CallerMemberName] string queryName = null)
        {
            var arguments = BuildDictionary(parameterValues, queryName);
            return new GraphItemQuery<T, T>(this, queryName) { Arguments = arguments };
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
            => new GraphQueryExecutor<T, TSource>(this, query, queryType, mapper);
    }
}