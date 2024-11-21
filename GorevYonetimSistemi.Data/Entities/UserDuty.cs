using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using GorevYonetimSistemi.Core.Enums;

namespace GorevYonetimSistemi.Data.Entities
{
    public class UserDuty
    {
        [Key]
        public Guid UserDutyId { get; set; }
        public Guid UserId { get; set; }
        public User? User { get; set; }
        public Guid DutyId { get; set; }
        public Duty? Duty { get; set; }
    }
}
