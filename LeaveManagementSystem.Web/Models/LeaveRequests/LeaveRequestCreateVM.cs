using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LeaveManagementSystem.Web.Models.LeaveRequests
{
    public class LeaveRequestCreateVM : IValidatableObject
    {
		[DisplayName("Start Date")]
        [Microsoft.Build.Framework.Required]
	    public DateOnly StartDate { get; set; }
        [DisplayName("End Date")]
        [Microsoft.Build.Framework.Required]
        public DateOnly EndDate { get; set; }
        [DisplayName("Desired Leave Type")]
        [Microsoft.Build.Framework.Required]
        public int LeaveTypeId { get; set; }
        [DisplayName("Additional Information")]
        [StringLength(250)]
        public string? RequestComments { get; set; }
	    public SelectList? LeaveTypes { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (StartDate > EndDate)
            {
                yield return new ValidationResult("Start Date Must Be Before the End Date", [nameof(StartDate), nameof(EndDate)]);
            }
        }
	}
}