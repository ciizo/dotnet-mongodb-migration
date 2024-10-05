using MongoDB.Migration.ExampleApi.Models.Constants;

namespace MongoDB.Migration.ExampleApi.Models.Domains;

public class User
{
    public string Id { get; init; }
    public string Name { get; init; }
    public UserType Type { get; init; }
    public DateTime CreatedAt { get; init; }
}
