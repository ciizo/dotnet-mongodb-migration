using MongoDB.Bson.Serialization.Conventions;

namespace MongoDB.Migration.ExampleApi.Configs;

public class MongoDbConfig
{
    public const string Name = "Database";
    public string ConnectionString { get; init; }
    public string DatabaseName { get; init; }

    public static void IgnoreExtraElements()
    {
        var pack = new ConventionPack { new IgnoreExtraElementsConvention(true) };
        ConventionRegistry.Register("IgnoreExtraElements", pack, t => true);
    }

    public static class UserCollection
    {
        public const string Name = "Users";
        public static class Indexes
        {
            public const string UserName = "UserName";
        }
    }
}

