using cash.hub.authentication.api.Domain.Entities;

namespace cash.hub.authentication.api.Domain.Ports;

public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username);
    Task AddUserAsync(User user);
    Task<bool> UserExistsAsync(string username);
}