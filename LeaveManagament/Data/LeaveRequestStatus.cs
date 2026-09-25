using System.ComponentModel.DataAnnotations;

namespace LeaveManagament.Data
{
    public enum LeaveRequestStatus
    {
        Pending = 1,
        Approved = 2,
        [Display(Name = "Rejected")]
        Declined = 3,
        [Display(Name = "Cancelled")]
        Canceled = 4
    }
}
