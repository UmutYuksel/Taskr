using GorevYonetimSistemi.Business.Dtos.Duty;
using GorevYonetimSistemi.Business.Dtos.User;
using GorevYonetimSistemi.Data.Entities;

namespace GorevYonetimSistemi.Business.Services.Interfaces
{
    public interface IUserDutyService
    {
        Task<IEnumerable<DutyDto>> GetDutiesByUserIdAsync(Guid userId);
        Task AssignUserToDutyAsync(Guid userId, Guid dutyId);
        Task RemoveUserFromDutyAsync(Guid userId, Guid dutyId);
        Task<IEnumerable<UserDto>> GetUsersByDutyIdAsync(Guid dutyId);
        Task DeleteUserAndRelatedDutiesAsync(Guid userId);
        Task<List<UserDuty>> GetAllUserDutiesAsync();
        Task<List<UserDuty>> GetUserDutiesByUserIdAsync(Guid userId);
    }
}