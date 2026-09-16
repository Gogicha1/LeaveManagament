using LeaveManagament.Models.LeaveTypes;

namespace LeaveManagament.Services
{
    public interface ILeaveTypeServices
    {
        Task<bool> CheckIfLeaveTypeNameExists(string name, int? excludeId = null);
        Task Create(LeaveTypeCreateVM leaveTypeCreateVM);
        Task Edit(int id, LeaveTypeEditVM leaveTypeEditVM);
        Task<List<LeaveTypeReadOnlyVM>> GetAllLeaveTypesAsync();
        Task<LeaveTypeReadOnlyVM?> GetLeaveTypeByIdAsync(int id);
        Task<LeaveTypeEditVM?> GetLeaveTypeForEditAsync(int id);
        bool LeaveTypeExists(int id);
        Task Remove(int id);
    }
}