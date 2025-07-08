
using AutoMapper;
using LeaveManagementSystem.Web.Common;
using LeaveManagementSystem.Web.Models.LeaveAllocations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Web.Services.LeaveAllocations
{
    public class LeaveAllocationsService : ILeaveAllocationsService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public LeaveAllocationsService(ApplicationDbContext context, 
            IHttpContextAccessor httpContextAccessor, 
            UserManager<ApplicationUser> userManager,
            IMapper mapper)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task AllocateLeave(string employeeId)
        {
            var leaveTypes = await _context.LeaveTypes.
	            Where(q => !q.LeaveAllocations.Any(x=>x.EmployeeId == employeeId)).ToListAsync();

            var currentDate = DateTime.Now;
            var period = await _context.Periods.SingleAsync(q => q.EndDate.Year == currentDate.Year);
            var monthsRemaining = period.EndDate.Month - currentDate.Month;

            foreach (var leaveType in leaveTypes)
            {
                var accuralRate = decimal.Divide(leaveType.NumberOfDays, 12);
                var leaveAllocation = new LeaveAllocation
                {
                    EmployeeId = employeeId,
                    LeaveTypeId = leaveType.Id,
                    PeriodId = period.Id,
                    Days = (int)Math.Ceiling(accuralRate * monthsRemaining)
                };
                _context.Add(leaveAllocation);
            }

            await _context.SaveChangesAsync();
        }

		public async Task EditAllocation(LeaveAllocationEditVM allocationEditVm)
		{
			await _context.LeaveAllocations
				.Where(q => q.Id == allocationEditVm.Id)
				.ExecuteUpdateAsync(s => s.SetProperty(e => e.Days, allocationEditVm.Days));

		}

		public async Task<LeaveAllocationEditVM> GetEmployeeAllocation(int allocationId)
		{
			var allocation = await _context.LeaveAllocations
				.Include(q => q.LeaveType)
				.Include(q => q.Employee)
				.FirstOrDefaultAsync(q => q.Id == allocationId);

            var model = _mapper.Map<LeaveAllocationEditVM>(allocation);

            return model;
		}

		public async Task<EmployeeAllocationVM> GetEmployeeAllocations(string? userId)
        {
			var user = string.IsNullOrEmpty(userId) 
				? await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User)
				: await _userManager.FindByIdAsync(userId);
			var allocations = await GetAllocations(user.Id);
            var allocationVmList = _mapper.Map<List<LeaveAllocation>, List<LeaveAllocationVM>>(allocations);
            var leaveTypesCount = _context.LeaveTypes.Count();
            
            var employeeVm = new EmployeeAllocationVM()
            {
                DateOfBirth = user.DateOfBirth,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Id = user.Id,
                LeaveAllocations = allocationVmList,
                IsCompletedAllocation = leaveTypesCount == allocations.Count
			};

            return employeeVm;
        }

        public async Task<List<EmployeeListVM>> GetEmployees()
        {
	        var users = await _userManager.GetUsersInRoleAsync(Roles.Employee);
	        var employees = _mapper.Map<List<ApplicationUser>, List<EmployeeListVM>>(users.ToList());

	        return employees;
        }

        private async Task<List<LeaveAllocation>> GetAllocations(string userId)
        {
	        //     string employeeId = string.Empty;
	        //     if (!string.IsNullOrEmpty(userId))
	        //     {
	        //      employeeId = userId;
	        //     }
	        //     else
	        //     {
	        //var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
	        //employeeId = user.Id;
	        //     }

	        var currentDate = DateTime.Now;
	        var leaveAllocations = await _context.LeaveAllocations
		        .Include(q => q.LeaveType)
		        .Include(q => q.Period)
		        .Where(q => q.EmployeeId == userId && q.Period.EndDate.Year == currentDate.Year)
		        .ToListAsync();

	        return leaveAllocations;
        }
	}
}
