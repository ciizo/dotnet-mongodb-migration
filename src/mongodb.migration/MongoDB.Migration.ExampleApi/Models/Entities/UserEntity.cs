using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Migration.ExampleApi.Models.Constants;
using MongoDB.Migration.ExampleApi.Models.Domains;

namespace MongoDB.Migration.ExampleApi.Models.Entities;

public class UserEntity
{
    private const int _latestVersion = 1;
    public required int? Version { get; init; }

    [BsonId]
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required string Type { get; init; }
    public DateTime CreatedAt { get; init; }

    public bool ShouldUpgradeVersion() => Version != _latestVersion;

    public static User? ToModel(UserEntity User)
    {
        if (User == null)
            return null;

        return new User
        {
            Id = User.Id,
            Name = User.Name,
            Type = Enum.Parse<UserType>(User.Type)
        };
    }

    public static UserEntity? ToEntity(User User)
    {
        if (User == null)
            return null;

        return new UserEntity
        {
            Version = _latestVersion,
            Id = User.Id,
            Name = User.Name,
            Type = User.Type.ToString()
        };
    }

}
