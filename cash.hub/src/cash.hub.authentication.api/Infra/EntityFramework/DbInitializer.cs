using cash.hub.authentication.api.Adapters.Outbound.Repository.DataBaseContextConfiguration;
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
                new User { Username = "admin", PasswordHash = "eLkqwA2n9bqftJV4eOCUR6vygnaf+2DPhdCGaiNV8PQ=", Salt = "sBuC/E4e2zeCG6Aujm7yhA==", Role = "Admin" }
            };

            context.Users.AddRange(users);
            context.SaveChanges();
        }
    }
}