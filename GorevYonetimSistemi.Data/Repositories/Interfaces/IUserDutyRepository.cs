using GorevYonetimSistemi.Data.Entities;

namespace GorevYonetimSistemi.Data.Repositories.Interfaces
{
    public interface IUserDutyRepository
    {
        Task<IEnumerable<Duty>> GetDutiesByUserIdAsync(Guid userId);
        Task DeleteUserAndRelatedDutiesAsync(Guid userId);
        Task AssignUserToDutyAsync(Guid dutyId, Guid userId);
        Task RemoveUserFromDutyAsync(Guid dutyId, Guid userId);
        Task<IEnumerable<User>> GetUsersByDutyIdAsync(Guid dutyId);
        Task<List<UserDuty>> GetAllAsync();
        Task<List<UserDuty>> GetByUserIdAsync(Guid userId);
    }
}