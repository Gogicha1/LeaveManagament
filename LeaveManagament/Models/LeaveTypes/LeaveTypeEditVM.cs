using System.ComponentModel.DataAnnotations;

namespace LeaveManagament.Models.LeaveTypes

{
    public class LeaveTypeEditVM : BaseLeaveTypeVM
    {
        

        [Required]
        [Length(4, 50, ErrorMessage = "Name cannot exceed 50 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Range(1, 50, ErrorMessage = "Number of days must be between 1 and 50.")]
        [Display(Name = "Maximum Number of Days")]
        public int NumberOfDays { get; set; }
    }
}