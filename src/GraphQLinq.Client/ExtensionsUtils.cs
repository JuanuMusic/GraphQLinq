using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Query.Builder;
using GraphQLinq.QueryExecutors;

namespace GraphQLinq
{
    public static class ExtensionsUtils
    {
        internal static bool IsValueTypeOrString(this Type type)
        {
            return type.IsValueType || type == typeof(string);
        }

        internal static bool IsList(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>);
        }

        internal static bool HasNestedProperties(this Type type)
        {
            return !IsValueTypeOrString(type);
        }

        internal static Type GetTypeOrListType(this Type type)
        {
            if (type.IsList())
            {
                var genericArguments = type.GetGenericArguments();

                return genericArguments[0].GetTypeOrListType();
            }

            return type;
        }

        internal static Expression RemoveConvert(this Expression expression)
        {
            while ((expression != null)
                   && (expression.NodeType == ExpressionType.Convert
                       || expression.NodeType == ExpressionType.ConvertChecked))
            {
                expression = RemoveConvert(((UnaryExpression)expression).Operand);
            }

            return expression;
        }

        internal static string ToCamelCase(this string input)
        {
            if (char.IsLower(input[0]))
            {
                return input;
            }
            return input.Substring(0, 1).ToLower() + input.Substring(1);
        }

        internal static string ToGraphQlType(this Type type)
        {
            if (type == typeof(bool))
            {
                return "Boolean";
            }

            if (type == typeof(int))
            {
                return "Int";
            }

            if (type == typeof(string))
            {
                return "String!";
            }

            if (type == typeof(float))
            {
                return "Float";
            }

            if (type.IsList())
            {
                var listType = type.GetTypeOrListType();
                return "[" + ToGraphQlType(listType) + "]";
            }

            return type.Name + "!"; // TODO: Is this Ok here?
        }

        public static async Task<T> ToItem<T>(this IQuery<T> query)
        {
            if(query is ContextualizedQuery<T>)
            {
                return await ((ContextualizedQuery<T>)query).ToItem();
            }

            throw new InvalidCastException($"Cannot cast {query} to {typeof(ContextualizedQuery<T>)}");
        }
        
        public static async Task<IEnumerable<T>> ToEnumerable<T>(this IQuery<T> query)
        {
            if (query is ContextualizedQuery<T>)
            {
                return await ((ContextualizedQuery<T>)query).ToEnumerable();
            }

            throw new InvalidCastException($"Cannot cast {query} to {typeof(ContextualizedQuery<T>)}");
        }

        public static async Task<TOutput> ToItem<T, TOutput>(this IQuery<T> query, Expression<Func<T, TOutput>> path)
        {
            if (query is ContextualizedQuery<T>)
            {
                var source = await ((ContextualizedQuery<T>)query).ToItem();
                return ConvertFromPath(source, path);
            }

            throw new InvalidCastException($"Cannot cast {query} to {typeof(ContextualizedQuery<T>)}");
        }

        private static TOutput ConvertFromPath<TSource, TOutput>(TSource source, Expression<Func<TSource, TOutput>> path)
        {
            return default(TOutput);
        }

        ///// <summary>Gets property infos from lambda.</summary>
        ///// <param name="lambda">The lambda.</param>
        ///// <typeparam name="TProperty">The property.</typeparam>
        ///// <returns>The property infos.</returns>
        //private static PropertyInfo GetPropertyInfo<TProperty>(Expression<Func<TSource, TProperty>> lambda)
        //{
        //    RequiredArgument.NotNull(lambda, nameof(lambda));

        //    if (lambda.Body is not MemberExpression member)
        //    {
        //        throw new ArgumentException($"Expression '{lambda}' body is not member expression.");
        //    }

        //    if (member.Member is not PropertyInfo propertyInfo)
        //    {
        //        throw new ArgumentException($"Expression '{lambda}' not refers to a property.");
        //    }

        //    if (propertyInfo.ReflectedType is null)
        //    {
        //        throw new ArgumentException($"Expression '{lambda}' not refers to a property.");
        //    }

        //    Type type = typeof(TSource);
        //    if (type != propertyInfo.ReflectedType && !propertyInfo.ReflectedType.IsAssignableFrom(type))
        //    {
        //        throw new ArgumentException($"Expression '{lambda}' refers to a property that is not from type {type}.");
        //    }

        //    return propertyInfo;
        //}

        public static IQuery<TInput> Select<TInput, TOutput>(this IQuery<TInput> query, Expression<Func<TInput, TOutput>> path)
        {

            if (!(path.Body is MemberExpression member))
            {
                throw new ArgumentException($"Expression '{path}' body is not member expression.");
            }

            if (!(member.Member is PropertyInfo propertyInfo))
            {
                throw new ArgumentException($"Expression '{path}' not refers to a property.");
            }

            if (propertyInfo.ReflectedType is null)
            {
                throw new ArgumentException($"Expression '{path}' not refers to a property.");
            }
            var fields = new List<string>();

            IQuery<TOutput> retVal = new Query<TOutput>(query.Name);

            switch (path.Body)
            {
                case MemberExpression memberExpression:
                    retVal.AddField(s => propertyInfo.Name, s => s.Alias(path.Name));
                    //selectClause = BuildMemberAccessSelectClause(body, selectClause, padding, member.Name);
                    break;

                case NewExpression newExpression:
                    //foreach (var argument in newExpression.Arguments.OfType<MemberExpression>())
                    //{
                    //    var selectField = BuildMemberAccessSelectClause(argument, selectClause, padding, argument.Member.Name);
                    //    fields.Add(selectField);
                    //}
                    //selectClause = string.Join(Environment.NewLine, fields);
                    break;

                case MemberInitExpression memberInitExpression:
                    //foreach (var argument in memberInitExpression.Bindings.OfType<MemberAssignment>())
                    //{
                    //    var selectField = BuildMemberAccessSelectClause(argument.Expression, selectClause, padding, argument.Member.Name);
                    //    fields.Add(selectField);
                    //}
                    //selectClause = string.Join(Environment.NewLine, fields);
                    break;
                default:
                    throw new NotSupportedException($"Selector of type {path.Body.NodeType} is not implemented yet");
            }

            //var isScalarQuery = string.IsNullOrEmpty(selectClause);
            //selectClause = Environment.NewLine + selectClause + Environment.NewLine;

            //var queryParameters = passedArguments.Any() ? $"({string.Join(", ", passedArguments.Select(pair => $"{pair.Key}: ${pair.Key}"))})" : "";
            //var queryParameterTypes = queryVariables.Any() ? $"({string.Join(", ", queryVariables.Select(pair => $"${pair.Key}: {pair.Value.GetType().ToGraphQlType()}"))})" : "";

            //var graphQLQuery = string.Format(isScalarQuery ? ScalarQueryTemplate : QueryTemplate, queryParameterTypes, ResultAlias, graphQuery.QueryName, queryParameters, selectClause);

            //var dictionary = new Dictionary<string, object> { { "query", graphQLQuery }, { "variables", queryVariables } };

            //var json = JsonSerializer.Serialize(dictionary, new JsonSerializerOptions
            //{
            //    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            //    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            //    Converters = { new JsonStringEnumConverter() }
            //});

            //return new GraphQLQuery(graphQLQuery, queryVariables, json);
            return query;
        }
    }
}