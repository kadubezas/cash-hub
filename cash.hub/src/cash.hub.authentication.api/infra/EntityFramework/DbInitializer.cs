using cash.hub.authentication.api.Adapters.Outbound.Repository.DataBaseContext;
using cash.hub.authentication.api.Domain.Entities;

namespace cash.hub.authentication.api.infra.EntityFramework;

public static class DbInitializer
{
    public static void Seed(DataBaseContext context)
    {
        if (!context.Users.Any())
        {
            var users = new List<User>
            {
                new User { Username = "admin", PasswordHash = "hash1", Salt = "salt1", Role = "Admin" },
                new User { Username = "user1", PasswordHash = "hash2", Salt = "salt2", Role = "User" },
                new User { Username = "user2", PasswordHash = "hash3", Salt = "salt3", Role = "User" },
                new User { Username = "user3", PasswordHash = "hash4", Salt = "salt4", Role = "User" },
                new User { Username = "user4", PasswordHash = "hash5", Salt = "salt5", Role = "User" },
                new User { Username = "user5", PasswordHash = "hash6", Salt = "salt6", Role = "User" }
            };

            context.Users.AddRange(users);
            context.SaveChanges();
        }
    }
}