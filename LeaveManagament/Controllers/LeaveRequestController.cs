using LeaveManagament.Models.LeaveRequests;
using LeaveManagament.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagament.Controllers
{
    [Authorize]
    public class LeaveRequestController(ILeaveRequestServices leaveRequestServices) : Controller
    {
        // Only these roles may look at other people's requests and decide on them.
        private const string ReviewerRoles = "Supervisor,Administrator";

        private readonly ILeaveRequestServices _leaveRequestServices = leaveRequestServices;

        // GET: LeaveRequest - the signed in employee's own requests and remaining days.
        public async Task<IActionResult> Index()
        {
            var viewData = await _leaveRequestServices.GetMyLeaveRequestsAsync();
            return View(viewData);
        }

        // GET: LeaveRequest/Create
        public async Task<IActionResult> Create()
        {
            var model = await _leaveRequestServices.GetCreateModelAsync();
            return View(model);
        }

        // POST: LeaveRequest/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LeaveRequestCreateVM leaveRequestVM)
        {
            if (!await _leaveRequestServices.LeaveTypeExistsAsync(leaveRequestVM.LeaveTypeId))
            {
                ModelState.AddModelError(nameof(leaveRequestVM.LeaveTypeId), "Please choose a valid leave type.");
            }
            else
            {
                var remainingDays = await _leaveRequestServices.GetRemainingDaysAsync(leaveRequestVM.LeaveTypeId);
                if (leaveRequestVM.NumberOfDays > remainingDays)
                {
                    ModelState.AddModelError(string.Empty,
                        $"You asked for {leaveRequestVM.NumberOfDays} day(s) but only have {remainingDays} day(s) left for this leave type.");
                }
            }

            if (ModelState.IsValid)
            {
                await _leaveRequestServices.CreateLeaveRequestAsync(leaveRequestVM);
                TempData["Success"] = "Your leave request was submitted and is waiting for approval.";
                return RedirectToAction(nameof(Index));
            }

            await _leaveRequestServices.PopulateLeaveTypesAsync(leaveRequestVM);
            return View(leaveRequestVM);
        }

        // POST: LeaveRequest/Cancel/5 - an employee withdraws a request nobody reviewed yet.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            var canceled = await _leaveRequestServices.CancelMyLeaveRequestAsync(id);
            if (canceled)
            {
                TempData["Success"] = "Your leave request was cancelled.";
            }
            else
            {
                TempData["Error"] = "This leave request can no longer be cancelled.";
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: LeaveRequest/ListRequests - every employee's requests, for supervisors.
        [Authorize(Roles = ReviewerRoles)]
        public async Task<IActionResult> ListRequests()
        {
            var viewData = await _leaveRequestServices.GetAllLeaveRequestsAsync();
            return View(viewData);
        }

        // GET: LeaveRequest/Review/5
        [Authorize(Roles = ReviewerRoles)]
        public async Task<IActionResult> Review(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveRequest = await _leaveRequestServices.GetLeaveRequestForReviewAsync(id.Value);
            if (leaveRequest == null)
            {
                return NotFound();
            }

            return View(leaveRequest);
        }

        // POST: LeaveRequest/Review/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ReviewerRoles)]
        public async Task<IActionResult> Review(int id, bool approved, string? reviewComments)
        {
            var reviewed = await _leaveRequestServices.ReviewLeaveRequestAsync(id, approved, reviewComments);
            if (reviewed)
            {
                TempData["Success"] = approved ? "The leave request was approved." : "The leave request was rejected.";
            }
            else
            {
                TempData["Error"] = "This leave request has already been reviewed.";
            }

            return RedirectToAction(nameof(ListRequests));
        }
    }
}
