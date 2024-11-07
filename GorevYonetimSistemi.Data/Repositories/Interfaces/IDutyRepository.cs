using GorevYonetimSistemi.Data.Entities;

namespace GorevYonetimSistemi.Data.Repositories.Interfaces
{
    public interface IDutyRepository
    {
        Task<Duty> GetDutyByIdAsync(Guid dutyId);
        Task<IEnumerable<Duty>> GetAllDutiesAsync();
        Task CreateDutyAsync(Duty duty);
        Task UpdateDutyAsync(Duty duty);
        Task DeleteDutyAsync(Guid dutyId);
    }
}