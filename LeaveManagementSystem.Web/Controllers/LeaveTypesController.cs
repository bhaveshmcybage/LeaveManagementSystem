﻿using LeaveManagementSystem.Application.Models.LeaveTypes;
using LeaveManagementSystem.Application.Services.LeaveTypes;

namespace LeaveManagementSystem.Web.Controllers
{
    [Authorize(Roles = Roles.Administrator)]
    public class LeaveTypesController(ILeaveTypesService _leaveTypesService, ILogger<LeaveTypesController> _logger) : Controller
    {
        
        private const string NameExistsValidationMessage = "This leave type already exist in the database";
        
        // GET: LeaveTypes
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Loading Leave Types");
            var viewData = await _leaveTypesService.GetAll();
			return View(viewData);
        }

        // GET: LeaveTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveType = await _leaveTypesService.Get<LeaveTypeReadOnlyVM>(id.Value);
            if (leaveType == null)
            {
                return NotFound();
            }
            return View(leaveType);
        }

        // GET: LeaveTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LeaveTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LeaveTypeCreateVM leaveTypeCreate)
        {
            // Adding custom validation and model state error
            if (await _leaveTypesService.CheckLeaveTypeNameExists(leaveTypeCreate.Name))
            {
                ModelState.AddModelError(nameof(leaveTypeCreate.Name), NameExistsValidationMessage);
            }
            
            if (ModelState.IsValid)
            {
                await _leaveTypesService.Create(leaveTypeCreate);
                return RedirectToAction(nameof(Index));
            }
            _logger.LogWarning("Leave Type attempt failed due to some invalidity");
            return View(leaveTypeCreate);
        }
        
        // GET: LeaveTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveType = await _leaveTypesService.Get<LeaveTypeEditVM>(id.Value);

            if (leaveType == null)
            {
                return NotFound();
            }
            return View(leaveType);
        }

        // POST: LeaveTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LeaveTypeEditVM leaveTypeEdit)
        {

            if (id != leaveTypeEdit.Id)
            {
                return NotFound();
            }

            // Adding custom validation and model state error
            if (await  _leaveTypesService.CheckLeaveTypeNameExistsForEdit(leaveTypeEdit))
            {
                ModelState.AddModelError(nameof(leaveTypeEdit.Name), NameExistsValidationMessage);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _leaveTypesService.Edit(leaveTypeEdit);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_leaveTypesService.LeaveTypeExists(leaveTypeEdit.Id))
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

        
        // GET: LeaveTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveType = await _leaveTypesService.Get<LeaveTypeReadOnlyVM>(id.Value);

            if (leaveType == null)
            {
                return NotFound();
            }

            return View(leaveType);
        }

        // POST: LeaveTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _leaveTypesService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
