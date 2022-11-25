using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Client;
using GraphQL;

namespace GraphQLinq
{
    class GraphQueryExecutor<T, TSource>
    {
        private readonly GraphContext context;
        private readonly string query;
        IReadOnlyDictionary<string, object> variables;
        object actualVariables;
        private readonly QueryType queryType;
        private readonly Func<TSource, T> mapper;
        private readonly JsonSerializerOptions jsonSerializerOptions;

        private const string DataPathPropertyName = "data";
        private const string ErrorPathPropertyName = "errors";

        internal GraphQueryExecutor(GraphContext context, string query, IReadOnlyDictionary<string, object> variables, QueryType queryType, Func<TSource, T> mapper, object actualVariables)
        {
            this.context = context;
            this.query = query;
            this.mapper = mapper;
            this.queryType = queryType;
            this.variables = variables;
            this.actualVariables = actualVariables;

            jsonSerializerOptions = context.JsonSerializerOptions;
        }

        private T JsonElementToItem(JsonElement jsonElement)
        {
            if (mapper != null)
            {
                var result = jsonElement.Deserialize<TSource>(jsonSerializerOptions);
                return mapper.Invoke(result);
            }
            else
            {
                var result = jsonElement.Deserialize<T>(jsonSerializerOptions);
                return result;
            }
        }

        internal async Task<(T Item, IEnumerable<T> Enumerable)> Execute()
        {
            
            var dynamicVars = Dict2Obj(variables);
            var request = new GraphQLRequest { Query = query, Variables = actualVariables } ;

            if (queryType == QueryType.Item)
            {
                var response = await context.Client.SendQueryAsync<ResultRoot<T>>(request);
                return (response.Data.Result, null);
            } else
            {
                var response = await context.Client.SendQueryAsync<ResultRoot<IEnumerable<T>>>(request);
                return (default(T), response.Data.Result);
            }
            //// If an error occrrurded
            //if(response.Errors != null && response.Errors.Length > 0)
            //    throw new Exception(response.Errors[0].Message);

            //{
            //    return (response.Data.Result, null);
            //}

            //return (default, response.Data.Result as IEnumerable<T>);
            //return (default, resultElement.EnumerateArray().Select(JsonElementToItem));
        }

        object Dict2Obj(IReadOnlyDictionary<string, object> dict)
        {
            dynamic vars = new System.Dynamic.ExpandoObject();

            foreach (var kvp in variables)
            {
                ((IDictionary<String, Object>)vars)[kvp.Key] = kvp.Value;
            }

            return vars;
        }
    }
}