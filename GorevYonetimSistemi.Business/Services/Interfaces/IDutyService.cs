using GorevYonetimSistemi.Business.Dtos.Duty;

namespace GorevYonetimSistemi.Business.Services.Interfaces
{
    public interface IDutyService
    {
        Task<DutyDto> GetDutyByIdAsync(Guid dutyId);
        Task<IEnumerable<DutyDto>> GetAllDutiesAsync();
        Task<DutyDto> CreateDutyAsync(DutyDto dutyDto);
        Task<DutyDto> UpdateDutyAsync(Guid dutyId,DutyDto dutyDto);
        Task DeleteDutyAsync(Guid dutyId);
    }
}