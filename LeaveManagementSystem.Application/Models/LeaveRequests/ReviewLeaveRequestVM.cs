using LeaveManagementSystem.Application.Models.LeaveAllocations;

namespace LeaveManagementSystem.Application.Models.LeaveRequests
{
    public class ReviewLeaveRequestVM : LeaveRequestReadOnlyVM
    {
	    public EmployeeListVM Employee { get; set; } = new EmployeeListVM();
        [DisplayName("Additional Information")]
	    public string RequestComments { get; set; }
    }
}