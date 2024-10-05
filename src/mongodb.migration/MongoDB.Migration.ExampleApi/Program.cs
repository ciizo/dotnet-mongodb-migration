using MongoDB.Migration.ExampleApi;
using MongoDB.Migration.ExampleApi.Migration;

try
{
    IConfiguration configuration = GetConfiguration();

    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddServices(configuration);

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    await RunDatabaseMigrations(app);

    app.Run();

}
catch (Exception ex)
{
    // log
}
finally
{
    // log
}

IConfiguration GetConfiguration()
{
    string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? Environments.Production;
    IConfigurationBuilder builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true)
        .AddEnvironmentVariables();

    return builder.Build();
}


async Task RunDatabaseMigrations(IHost host)
{
    using IServiceScope scope = host.Services.CreateScope();
    IServiceProvider services = scope.ServiceProvider;
    var migrationRunner = services.GetRequiredService<MigrationRunner>();

    await migrationRunner.RunMigrations();
}
