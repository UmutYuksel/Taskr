using GorevYonetimSistemi.Data.Entities;

namespace GorevYonetimSistemi.Data.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(Guid userId);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task CreateUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(Guid userId);
        Task<User> GetUserByEmail(string email);
    }
}