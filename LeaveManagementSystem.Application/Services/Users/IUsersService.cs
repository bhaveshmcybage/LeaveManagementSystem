namespace LeaveManagementSystem.Application.Services.Users;

public interface IUsersService
{
	Task<ApplicationUser> GetLoggedInUser();
	Task<ApplicationUser> GetUserById(string userId);
	Task<List<ApplicationUser>> GetEmployees();
}