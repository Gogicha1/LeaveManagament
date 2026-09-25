using System.ComponentModel.DataAnnotations;
using LeaveManagament.Data;

namespace LeaveManagament.Models.LeaveRequests
{
    public class LeaveRequestReadOnlyVM
    {
        public int Id { get; set; }

        [Display(Name = "Leave Type")]
        public string LeaveTypeName { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateOnly StartDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateOnly EndDate { get; set; }

        [Display(Name = "Days")]
        public int NumberOfDays { get; set; }

        [Display(Name = "Requested On")]
        public DateTime DateRequested { get; set; }

        public LeaveRequestStatus Status { get; set; }

        [Display(Name = "Comments")]
        public string? RequestComments { get; set; }

        [Display(Name = "Reviewer Comments")]
        public string? ReviewComments { get; set; }

        // Only a request nobody has reviewed yet can be withdrawn by the employee.
        public bool CanBeCanceled => Status == LeaveRequestStatus.Pending;
    }
}
