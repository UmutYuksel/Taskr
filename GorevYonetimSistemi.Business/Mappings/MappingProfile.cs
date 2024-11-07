using AutoMapper;
using GorevYonetimSistemi.Business.Dtos.Duty;
using GorevYonetimSistemi.Business.Dtos.User;
using GorevYonetimSistemi.Data.Entities;

namespace GorevYonetimSistemi.Business.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User ile UserDto arasındaki dönüşüm
            CreateMap<User, UserDto>().ReverseMap();
            
            // Duty ile DutyDto arasındaki dönüşüm
            CreateMap<Duty, DutyDto>().ReverseMap();
        }
    }
}
