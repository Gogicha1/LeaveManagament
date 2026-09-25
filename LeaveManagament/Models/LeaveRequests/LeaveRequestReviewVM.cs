using System.ComponentModel.DataAnnotations;

namespace LeaveManagament.Models.LeaveRequests
{
    public class LeaveRequestReviewVM : LeaveRequestReadOnlyVM
    {
        [Display(Name = "Employee")]
        public string EmployeeName { get; set; } = string.Empty;

        [Display(Name = "Email")]
        public string? EmployeeEmail { get; set; }

        [Display(Name = "Reviewed By")]
        public string? ReviewerName { get; set; }
    }
}
