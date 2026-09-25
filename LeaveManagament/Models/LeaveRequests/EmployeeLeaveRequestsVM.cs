namespace LeaveManagament.Models.LeaveRequests
{
    public class EmployeeLeaveRequestsVM
    {
        public List<LeaveTypeBalanceVM> Balances { get; set; } = [];
        public List<LeaveRequestReadOnlyVM> Requests { get; set; } = [];
    }
}
