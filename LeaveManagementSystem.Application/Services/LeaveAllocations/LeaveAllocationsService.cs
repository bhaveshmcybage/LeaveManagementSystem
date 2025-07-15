
using AutoMapper;
using LeaveManagementSystem.Application.Models.LeaveAllocations;
using LeaveManagementSystem.Application.Services.Periods;
using LeaveManagementSystem.Application.Services.Users;
using Microsoft.AspNetCore.Http;

namespace LeaveManagementSystem.Application.Services.LeaveAllocations
{
    public class LeaveAllocationsService : ILeaveAllocationsService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUsersService _usersService;
        private readonly IPeriodsService _periodsService;
        private readonly IMapper _mapper;

        public LeaveAllocationsService(ApplicationDbContext context, 
            IHttpContextAccessor httpContextAccessor,
            IUsersService usersService,
            IPeriodsService periodsService,
            IMapper mapper)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _usersService = usersService;
            _periodsService = periodsService;
            _mapper = mapper;
        }

        public async Task AllocateLeave(string employeeId)
        {
            var leaveTypes = await _context.LeaveTypes.
	            Where(q => !q.LeaveAllocations.Any(x=>x.EmployeeId == employeeId)).ToListAsync();

			var period = await _periodsService.GetCurrentPeriod();
			var monthsRemaining = period.EndDate.Month - DateTime.Now.Month;

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

		public async Task<LeaveAllocation> GetCurrentAllocation(int leaveTypeId, string employeeId)
		{
			var period = await _periodsService.GetCurrentPeriod();
			var allocation = await _context.LeaveAllocations
				.FirstAsync(q => q.LeaveTypeId == leaveTypeId
								 && q.EmployeeId == employeeId
				                 && q.PeriodId == period.Id);
            return allocation;
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
				? await _usersService.GetLoggedInUser()
				: await _usersService.GetUserById(userId);
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
	        var users = await _usersService.GetEmployees();
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
