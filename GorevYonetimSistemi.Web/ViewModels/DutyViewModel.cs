

using GorevYonetimSistemi.Core.Enums;

namespace GorevYonetimSistemi.Web.ViewModels
{
    public class DutyViewModel
{
    public Guid DutyId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public ProgressEnum Progress { get; set; } // Enum
    public string? ProgressDisplayName { get; set; } // Enum'un Display Name'i
    public DateTime CreatedDate { get; set; }
}

}