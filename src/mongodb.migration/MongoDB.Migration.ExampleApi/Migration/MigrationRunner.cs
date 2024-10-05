using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Migration.ExampleApi.Configs;

namespace MongoDB.Migration.ExampleApi.Migration;

public class MigrationRunner
{
    private MongoDbConfig _config;
    private IMongoClient _client;

    public MigrationRunner(IMongoClient client, IOptions<MongoDbConfig> options)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _config = options.Value ?? throw new ArgumentNullException(nameof(options));
    }

    public async Task RunMigrations()
    {
        var database = _client.GetDatabase(_config.DatabaseName);

        await Migrate_20240901_1(database);
    }

    private async Task Migrate_20240901_1(IMongoDatabase database)
    {
        await UserMigration.ExampleMigrateToV0(database);
    }
}