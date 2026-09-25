using LeaveManagament.Models.LeaveRequests;

namespace LeaveManagament.Services
{
    public interface ILeaveRequestServices
    {
        Task<LeaveRequestCreateVM> GetCreateModelAsync();
        Task PopulateLeaveTypesAsync(LeaveRequestCreateVM leaveRequestCreateVM);
        Task CreateLeaveRequestAsync(LeaveRequestCreateVM leaveRequestCreateVM);
        Task<EmployeeLeaveRequestsVM> GetMyLeaveRequestsAsync();
        Task<List<LeaveRequestReviewVM>> GetAllLeaveRequestsAsync();
        Task<LeaveRequestReviewVM?> GetLeaveRequestForReviewAsync(int id);
        Task<bool> ReviewLeaveRequestAsync(int id, bool approved, string? reviewComments);
        Task<bool> CancelMyLeaveRequestAsync(int id);
        Task<bool> LeaveTypeExistsAsync(int leaveTypeId);
        Task<int> GetRemainingDaysAsync(int leaveTypeId);
    }
}
