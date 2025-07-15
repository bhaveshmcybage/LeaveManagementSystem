﻿using LeaveManagementSystem.Application.Models.LeaveAllocations;
using LeaveManagementSystem.Application.Services.LeaveAllocations;
using LeaveManagementSystem.Application.Services.LeaveTypes;


namespace LeaveManagementSystem.Web.Controllers
{
    [Authorize]
    public class LeaveAllocationsController : Controller
    {
        private readonly ILeaveAllocationsService _leaveAllocationsService;
        private readonly ILeaveTypesService _leaveTypesService;

        public LeaveAllocationsController(ILeaveAllocationsService leaveAllocationsService, ILeaveTypesService leaveTypesService)
        {
	        this._leaveAllocationsService = leaveAllocationsService;
	        _leaveTypesService = leaveTypesService;
        }

        [Authorize(Roles = Roles.Administrator)]
        public async Task<IActionResult> Index()
        {
	        var employees = await _leaveAllocationsService.GetEmployees();

	        return View(employees);
        }

        [Authorize(Roles = Roles.Administrator)]
        [HttpPost]
        [ValidateAntiForgeryToken]
		public async Task<IActionResult> AllocateLeave(string? id)
        {
	        await _leaveAllocationsService.AllocateLeave(id);

	        return RedirectToAction(nameof(Details), new {userId = id});
        }

		[Authorize(Roles = Roles.Administrator)]
		public async Task<IActionResult> EditAllocation(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var allocation = await _leaveAllocationsService.GetEmployeeAllocation(id.Value);

			if (allocation == null)
			{
				return NotFound();
			}
			return View(allocation);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> EditAllocation(LeaveAllocationEditVM allocation)
		{
			if (await _leaveTypesService.DaysExceedMaximum(allocation.LeaveType.Id, allocation.Days))
			{
				ModelState.AddModelError("Days", "The allocation exceeds maximum leave type value");
			}

			if (ModelState.IsValid)
			{
				await _leaveAllocationsService.EditAllocation(allocation);
				return RedirectToAction(nameof(Details), new { userId = allocation.Employee.Id });
			}
			var days = allocation.Days;
			allocation = await _leaveAllocationsService.GetEmployeeAllocation(allocation.Id);
			allocation.Days = days;
			return View(allocation);
		}

		public async Task<IActionResult> Details(string? userId)
        {
            var employeeVm = await _leaveAllocationsService.GetEmployeeAllocations(userId);

            return View(employeeVm);
        }
    }
}
