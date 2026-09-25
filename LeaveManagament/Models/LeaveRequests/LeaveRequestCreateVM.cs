using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LeaveManagament.Models.LeaveRequests
{
    public class LeaveRequestCreateVM : IValidatableObject
    {
        [Required]
        [Display(Name = "Leave Type")]
        public int LeaveTypeId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateOnly StartDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateOnly EndDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);

        [MaxLength(250, ErrorMessage = "Comments cannot exceed 250 characters.")]
        [Display(Name = "Comments")]
        public string? RequestComments { get; set; }

        public SelectList? LeaveTypes { get; set; }

        [Display(Name = "Days Requested")]
        public int NumberOfDays => EndDate.DayNumber - StartDate.DayNumber + 1;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (EndDate < StartDate)
            {
                yield return new ValidationResult(
                    "The end date cannot be earlier than the start date.", [nameof(EndDate)]);
            }

            if (StartDate < DateOnly.FromDateTime(DateTime.Today))
            {
                yield return new ValidationResult(
                    "Leave cannot be requested for a date in the past.", [nameof(StartDate)]);
            }
        }
    }
}
