using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Migration.ExampleApi.Configs;

namespace MongoDB.Migration.ExampleApi.Migration;

public static class UserMigration
{
    public static readonly Dictionary<int, Func<IMongoDatabase, MigrationRunOn, IEnumerable<string>, Task>> OnFlyMigrationSteps = new()
    {
        { 0, ExampleMigrateToV1 }
    };

    public static async Task MigrateToLatest(IMongoDatabase database)
    {
        await ExampleMigrateToV0(database);
        await ExampleMigrateToV1(database, MigrationRunOn.AppStart);
    }

    public static async Task ExampleMigrateToV0(IMongoDatabase database)
    {
        if (!MigrationConfig.ShouldRunOnAppStart())
            return;

        var col = database.GetCollection<BsonDocument>(MongoDbConfig.UserCollection.Name);
        var filter = Builders<BsonDocument>.Filter.Eq<int?>("Version", null);
        var update = Builders<BsonDocument>.Update
            .Set("Version", 0);

        await col.UpdateManyAsync(filter, update);
    }

    public static async Task ExampleMigrateToV1(IMongoDatabase database, MigrationRunOn runOn)
    {
        var filter = Builders<BsonDocument>.Filter.Eq<int?>("Version", 0);

        await ExampleMigrateToV1(database, runOn, filter);
    }

    public static async Task ExampleMigrateToV1(IMongoDatabase database, MigrationRunOn runOn, IEnumerable<string> ids)
    {
        var filter = Builders<BsonDocument>.Filter.In("_id", ids);
        await ExampleMigrateToV1(database, runOn, filter);
    }

    private static async Task ExampleMigrateToV1(IMongoDatabase database, MigrationRunOn runOn, FilterDefinition<BsonDocument> filter)
    {
        if (!MigrationConfig.ShouldRun(runOn))
            return;

        var col = database.GetCollection<BsonDocument>(MongoDbConfig.UserCollection.Name);
        var update = Builders<BsonDocument>.Update
            .Set("Version", 1);

        await col.UpdateManyAsync(filter, update);
    }
}
