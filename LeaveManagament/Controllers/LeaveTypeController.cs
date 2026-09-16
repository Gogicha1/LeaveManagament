using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LeaveManagament.Data;
using LeaveManagament.Models.LeaveTypes;
using AutoMapper;
using LeaveManagament.Services;

namespace LeaveManagament.Controllers
{
    public class LeaveTypeController(ILeaveTypeServices leaveTypeServices) : Controller
    {
        private const string LeaveTypeNameExistsErrorMessage = "Leave type with this name already exists.";
        private readonly ILeaveTypeServices _leaveTypeServices = leaveTypeServices;

        // GET: LeaveType
        public async Task<IActionResult> Index()
        {
            var viewData = await _leaveTypeServices.GetAllLeaveTypesAsync();
            return View(viewData);
        }

        // GET: LeaveType/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveType = await _leaveTypeServices.GetLeaveTypeByIdAsync(id.Value);
            if (leaveType == null)
            {
                return NotFound();
            }

            return View(leaveType);
        }

        // GET: LeaveType/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LeaveType/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LeaveTypeCreateVM leaveTypeVM)
        {
            if (await _leaveTypeServices.CheckIfLeaveTypeNameExists(leaveTypeVM.Name))
            {
                ModelState.AddModelError(nameof(leaveTypeVM.Name), LeaveTypeNameExistsErrorMessage);
            }

            if (ModelState.IsValid)
            {
                await _leaveTypeServices.Create(leaveTypeVM);
                return RedirectToAction(nameof(Index));
            }
            return View(leaveTypeVM);
        }

        // GET: LeaveType/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveType = await _leaveTypeServices.GetLeaveTypeForEditAsync(id.Value);
            if (leaveType == null)
            {
                return NotFound();
            }

            return View(leaveType);
        }

        // POST: LeaveType/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LeaveTypeEditVM leaveTypeEdit)
        {
            if (id != leaveTypeEdit.Id)
            {
                return NotFound();
            }

            if (await _leaveTypeServices.CheckIfLeaveTypeNameExists(leaveTypeEdit.Name, leaveTypeEdit.Id))
            {
                ModelState.AddModelError(nameof(leaveTypeEdit.Name), LeaveTypeNameExistsErrorMessage);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _leaveTypeServices.Edit(id, leaveTypeEdit);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_leaveTypeServices.LeaveTypeExists(leaveTypeEdit.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(leaveTypeEdit);
        }

        // GET: LeaveType/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveType = await _leaveTypeServices.GetLeaveTypeByIdAsync(id.Value);
            if (leaveType == null)
            {
                return NotFound();
            }

            return View(leaveType);
        }

        // POST: LeaveType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _leaveTypeServices.Remove(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
