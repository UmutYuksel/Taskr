using System.ComponentModel.DataAnnotations;
using GorevYonetimSistemi.Core.Enums;

namespace GorevYonetimSistemi.Business.Dtos.Duty
{

    public class DutyDto
    {
        [Required(ErrorMessage = "DutyId is required.")]
        public Guid DutyId { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(255, ErrorMessage = "Title cannot be longer than 255 characters.")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(2000, ErrorMessage = "Description cannot be longer than 2000 characters.")]
        public string? Description { get; set; }

        public ProgressEnum Progress { get; set; }

        [Required(ErrorMessage = "Created date is required.")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid date format.")]
        public DateTime CreatedDate { get; set; }
    }

}