using GorevYonetimSistemi.Business.Dtos;
using GorevYonetimSistemi.Business.Dtos.User;
using GorevYonetimSistemi.Core.Enums;

namespace GorevYonetimSistemi.Web.ViewModels
{
    public class UserDutyViewModel
    {
        public Guid UserDutyId { get; set; }
        public Guid DutyId { get; set; }
        public string? DutyTitle { get; set; }
        public string? DutyDescription { get; set; } 
        public ProgressEnum DutyProgress { get; set; }
        public string? DutyProgressDisplay { get; set; }
        public DateTime DutyCreatedDate { get; set; }
        public List<UserDto> Users { get; set; } = new();
    }
}