using System.ComponentModel.DataAnnotations;

namespace LeaveManagament.Models.LeaveRequests
{
    public class LeaveTypeBalanceVM
    {
        public int LeaveTypeId { get; set; }

        [Display(Name = "Leave Type")]
        public string LeaveTypeName { get; set; } = string.Empty;

        [Display(Name = "Allowed Days")]
        public int AllowedDays { get; set; }

        [Display(Name = "Requested / Approved Days")]
        public int UsedDays { get; set; }

        [Display(Name = "Remaining Days")]
        public int RemainingDays => AllowedDays - UsedDays;
    }
}
