using System.ComponentModel.DataAnnotations;

namespace LeaveManagament.Models.LeaveTypes

{
    public class LeaveTypeCreateVM
    {
        [Required]
        [Length(2, 50, ErrorMessage = "Name must contain between 2 and 50 characters.")]
        public string Name { get; set; } = string.Empty;
        [Required]
        [Range(1, 50, ErrorMessage = "Number of days must be between 1 and 50.")]
        [Display(Name = "Maximum Number of Days")]
        public int NumberOfDays { get; set; }
    }
}