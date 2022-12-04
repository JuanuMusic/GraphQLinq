namespace SpaceX
{
    using GraphQL.Client.Http;
    using GraphQL.Query.Builder;
    using GraphQLinq;
    using GraphQLinq.QueryExecutors;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;

    public partial class QueryContext : GraphQLClientContext
    {
        public QueryContext() : this("https://api.spacex.land/graphql")
        {
        }

        public QueryContext(string baseUrl) : base(baseUrl, "")
        {
        }

        public QueryContext(GraphQLHttpClient gqlClient) : base(gqlClient)
        {
        }

        public ContextualizedQuery<IEnumerable<Users>> Users(List<Users_select_column> distinct_on, int? limit, int? offset, List<Users_order_by> order_by, Users_bool_exp where)
        {
            var parameterValues = new object[] { distinct_on, limit, offset, order_by, where };
            return (ContextualizedQuery<IEnumerable<Users>>)BuildCollectionQuery<Users>(parameterValues, "users");
        }

        public ContextualizedQuery<Users_aggregate> Users_aggregate(List<Users_select_column> distinct_on, int? limit, int? offset, List<Users_order_by> order_by, Users_bool_exp where)
        {
            var parameterValues = new object[] { distinct_on, limit, offset, order_by, where };
            return (ContextualizedQuery<Users_aggregate>)BuildItemQuery<Users_aggregate>(parameterValues, "users_aggregate");
        }

        public ContextualizedQuery<Users> Users_by_pk(Guid id)
        {
            var parameterValues = new object[] { id };
            return (ContextualizedQuery<Users>)BuildItemQuery<Users>(parameterValues, "users_by_pk");
        }

        public ContextualizedQuery<Capsule> Capsules(CapsulesFind find, int? limit, int? offset, string order, string sort)
        {
            var parameterValues = new object[] { find, limit, offset, order, sort };
            return (ContextualizedQuery<Capsule>)BuildCollectionQuery<Capsule>(parameterValues, "capsules");
        }

        public ContextualizedQuery<Capsule> CapsulesPast(CapsulesFind find, int? limit, int? offset, string order, string sort)
        {
            var parameterValues = new object[] { find, limit, offset, order, sort };
            return (ContextualizedQuery<Capsule>)BuildCollectionQuery<Capsule>(parameterValues, "capsulesPast");
        }

        public ContextualizedQuery<Capsule> CapsulesUpcoming(CapsulesFind find, int? limit, int? offset, string order, string sort)
        {
            var parameterValues = new object[] { find, limit, offset, order, sort };
            return (ContextualizedQuery<Capsule>)BuildCollectionQuery<Capsule>(parameterValues, "capsulesUpcoming");
        }

        public ContextualizedQuery<Capsule> Capsule(string id)
        {
            var parameterValues = new object[] { id };
            return (ContextualizedQuery<Capsule>)BuildItemQuery<Capsule>(parameterValues, "capsule");
        }

        public ContextualizedQuery<Info> Company()
        {
            var parameterValues = new object[] { };
            return (ContextualizedQuery<Info>)BuildItemQuery<Info>(parameterValues, "company");
        }

        public ContextualizedQuery<Core> Cores(CoresFind find, int? limit, int? offset, string order, string sort)
        {
            var parameterValues = new object[] { find, limit, offset, order, sort };
            return (ContextualizedQuery<Core>)BuildCollectionQuery<Core>(parameterValues, "cores");
        }

        public ContextualizedQuery<Core> CoresPast(CoresFind find, int? limit, int? offset, string order, string sort)
        {
            var parameterValues = new object[] { find, limit, offset, order, sort };
            return (ContextualizedQuery<Core>)BuildCollectionQuery<Core>(parameterValues, "coresPast");
        }

        public ContextualizedQuery<Core> CoresUpcoming(CoresFind find, int? limit, int? offset, string order, string sort)
        {
            var parameterValues = new object[] { find, limit, offset, order, sort };
            return (ContextualizedQuery<Core>)BuildCollectionQuery<Core>(parameterValues, "coresUpcoming");
        }

        public ContextualizedQuery<Core> Core(string id)
        {
            var parameterValues = new object[] { id };
            return (ContextualizedQuery<Core>)BuildItemQuery<Core>(parameterValues, "core");
        }

        public ContextualizedQuery<Dragon> Dragons(int? limit, int? offset)
        {
            var parameterValues = new object[] { limit, offset };
            return (ContextualizedQuery<Dragon>)BuildCollectionQuery<Dragon>(parameterValues, "dragons");
        }

        public ContextualizedQuery<Dragon> Dragon(string id)
        {
            var parameterValues = new object[] { id };
            return (ContextualizedQuery<Dragon>)BuildItemQuery<Dragon>(parameterValues, "dragon");
        }

        public ContextualizedQuery<History> Histories(HistoryFind find, int? limit, int? offset, string order, string sort)
        {
            var parameterValues = new object[] { find, limit, offset, order, sort };
            return (ContextualizedQuery<History>)BuildCollectionQuery<History>(parameterValues, "histories");
        }

        public ContextualizedQuery<HistoriesResult> HistoriesResult(HistoryFind find, int? limit, int? offset, string order, string sort)
        {
            var parameterValues = new object[] { find, limit, offset, order, sort };
            return (ContextualizedQuery<HistoriesResult>)BuildItemQuery<HistoriesResult>(parameterValues, "historiesResult");
        }

        public ContextualizedQuery<History> History(string id)
        {
            var parameterValues = new object[] { id };
            return (ContextualizedQuery<History>)BuildItemQuery<History>(parameterValues, "history");
        }

        public ContextualizedQuery<Landpad> Landpads(int? limit, int? offset)
        {
            var parameterValues = new object[] { limit, offset };
            return (ContextualizedQuery<Landpad>)BuildCollectionQuery<Landpad>(parameterValues, "landpads");
        }

        public ContextualizedQuery<Landpad> Landpad(string id)
        {
            var parameterValues = new object[] { id };
            return (ContextualizedQuery<Landpad>)BuildItemQuery<Landpad>(parameterValues, "landpad");
        }

        public ContextualizedQuery<Launch> Launches(LaunchFind find, int? limit, int? offset, string order, string sort)
        {
            var parameterValues = new object[] { find, limit, offset, order, sort };
            return (ContextualizedQuery<Launch>)BuildCollectionQuery<Launch>(parameterValues, "launches");
        }

        public ContextualizedQuery<Launch> LaunchesPast(LaunchFind find, int? limit, int? offset, string order, string sort)
        {
            var parameterValues = new object[] { find, limit, offset, order, sort };
            return (ContextualizedQuery<Launch>)BuildCollectionQuery<Launch>(parameterValues, "launchesPast");
        }

        public ContextualizedQuery<LaunchesPastResult> LaunchesPastResult(LaunchFind find, int? limit, int? offset, string order, string sort)
        {
            var parameterValues = new object[] { find, limit, offset, order, sort };
            return (ContextualizedQuery<LaunchesPastResult>)BuildItemQuery<LaunchesPastResult>(parameterValues, "launchesPastResult");
        }

        public ContextualizedQuery<Launch> LaunchesUpcoming(LaunchFind find, int? limit, int? offset, string order, string sort)
        {
            var parameterValues = new object[] { find, limit, offset, order, sort };
            return (ContextualizedQuery<Launch>)BuildCollectionQuery<Launch>(parameterValues, "launchesUpcoming");
        }

        public ContextualizedQuery<Launch> Launch(string id)
        {
            var parameterValues = new object[] { id };
            return (ContextualizedQuery<Launch>)BuildItemQuery<Launch>(parameterValues, "launch");
        }

        public ContextualizedQuery<Launch> LaunchLatest(int? offset)
        {
            var parameterValues = new object[] { offset };
            return (ContextualizedQuery<Launch>)BuildItemQuery<Launch>(parameterValues, "launchLatest");
        }

        public ContextualizedQuery<Launch> LaunchNext(int? offset)
        {
            var parameterValues = new object[] { offset };
            return (ContextualizedQuery<Launch>)BuildItemQuery<Launch>(parameterValues, "launchNext");
        }

        public ContextualizedQuery<Launchpad> Launchpads(int? limit, int? offset)
        {
            var parameterValues = new object[] { limit, offset };
            return (ContextualizedQuery<Launchpad>)BuildCollectionQuery<Launchpad>(parameterValues, "launchpads");
        }

        public ContextualizedQuery<Launchpad> Launchpad(string id)
        {
            var parameterValues = new object[] { id };
            return (ContextualizedQuery<Launchpad>)BuildItemQuery<Launchpad>(parameterValues, "launchpad");
        }

        public ContextualizedQuery<Mission> Missions(MissionsFind find, int? limit, int? offset)
        {
            var parameterValues = new object[] { find, limit, offset };
            return (ContextualizedQuery<Mission>)BuildCollectionQuery<Mission>(parameterValues, "missions");
        }

        public ContextualizedQuery<MissionResult> MissionsResult(MissionsFind find, int? limit, int? offset)
        {
            var parameterValues = new object[] { find, limit, offset };
            return (ContextualizedQuery<MissionResult>)BuildItemQuery<MissionResult>(parameterValues, "missionsResult");
        }

        public ContextualizedQuery<Mission> Mission(string id)
        {
            var parameterValues = new object[] { id };
            return (ContextualizedQuery<Mission>)BuildItemQuery<Mission>(parameterValues, "mission");
        }

        public ContextualizedQuery<Payload> Payloads(PayloadsFind find, int? limit, int? offset, string order, string sort)
        {
            var parameterValues = new object[] { find, limit, offset, order, sort };
            return (ContextualizedQuery<Payload>)BuildCollectionQuery<Payload>(parameterValues, "payloads");
        }

        public ContextualizedQuery<Payload> Payload(string id)
        {
            var parameterValues = new object[] { id };
            return (ContextualizedQuery<Payload>)BuildItemQuery<Payload>(parameterValues, "payload");
        }

        public ContextualizedQuery<Roadster> Roadster()
        {
            var parameterValues = new object[] { };
            return (ContextualizedQuery<Roadster>)BuildItemQuery<Roadster>(parameterValues, "roadster");
        }

        public ContextualizedQuery<Rocket> Rockets(int? limit, int? offset)
        {
            var parameterValues = new object[] { limit, offset };
            return (ContextualizedQuery<Rocket>)BuildCollectionQuery<Rocket>(parameterValues, "rockets");
        }

        public ContextualizedQuery<RocketsResult> RocketsResult(int? limit, int? offset)
        {
            var parameterValues = new object[] { limit, offset };
            return (ContextualizedQuery<RocketsResult>)BuildItemQuery<RocketsResult>(parameterValues, "rocketsResult");
        }

        public ContextualizedQuery<Rocket> Rocket(string id)
        {
            var parameterValues = new object[] { id };
            return (ContextualizedQuery<Rocket>)BuildItemQuery<Rocket>(parameterValues, "rocket");
        }

        public ContextualizedQuery<Ship> Ships(ShipsFind find, int? limit, int? offset, string order, string sort)
        {
            var parameterValues = new object[] { find, limit, offset, order, sort };
            return (ContextualizedQuery<Ship>)BuildCollectionQuery<Ship>(parameterValues, "ships");
        }

        public ContextualizedQuery<ShipsResult> ShipsResult(ShipsFind find, int? limit, int? offset, string order, string sort)
        {
            var parameterValues = new object[] { find, limit, offset, order, sort };
            return (ContextualizedQuery<ShipsResult>)BuildItemQuery<ShipsResult>(parameterValues, "shipsResult");
        }

        public ContextualizedQuery<Ship> Ship(string id)
        {
            var parameterValues = new object[] { id };
            return (ContextualizedQuery<Ship>)BuildItemQuery<Ship>(parameterValues, "ship");
        }
    }
}