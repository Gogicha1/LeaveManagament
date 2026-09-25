using System.ComponentModel.DataAnnotations.Schema;

namespace LeaveManagament.Data
{
    public class LeaveRequest
    {
        public int Id { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }

        public int LeaveTypeId { get; set; }
        public LeaveType LeaveType { get; set; } = null!;

        public string RequestingEmployeeId { get; set; } = string.Empty;
        public ApplicationUser RequestingEmployee { get; set; } = null!;

        [Column(TypeName = "nvarchar(250)")]
        public string? RequestComments { get; set; }
        public DateTime DateRequested { get; set; }

        public LeaveRequestStatus Status { get; set; }

        // Filled in when a supervisor approves or rejects the request.
        public string? ReviewerId { get; set; }
        public ApplicationUser? Reviewer { get; set; }
        [Column(TypeName = "nvarchar(250)")]
        public string? ReviewComments { get; set; }
        public DateTime? DateReviewed { get; set; }

        // Both dates are inclusive, so a one day leave counts as one day.
        [NotMapped]
        public int NumberOfDays => EndDate.DayNumber - StartDate.DayNumber + 1;
    }
}
