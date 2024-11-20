using AutoMapper;
using GorevYonetimSistemi.Business.Dtos.Duty;
using GorevYonetimSistemi.Business.Dtos.User;
using GorevYonetimSistemi.Business.Services.Interfaces;
using GorevYonetimSistemi.Core.Enums;
using GorevYonetimSistemi.Data.Entities;
using GorevYonetimSistemi.Data.Repositories.Interfaces;

namespace GorevYonetimSistemi.Business.Services
{
    public class DutyService : IDutyService
    {
        private readonly IDutyRepository _dutyRepository;
        private readonly IMapper _mapper;

        public DutyService
        (
            IDutyRepository dutyRepository,
            IMapper mapper
        )
        {
            _dutyRepository = dutyRepository;
            _mapper = mapper;
        }

        public async Task<DutyDto> GetDutyByIdAsync(Guid dutyId)
        {
            var duty = await _dutyRepository.GetDutyByIdAsync(dutyId);

            if (duty == null)
            {
                throw new KeyNotFoundException("Duty not found.");
            }

            return _mapper.Map<DutyDto>(duty);
        }

        public async Task<IEnumerable<DutyDto>> GetAllDutiesAsync()
        {
            var duty = await _dutyRepository.GetAllDutiesAsync();
            return _mapper.Map<IEnumerable<DutyDto>>(duty);
        }

        public async Task<DutyDto> CreateDutyAsync(DutyDto dutyDto)
        {
            var duty = new Duty
            {
                DutyId = Guid.NewGuid(),
                Title = dutyDto.Title,
                Description = dutyDto.Description,
                Progress = ProgressEnum.Assignment_Awaiting,
                CreatedDate = DateTime.Now, 
            };
            
            await _dutyRepository.CreateDutyAsync(duty);
            return _mapper.Map<DutyDto>(duty);
        }

        public async Task<DutyDto> UpdateDutyAsync(Guid dutyId, DutyDto dutyDto)
        {
            var duty = await _dutyRepository.GetDutyByIdAsync(dutyId);

            if (duty == null)
            {
                throw new KeyNotFoundException("Duty not found.");
            }

            _mapper.Map(dutyDto, duty);
            await _dutyRepository.UpdateDutyAsync(duty);
            return _mapper.Map<DutyDto>(duty);
        }

        public async Task DeleteDutyAsync(Guid dutyId)
        {
            await _dutyRepository.DeleteDutyAsync(dutyId);
        }
    }
}