using GorevYonetimSistemi.Data.Entities;
using GorevYonetimSistemi.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GorevYonetimSistemi.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByIdAsync(Guid userId)
        {
            var user = await _context.Users
                                     .Include(u => u.UserDuties)
                                     .ThenInclude(ud => ud.Duty)
                                     .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            return user;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users
                                 .Include(u => u.UserDuties)
                                 .ThenInclude(ud => ud.Duty)
                                 .ToListAsync();
        }

        public async Task CreateUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(Guid userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(i => i.UserId == userId);

            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetUserByEmail(string email, bool throwIfNotFound = true)
        {
            var user = await _context.Users.FirstOrDefaultAsync(e => e.Email == email);

            if (user == null && throwIfNotFound)
            {
                throw new KeyNotFoundException("User not found.");
            }

            return user;
        }
    }
}