using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GorevYonetimSistemi.Data.Entities
{
    public class UserDuty
    {
        [Key]
        public Guid UserDutyId { get; set; }

        public Guid UserId { get; set; }

        [JsonIgnore]
        public User? User { get; set; }

        public Guid DutyId { get; set; }

        [JsonIgnore]
        public Duty? Duty { get; set; }
    }
}
