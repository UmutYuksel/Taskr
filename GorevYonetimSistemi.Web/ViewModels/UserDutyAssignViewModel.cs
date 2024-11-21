using GorevYonetimSistemi.Business.Dtos;
using GorevYonetimSistemi.Business.Dtos.Duty;
using GorevYonetimSistemi.Business.Dtos.User;
using GorevYonetimSistemi.Business.Dtos.UserDuty;

namespace GorevYonetimSistemi.Web.ViewModels
{
    public class UserDutyAssignViewModel
    {
        public List<UserDto> Users { get; set; }
        public List<DutyDto> Duties { get; set; }
        public UserDutyDto UserDutyDto { get; set; }
    }
}