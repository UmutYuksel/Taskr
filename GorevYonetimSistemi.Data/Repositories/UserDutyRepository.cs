using GorevYonetimSistemi.Data.Entities;
using GorevYonetimSistemi.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GorevYonetimSistemi.Data.Repositories
{
    public class UserDutyRepository : IUserDutyRepository
    {
        private readonly DataContext _context;
        private readonly IUserRepository _userRepository;
        private readonly IDutyRepository _dutyRepository;

        public UserDutyRepository
        (
            DataContext context,
            IUserRepository userRepository,
            IDutyRepository dutyRepository
        )
        {
            _context = context;
            _userRepository = userRepository;
            _dutyRepository = dutyRepository;
        }
        public async Task<IEnumerable<Duty>> GetDutiesByUserIdAsync(Guid userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            // Null olmayan görevleri filtrele
            return user.UserDuties
                       .Select(ud => ud.Duty)
                       .Where(duty => duty != null)
                       .Cast<Duty>() // Nullable olmayan türe dönüştür.
                       .ToList();
        }

        public async Task DeleteUserAndRelatedDutiesAsync(Guid userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            foreach (var userDuty in user.UserDuties.ToList())
            {
                if (userDuty.Duty != null && userDuty.Duty.UserDuties.Count == 1)
                {
                    _context.Duties.Remove(userDuty.Duty);
                }
                _context.UserDuties.Remove(userDuty);
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task AssignUserToDutyAsync(Guid dutyId, Guid userId)
        {
            var duty = await _dutyRepository.GetDutyByIdAsync(dutyId);
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }
            var userDuty = new UserDuty
            {
                UserDutyId = Guid.NewGuid(),
                DutyId = dutyId,
                UserId = userId
            };
            duty.UserDuties.Add(userDuty);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveUserFromDutyAsync(Guid dutyId, Guid userId)
        {
            // Verilen DutyId ve UserId'ye sahip UserDuty'yi buluyoruz
            var userDuty = await _context.UserDuties
                                          .FirstOrDefaultAsync(ud => ud.DutyId == dutyId && ud.UserId == userId);

            if (userDuty == null)
            {
                // Kullanıcı ile görev arasındaki ilişki bulunamazsa hata fırlatıyoruz
                throw new KeyNotFoundException("Relation between User and Duty not found.");
            }

            // İlgili görevi ve görevle ilişkili kullanıcıları alıyoruz
            var duty = await _context.Duties.Include(d => d.UserDuties)
                                             .FirstOrDefaultAsync(d => d.DutyId == dutyId);

            if (duty == null)
            {
                // Görev bulunamazsa hata fırlatıyoruz
                throw new KeyNotFoundException("Duty not found.");
            }

            // Eğer görevde sadece 1 kullanıcı varsa, UserDuty kaydını tamamen sileriz
            if (duty.UserDuties.Count == 1)
            {
                _context.UserDuties.Remove(userDuty);  // Bu kullanıcının görevle olan ilişkisini tamamen sil
            }
            else
            {
                _context.UserDuties.Remove(userDuty);  // Sadece bu kullanıcının görevle olan ilişkisini sil
            }

            await _context.SaveChangesAsync();  // Değişiklikleri kaydediyoruz
        }



        public async Task<IEnumerable<User>> GetUsersByDutyIdAsync(Guid dutyId)
        {
            var duty = await _dutyRepository.GetDutyByIdAsync(dutyId);

            if (duty == null)
            {
                throw new KeyNotFoundException("Duty not found.");
            }
            return duty.UserDuties.Select(ud => ud.User).ToList()!;
        }

        public async Task<List<UserDuty>> GetAllAsync()
        {
            return await _context.UserDuties
                                 .Include(ud => ud.User)
                                 .Include(ud => ud.Duty)
                                 .ToListAsync();
        }

        public async Task<List<UserDuty>> GetByUserIdAsync(Guid userId)
        {
            return await _context.UserDuties
                                 .Include(ud => ud.Duty)
                                 .Where(ud => ud.UserId == userId)
                                 .ToListAsync();
        }
    }
}