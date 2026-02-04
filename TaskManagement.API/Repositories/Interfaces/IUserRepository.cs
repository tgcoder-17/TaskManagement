using TaskManagement.API.Models;

namespace TaskManagement.API.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<bool> EmailExistsAsync(string email);
        Task<bool> UserExistsAsync(Guid userId);
        Task AddAsync(User user);
    }
}
