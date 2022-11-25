using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;

namespace GraphQLinq
{
    public class GraphContext : IDisposable
    {
        private readonly bool ownsHttpClient = false;

        //public HttpClient HttpClient { get; }
        public GraphQLHttpClient Client { get; }

        protected GraphContext(GraphQLHttpClient httpClient)
        {
            if (httpClient == null)
            {
                throw new ArgumentNullException($"{nameof(httpClient)} cannot be empty");
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
            var serializer = new NewtonsoftJsonSerializer();
            Client = new GraphQLHttpClient(baseUrl, serializer);

            if (!string.IsNullOrEmpty(authorization))
            {
                Client.HttpClient.DefaultRequestHeaders.Add("Authorization", authorization);
            }
        }

        public JsonSerializerOptions JsonSerializerOptions { get; set; } = new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            Converters = { new JsonStringEnumConverter() },
        };

        protected GraphCollectionQuery<T> BuildCollectionQuery<T>(object[] parameterValues, [CallerMemberName] string queryName = null, object variables = null)
        {
            var arguments = BuildDictionary(parameterValues, queryName);
            return new GraphCollectionQuery<T, T>(this, queryName, variables) { Arguments = arguments};
        }

        protected virtual GraphItemQuery<T> BuildItemQuery<T>(object[] parameterValues, [CallerMemberName] string queryName = null, object variables = null)
        {
            var arguments = BuildDictionary(parameterValues, queryName);
            return new GraphItemQuery<T, T>(this, queryName, variables) { Arguments = arguments};
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
    }
}