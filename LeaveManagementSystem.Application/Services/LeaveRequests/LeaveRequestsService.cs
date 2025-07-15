using AutoMapper;
using LeaveManagementSystem.Application.Models.LeaveAllocations;
using LeaveManagementSystem.Application.Models.LeaveRequests;
using LeaveManagementSystem.Application.Services.LeaveAllocations;
using LeaveManagementSystem.Application.Services.Periods;
using LeaveManagementSystem.Application.Services.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Application.Services.LeaveRequests
{
	public class LeaveRequestsService : ILeaveRequestsService
	{
        private readonly IMapper _mapper;
        private readonly IUsersService _usersService;
        private readonly IPeriodsService _periodsService;
        private readonly ILeaveAllocationsService _leaveAllocationsService;
        private readonly ApplicationDbContext _context;

        public LeaveRequestsService(IMapper mapper,
	        IUsersService usersService,
			IPeriodsService periodsService,
	        ILeaveAllocationsService leaveAllocationsService,
	        ApplicationDbContext context)
        {
            _mapper = mapper;
            _usersService = usersService;
            _periodsService = periodsService;
            _leaveAllocationsService = leaveAllocationsService;
            _context = context;
        }
		public async Task CancelLeaveRequest(int leaveRequestId)
		{
			var leaveRequest = await _context.LeaveRequests.FindAsync(leaveRequestId);
			leaveRequest.LeaveRequestStatusId = (int)LeaveRequestStatusEnum.Canceled;

			await UpdateAllocationDays(leaveRequest, false);
			await _context.SaveChangesAsync();

		}

		public async Task CreateLeaveRequest(LeaveRequestCreateVM model)
        {
            var leaveRequest = _mapper.Map<LeaveRequest>(model);
            var user = await _usersService.GetLoggedInUser();
            leaveRequest.EmployeeId = user.Id;

            leaveRequest.LeaveRequestStatusId = (int)LeaveRequestStatusEnum.Pending;
            _context.Add(leaveRequest);

            await UpdateAllocationDays(leaveRequest, true);
			await _context.SaveChangesAsync();
        }

		public async Task<EmployeeLeaveRequestListVM> AdminGetAllLeaveRequests()
		{
			var leaveRequests = await _context.LeaveRequests
				.Include(q => q.LeaveType)
				.ToListAsync();

			var approvedLeaveRequestCount = leaveRequests.
				Count(q => q.LeaveRequestStatusId == (int)LeaveRequestStatusEnum.Approved);
			var pendingLeaveRequestCount = leaveRequests.
				Count(q => q.LeaveRequestStatusId == (int)LeaveRequestStatusEnum.Pending);
			var declinedLeaveRequestCount = leaveRequests.
				Count(q => q.LeaveRequestStatusId == (int)LeaveRequestStatusEnum.Declined);

			var leaveRequestModels = leaveRequests.Select(q => new LeaveRequestReadOnlyVM
			{
				StartDate = q.StartDate,
				EndDate = q.EndDate,
				Id = q.Id,
				LeaveType = q.LeaveType.Name,
				LeaveRequestStatus = (LeaveRequestStatusEnum)q.LeaveRequestStatusId,
				NumberOfDays = q.EndDate.DayNumber - q.StartDate.DayNumber
			}).ToList();

			var model = new EmployeeLeaveRequestListVM
			{
				ApprovedRequests = approvedLeaveRequestCount,
				PendingRequests = pendingLeaveRequestCount,
				DeclinedRequests = declinedLeaveRequestCount,
				TotalRequests = leaveRequests.Count,
				LeaveRequests = leaveRequestModels
			};
			return model;
		}

		public async Task<List<LeaveRequestReadOnlyVM>> GetEmployeeLeaveRequests()
		{
			var user = await _usersService.GetLoggedInUser();
			var leaveRequests = await _context.LeaveRequests
				.Include(q => q.LeaveType)
				.Where(q => q.EmployeeId == user.Id)
				.ToListAsync();
			var model = leaveRequests.Select(q => new LeaveRequestReadOnlyVM
			{
                StartDate = q.StartDate,
                EndDate = q.EndDate,
                Id = q.Id,
                LeaveType = q.LeaveType.Name,
                LeaveRequestStatus = (LeaveRequestStatusEnum)q.LeaveRequestStatusId,
                NumberOfDays = q.EndDate.DayNumber - q.StartDate.DayNumber
			}).ToList();

            return model;
		}
		
        public async Task<bool> RequestDatedExceedAllocation(LeaveRequestCreateVM model)
        {
	        var user = await _usersService.GetLoggedInUser();
			var numberOfDays = model.EndDate.DayNumber - model.StartDate.DayNumber;
			var allocation = await _leaveAllocationsService.GetCurrentAllocation(model.LeaveTypeId, user.Id);
			
			return allocation.Days < numberOfDays;
        }


        public async Task ReviewLeaveRequest(int leaveRequestId, bool approved)
		{
			var user = await _usersService.GetLoggedInUser();
			var leaveRequest = await _context.LeaveRequests.FindAsync(leaveRequestId);
			leaveRequest.LeaveRequestStatusId = approved
				? (int)LeaveRequestStatusEnum.Approved
				: (int)LeaveRequestStatusEnum.Declined;

			leaveRequest.ReviewerId = user.Id;

			if (!approved)
			{
				await UpdateAllocationDays(leaveRequest, false);
			}

			await _context.SaveChangesAsync();

		}

        public async Task<ReviewLeaveRequestVM> GetLeaveRequestForReview(int id)
        {
	        var leaveRequests = await _context.LeaveRequests
		        .Include(q => q.LeaveType)
		        .FirstAsync(q => q.Id == id);
	        var user = await _usersService.GetUserById(leaveRequests.EmployeeId);

	        var model = new ReviewLeaveRequestVM
	        {
		        StartDate = leaveRequests.StartDate,
		        EndDate = leaveRequests.EndDate,
		        Id = leaveRequests.Id,
		        LeaveType = leaveRequests.LeaveType.Name,
		        LeaveRequestStatus = (LeaveRequestStatusEnum)leaveRequests.LeaveRequestStatusId,
		        NumberOfDays = leaveRequests.EndDate.DayNumber - leaveRequests.StartDate.DayNumber,
				RequestComments = leaveRequests.RequestComments,
		        Employee = new EmployeeListVM()
		        {
			        Id = leaveRequests.EmployeeId,
			        Email = user.Email,
			        FirstName = user.FirstName,
			        LastName = user.LastName
		        }
	        };

			return model;
        }

        private async Task UpdateAllocationDays(LeaveRequest leaveRequest, bool deductDays)
        {
			var allocation = await _leaveAllocationsService.GetCurrentAllocation(leaveRequest.LeaveTypeId, leaveRequest.EmployeeId);
			var numberOfDays = CalculateDays(leaveRequest.StartDate, leaveRequest.EndDate);

			if (deductDays)
			{
				allocation.Days -= numberOfDays;
			}
			else
			{
				allocation.Days += numberOfDays;
			}
	        _context.Entry(allocation).State = EntityState.Modified;
		}

        private int CalculateDays(DateOnly start, DateOnly end)
        {
			return end.DayNumber - start.DayNumber;
        }
	}
}
