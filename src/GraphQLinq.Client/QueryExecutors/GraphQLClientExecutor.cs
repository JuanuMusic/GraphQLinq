using System;
using GraphQLinq;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using GraphQL;
using Newtonsoft.Json;

namespace GraphQLinq
{
	public class GraphQLClientExecutor<T, TSource> : IQueryExecutor<T, TSource>
	{
        private readonly GraphQLClientContext context;
        private readonly GraphQLQuery query;
        private readonly QueryType queryType;
        private readonly Func<TSource, T> mapper;
        private readonly JsonSerializerOptions jsonSerializerOptions;

        private const string DataPathPropertyName = "data";
        private const string ErrorPathPropertyName = "errors";

        internal GraphQLClientExecutor(GraphQLClientContext context, GraphQLQuery query, QueryType queryType, Func<TSource, T> mapper)
        {
            this.context = context;
            this.query = query;
            this.mapper = mapper;
            this.queryType = queryType;

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

        public async Task<(T Item, IEnumerable<T> Enumerable)> Execute()
        {
            if (queryType == QueryType.Item)
            {

                var res = await context.GraphQLClient.SendQueryAsync<ResultModel<T>>(new GraphQLRequest { Query = query.Query, Variables = query.Variables });
                if(res.Errors != null && res.Errors.Length > 0)
                    throw new Exception(res.Errors[0].Message);
                
                return (res.Data.Result, null);
            }
            else
            {
                var res = await context.GraphQLClient.SendQueryAsync<ResultModel<IEnumerable<object>>>(new GraphQLRequest { Query = query.Query, Variables = query.Variables });
                var str = JsonConvert.SerializeObject(res.Data.Result);
                var theObj = JsonConvert.DeserializeObject<IEnumerable<T>>(str);
                if (res.Errors != null && res.Errors.Length > 0)
                    throw new Exception(res.Errors[0].Message);
                return (default(T), theObj);
            }
        }
    }

    public class ResultModel<T>
    {
        public T Result { get; set; }
    }
}

