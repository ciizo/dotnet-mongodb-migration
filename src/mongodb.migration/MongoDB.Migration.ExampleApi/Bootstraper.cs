using MongoDB.Driver;
using MongoDB.Migration.ExampleApi.Configs;
using MongoDB.Migration.ExampleApi.Migration;

namespace MongoDB.Migration.ExampleApi
{
    public static class Bootstraper
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoDbConfig>(configuration.GetSection(MongoDbConfig.Name));

            MongoDbConfig.IgnoreExtraElements();
            services.AddSingleton<IMongoClient, MongoClient>(serviceProvider =>
            {
                var mongoConfig = configuration.GetSection(MongoDbConfig.Name).Get<MongoDbConfig>();
                var settings = MongoClientSettings.FromUrl(new MongoUrl(mongoConfig.ConnectionString));
                return new MongoClient(settings);
            });

            services.AddTransient<MigrationRunner>();

            return services;
        }
    }
}
