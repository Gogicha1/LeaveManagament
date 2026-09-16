using AutoMapper;
using LeaveManagament.Data;
using LeaveManagament.Models.LeaveTypes;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagament.Services
{
    public class LeaveTypeServices : ILeaveTypeServices
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public LeaveTypeServices(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<LeaveTypeReadOnlyVM>> GetAllLeaveTypesAsync()
        {
            var leaveTypes = await _context.LeaveTypes.ToListAsync();
            return _mapper.Map<List<LeaveTypeReadOnlyVM>>(leaveTypes);
        }

        public async Task<LeaveTypeReadOnlyVM?> GetLeaveTypeByIdAsync(int id)
        {
            var leaveType = await _context.LeaveTypes.FindAsync(id);
            return leaveType == null ? null : _mapper.Map<LeaveTypeReadOnlyVM>(leaveType);
        }

        public async Task<LeaveTypeEditVM?> GetLeaveTypeForEditAsync(int id)
        {
            var leaveType = await _context.LeaveTypes.FindAsync(id);
            return leaveType == null ? null : _mapper.Map<LeaveTypeEditVM>(leaveType);
        }

        public async Task Remove(int id)
        {
            var data = await _context.LeaveTypes.FindAsync(id);
            if (data != null)
            {
                _context.LeaveTypes.Remove(data);
                await _context.SaveChangesAsync();
            }
        }

        public async Task Edit(int id, LeaveTypeEditVM leaveTypeEditVM)
        {
            var data = await _context.LeaveTypes.FindAsync(id);
            if (data != null)
            {
                _mapper.Map(leaveTypeEditVM, data);
                await _context.SaveChangesAsync();
            }
        }

        public async Task Create(LeaveTypeCreateVM leaveTypeCreateVM)
        {
            var leaveType = _mapper.Map<LeaveType>(leaveTypeCreateVM);
            _context.LeaveTypes.Add(leaveType);
            await _context.SaveChangesAsync();
        }

        public bool LeaveTypeExists(int id)
        {
            return _context.LeaveTypes.Any(e => e.Id == id);
        }

        public async Task<bool> CheckIfLeaveTypeNameExists(string name, int? excludeId = null)
        {
            var lowerCaseName = name.ToLower();
            return await _context.LeaveTypes.AnyAsync(lt =>
                lt.Name.ToLower().Equals(lowerCaseName) && lt.Id != excludeId);
        }
    }
}
