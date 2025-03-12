using cash.hub.authentication.api.Adapters.Outbound.Repository.DataBaseContextConfiguration;
using cash.hub.authentication.api.Domain.Entities;
using cash.hub.authentication.api.Domain.Ports;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace cash.hub.authentication.api.Adapters.Outbound.Repository;

public class UserRepository(DataBaseContext context, ILogger<UserRepository> logger) : IUserRepository
{
    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task AddUserAsync(User user)
    {
        context.Users.Add(user);
        await context.SaveChangesAsync();
    }

    public async Task<bool> UserExistsAsync(string username)
    {
        return await context.Users.AnyAsync(u => u.Username == username);
    }
}