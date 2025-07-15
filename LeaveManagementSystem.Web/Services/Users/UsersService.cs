using LeaveManagementSystem.Web.Common;
using LeaveManagementSystem.Web.Services.Periods;
using Microsoft.AspNetCore.Identity;

namespace LeaveManagementSystem.Web.Services.Users
{
	public class UsersService : IUsersService
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public UsersService(UserManager<ApplicationUser> userManager,
			IHttpContextAccessor httpContextAccessor)
		{
			_userManager = userManager;
			_httpContextAccessor = httpContextAccessor;
		}

		public async Task<ApplicationUser> GetLoggedInUser()
		{
			var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
			return user;
		}

		public async Task<ApplicationUser> GetUserById(string userId)
		{
			var user = await _userManager.FindByIdAsync(userId);
			return user;
		}

		public async Task<List<ApplicationUser>> GetEmployees()
		{
			var employees = await _userManager.GetUsersInRoleAsync(Roles.Employee);
			return employees.ToList();
		}
	}
}
