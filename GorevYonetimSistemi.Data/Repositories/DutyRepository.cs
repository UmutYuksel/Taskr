using GorevYonetimSistemi.Data.Entities;
using GorevYonetimSistemi.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GorevYonetimSistemi.Data.Repositories
{
    public class DutyRepositoy : IDutyRepository
    {
        private readonly DataContext _context;

        public DutyRepositoy(DataContext context)
        {
            _context = context;
        }

        public async Task<Duty> GetDutyByIdAsync(Guid dutyId)
        {
            var duty = await _context.Duties.FirstOrDefaultAsync(d => d.DutyId == dutyId);

            if (duty == null)
            {
                throw new KeyNotFoundException("Duty not found.");
            }

            return duty;
        }

        public async Task<IEnumerable<Duty>> GetAllDutiesAsync()
        {
            return await _context.Duties.ToListAsync();
        }

        public async Task CreateDutyAsync(Duty duty)
        {
            await _context.Duties.AddAsync(duty);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDutyAsync(Duty duty)
        {
            _context.Duties.Update(duty);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDutyAsync(Guid dutyId)
        {
            var user = await _context.Duties.FirstOrDefaultAsync(d => d.DutyId == dutyId);
            
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }
            _context.Duties.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}