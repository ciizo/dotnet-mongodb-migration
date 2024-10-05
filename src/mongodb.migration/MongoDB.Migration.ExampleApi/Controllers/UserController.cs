using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Migration.ExampleApi.Configs;
using MongoDB.Migration.ExampleApi.Migration;
using MongoDB.Migration.ExampleApi.Models.Domains;
using MongoDB.Migration.ExampleApi.Models.Entities;

namespace MongoDB.Migration.ExampleApi.Controllers
{
    [ApiController]
    [Route("/users")]
    public class UserController : ControllerBase
    {
        private MongoDbConfig _config;
        private IMongoClient _client;
        private readonly ILogger<UserController> _logger;

        public UserController(IMongoClient client, IOptions<MongoDbConfig> options, ILogger<UserController> logger)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _config = options.Value ?? throw new ArgumentNullException(nameof(options));

            _logger = logger;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUser([FromRoute] string id)
        {

            var db = _client.GetDatabase(_config.DatabaseName);
            var query = db.GetCollection<UserEntity>(MongoDbConfig.UserCollection.Name).AsQueryable();

            var data = query.Where(x => x.Id == id).FirstOrDefault();
            var user = UserEntity.ToModel(data);

            if (user == null)
                return NotFound();

            if (data.ShouldUpgradeVersion())
                await UserMigration.ExampleMigrateToV1(db, MigrationRunOn.ReadData, [user.Id]);

            return Ok(user);
        }
    }
}
