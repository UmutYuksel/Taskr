using GorevYonetimSistemi.Business.Dtos.User;
using GorevYonetimSistemi.Core.Enums;

namespace GorevYonetimSistemi.Business.Dtos.UserDuty
{
    public class UserDutyDto
    {
        public Guid UserDutyId { get; set; }
        public Guid DutyId { get; set; }
        public Guid UserId { get; set; }
        public string? DutyTitle { get; set; }
        public string? DutyDescription { get; set; } 
        public ProgressEnum DutyProgress { get; set; }
        public DateTime DutyCreatedDate { get; set; }
        public List<UserDto> Users { get; set; } = new();
    }
}