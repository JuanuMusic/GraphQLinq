namespace TestServer
{
    using GraphQL.Query.Builder;
    using GraphQLinq;
    using System;
    using System.Collections.Generic;

    public class QueryContext : GraphContext
    {
        public QueryContext() : this("http://localhost:10000/graphql")
        {
        }

        public QueryContext(string baseUrl) : base(baseUrl, "")
        {
        }

        public IQuery<User> UserTemporaryFixForNullable(int? id)
        {
            var parameterValues = new object[] { id };
            return (IQuery<User>)BuildItemQuery<User>(parameterValues, "userTemporaryFixForNullable");
        }

        public IQuery<User> User(int id)
        {
            var parameterValues = new object[] { id };
            return (IQuery<User>)BuildItemQuery<User>(parameterValues, "user");
        }

        public IQuery<User> FailUser()
        {
            var parameterValues = new object[] { };
            return (IQuery<User>)BuildItemQuery<User>(parameterValues, "failUser");
        }
    }
}