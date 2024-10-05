using System.Runtime.CompilerServices;

namespace MongoDB.Migration.ExampleApi.Migration;

public static class MigrationConfig
{
    // TODO disable each function using Enable configuration or MigrationHistory collection.
    private static readonly Dictionary<string, MigrationRunOn[]> _configs = new(StringComparer.OrdinalIgnoreCase)
    {
        {
            "UserMigration.ExampleMigrateToV0",
            new [] { MigrationRunOn.AppStart }
        },
        {
            "UserMigration.ExampleMigrateToV1",
            new [] { MigrationRunOn.ReadData }
        }
    };

    private static string Key(string collectionMigration, string function) => $"{collectionMigration}.{function}";

    public static bool ShouldRun(
        MigrationRunOn runOn,
        [CallerMemberName] string migrationFunc = "",
        [CallerFilePath] string migrationFilePath = "")
    {
        string colMigration = Path.GetFileNameWithoutExtension(migrationFilePath);
        var migrationParts = colMigration.Split('\\');
        colMigration = migrationParts.LastOrDefault() ?? string.Empty;
        var key = Key(colMigration, migrationFunc);
        return _configs[key].Contains(runOn);
    }

    public static bool ShouldRunOnAppStart(
        [CallerMemberName] string migrationFunc = "",
        [CallerFilePath] string migrationFilePath = "")
    {
        string colMigration = Path.GetFileNameWithoutExtension(migrationFilePath);
        var migrationParts = colMigration.Split('\\');
        colMigration = migrationParts.LastOrDefault() ?? string.Empty;
        var key = Key(colMigration, migrationFunc);
        return _configs[key].Contains(MigrationRunOn.AppStart);
    }

    public static bool ShouldRunOnReadData(
        [CallerMemberName] string migrationFunc = "",
        [CallerFilePath] string migrationFilePath = "")
    {
        string colMigration = Path.GetFileNameWithoutExtension(migrationFilePath);
        var migrationParts = colMigration.Split('\\');
        colMigration = migrationParts.LastOrDefault() ?? string.Empty;
        var key = Key(colMigration, migrationFunc);
        return _configs[key].Contains(MigrationRunOn.ReadData);
    }
}
