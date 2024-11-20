using System.ComponentModel.DataAnnotations;

namespace GorevYonetimSistemi.Core.Enums
{
    public enum ProgressEnum
    {
        [Display(Name = "Not Started")]
        NotStarted,

        [Display(Name = "In Progress")]
        InProgress,

        [Display(Name = "Completed")]
        Completed,

        [Display(Name = "On Hold")]
        OnHold,

        [Display(Name = "Assignment Awaiting")]
        Assignment_Awaiting,

        [Display(Name = "Awaiting Approval")]
        Awaiting_Approval
    }
}