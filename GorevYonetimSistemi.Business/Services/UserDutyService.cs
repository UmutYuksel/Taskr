using AutoMapper;
using GorevYonetimSistemi.Business.Dtos.Duty;
using GorevYonetimSistemi.Business.Dtos.User;
using GorevYonetimSistemi.Business.Services.Interfaces;
using GorevYonetimSistemi.Data.Entities;
using GorevYonetimSistemi.Data.Repositories.Interfaces;

namespace GorevYonetimSistemi.Business.Services
{
    public class UserDutyService : IUserDutyService
    {
        private readonly IUserDutyRepository _userDutyRepository;
        private readonly IMapper _mapper;

        public UserDutyService
        (
            IUserDutyRepository userDutyRepository,
            IMapper mapper
        )
        {
            _userDutyRepository = userDutyRepository;
            _mapper = mapper;
        }

        public async Task RemoveUserFromDutyAsync(Guid dutyId, Guid userId)
        {
            await _userDutyRepository.RemoveUserFromDutyAsync(dutyId, userId);
        }

        public async Task<IEnumerable<UserDto>> GetUsersByDutyIdAsync(Guid dutyId)
        {
            var users = await _userDutyRepository.GetUsersByDutyIdAsync(dutyId);
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<IEnumerable<DutyDto>> GetDutiesByUserIdAsync(Guid userId)
        {
            var duties = await _userDutyRepository.GetDutiesByUserIdAsync(userId);

            if (duties == null)
            {
                throw new KeyNotFoundException("Duties not found.");
            }

            return _mapper.Map<IEnumerable<DutyDto>>(duties);
        }

        public async Task AssignUserToDutyAsync(Guid userId, Guid dutyId)
        {
            await _userDutyRepository.AssignUserToDutyAsync(dutyId, userId);
        }

        public async Task DeleteUserAndRelatedDutiesAsync(Guid userId)
        {
            await _userDutyRepository.DeleteUserAndRelatedDutiesAsync(userId);
        }

        public async Task<List<UserDuty>> GetAllUserDutiesAsync()
        {
            var userDuties = await _userDutyRepository.GetAllAsync();

            if (userDuties == null || !userDuties.Any())
            {
                throw new KeyNotFoundException("No UserDuties found.");
            }

            return userDuties;
        }

        public async Task<List<UserDuty>> GetUserDutiesByUserIdAsync(Guid userId)
        {
            var userDuties = await _userDutyRepository.GetByUserIdAsync(userId);

            if (userDuties == null || !userDuties.Any())
            {
                throw new KeyNotFoundException($"No UserDuties found for UserId {userId}");
            }

            return userDuties;
        }
    }
}