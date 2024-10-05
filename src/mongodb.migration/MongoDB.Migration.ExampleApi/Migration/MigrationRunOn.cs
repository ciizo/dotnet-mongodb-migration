namespace MongoDB.Migration.ExampleApi.Migration;

public enum MigrationRunOn
{
    AppStart = 1,
    AppStartWithDelay,
    ReadData,
    ManualTrigger,
    ScheduledTime,
}
