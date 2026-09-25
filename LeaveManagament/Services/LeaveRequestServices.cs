using AutoMapper;
using LeaveManagament.Data;
using LeaveManagament.Models.LeaveRequests;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagament.Services
{
    public class LeaveRequestServices : ILeaveRequestServices
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LeaveRequestServices(
            ApplicationDbContext context,
            IMapper mapper,
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<LeaveRequestCreateVM> GetCreateModelAsync()
        {
            var model = new LeaveRequestCreateVM();
            await PopulateLeaveTypesAsync(model);
            return model;
        }

        public async Task PopulateLeaveTypesAsync(LeaveRequestCreateVM leaveRequestCreateVM)
        {
            var leaveTypes = await _context.LeaveTypes
                .OrderBy(lt => lt.Name)
                .ToListAsync();

            leaveRequestCreateVM.LeaveTypes = new SelectList(
                leaveTypes, nameof(LeaveType.Id), nameof(LeaveType.Name), leaveRequestCreateVM.LeaveTypeId);
        }

        public async Task CreateLeaveRequestAsync(LeaveRequestCreateVM leaveRequestCreateVM)
        {
            var leaveRequest = _mapper.Map<LeaveRequest>(leaveRequestCreateVM);
            leaveRequest.RequestingEmployeeId = GetCurrentUserId();
            leaveRequest.DateRequested = DateTime.Now;
            leaveRequest.Status = LeaveRequestStatus.Pending;

            _context.LeaveRequests.Add(leaveRequest);
            await _context.SaveChangesAsync();
        }

        public async Task<EmployeeLeaveRequestsVM> GetMyLeaveRequestsAsync()
        {
            var employeeId = GetCurrentUserId();

            var myRequests = await _context.LeaveRequests
                .Include(lr => lr.LeaveType)
                .Where(lr => lr.RequestingEmployeeId == employeeId)
                .OrderByDescending(lr => lr.DateRequested)
                .ToListAsync();

            var leaveTypes = await _context.LeaveTypes
                .OrderBy(lt => lt.Name)
                .ToListAsync();

            return new EmployeeLeaveRequestsVM
            {
                Requests = _mapper.Map<List<LeaveRequestReadOnlyVM>>(myRequests),
                Balances = leaveTypes
                    .Select(lt => new LeaveTypeBalanceVM
                    {
                        LeaveTypeId = lt.Id,
                        LeaveTypeName = lt.Name,
                        AllowedDays = lt.NumberOfDays,
                        UsedDays = myRequests
                            .Where(lr => lr.LeaveTypeId == lt.Id && CountsAgainstAllowance(lr))
                            .Sum(lr => lr.NumberOfDays)
                    })
                    .ToList()
            };
        }

        public async Task<List<LeaveRequestReviewVM>> GetAllLeaveRequestsAsync()
        {
            var leaveRequests = await _context.LeaveRequests
                .Include(lr => lr.LeaveType)
                .Include(lr => lr.RequestingEmployee)
                .Include(lr => lr.Reviewer)
                .OrderBy(lr => lr.Status)
                .ThenByDescending(lr => lr.DateRequested)
                .ToListAsync();

            return _mapper.Map<List<LeaveRequestReviewVM>>(leaveRequests);
        }

        public async Task<LeaveRequestReviewVM?> GetLeaveRequestForReviewAsync(int id)
        {
            var leaveRequest = await _context.LeaveRequests
                .Include(lr => lr.LeaveType)
                .Include(lr => lr.RequestingEmployee)
                .Include(lr => lr.Reviewer)
                .FirstOrDefaultAsync(lr => lr.Id == id);

            return leaveRequest == null ? null : _mapper.Map<LeaveRequestReviewVM>(leaveRequest);
        }

        public async Task<bool> ReviewLeaveRequestAsync(int id, bool approved, string? reviewComments)
        {
            var leaveRequest = await _context.LeaveRequests.FindAsync(id);

            // Only a request that is still waiting for a decision can be reviewed.
            if (leaveRequest == null || leaveRequest.Status != LeaveRequestStatus.Pending)
            {
                return false;
            }

            leaveRequest.Status = approved ? LeaveRequestStatus.Approved : LeaveRequestStatus.Declined;
            leaveRequest.ReviewerId = GetCurrentUserId();
            leaveRequest.ReviewComments = reviewComments;
            leaveRequest.DateReviewed = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CancelMyLeaveRequestAsync(int id)
        {
            var employeeId = GetCurrentUserId();
            var leaveRequest = await _context.LeaveRequests
                .FirstOrDefaultAsync(lr => lr.Id == id && lr.RequestingEmployeeId == employeeId);

            if (leaveRequest == null || leaveRequest.Status != LeaveRequestStatus.Pending)
            {
                return false;
            }

            leaveRequest.Status = LeaveRequestStatus.Canceled;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> LeaveTypeExistsAsync(int leaveTypeId)
        {
            return await _context.LeaveTypes.AnyAsync(lt => lt.Id == leaveTypeId);
        }

        public async Task<int> GetRemainingDaysAsync(int leaveTypeId)
        {
            var leaveType = await _context.LeaveTypes.FindAsync(leaveTypeId);
            if (leaveType == null)
            {
                return 0;
            }

            var employeeId = GetCurrentUserId();
            var currentYear = DateTime.Today.Year;

            var reservedPeriods = await _context.LeaveRequests
                .Where(lr => lr.RequestingEmployeeId == employeeId
                    && lr.LeaveTypeId == leaveTypeId
                    && lr.StartDate.Year == currentYear
                    && (lr.Status == LeaveRequestStatus.Pending || lr.Status == LeaveRequestStatus.Approved))
                .Select(lr => new { lr.StartDate, lr.EndDate })
                .ToListAsync();

            var usedDays = reservedPeriods.Sum(p => p.EndDate.DayNumber - p.StartDate.DayNumber + 1);
            return leaveType.NumberOfDays - usedDays;
        }

        // Pending and approved days stay reserved, rejected and cancelled days go back to the employee.
        private static bool CountsAgainstAllowance(LeaveRequest leaveRequest)
        {
            return leaveRequest.StartDate.Year == DateTime.Today.Year
                && (leaveRequest.Status == LeaveRequestStatus.Pending
                    || leaveRequest.Status == LeaveRequestStatus.Approved);
        }

        private string GetCurrentUserId()
        {
            return _userManager.GetUserId(_httpContextAccessor.HttpContext!.User)
                ?? throw new InvalidOperationException("No signed in user could be found for this request.");
        }
    }
}
