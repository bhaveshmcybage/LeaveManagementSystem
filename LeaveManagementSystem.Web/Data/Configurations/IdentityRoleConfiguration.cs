using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeaveManagementSystem.Web.Data.Configurations
{
	public class IdentityRoleConfiguration : IEntityTypeConfiguration<IdentityRole>
	{
		public void Configure(EntityTypeBuilder<IdentityRole> builder)
		{
			builder.HasData(
				new IdentityRole
				{
					Id = "33b1258b-0dfa-44af-b3bd-b586beae7c9d",
					Name = "Employee",
					NormalizedName = "EMPLOYEE"
				},
				new IdentityRole
				{
					Id = "6a773c09-6ffe-4202-bec1-d284be134155",
					Name = "Supervisor",
					NormalizedName = "SUPERVISOR"
				},
				new IdentityRole
				{
					Id = "fc96c318-9a59-495e-a074-485e6335d878",
					Name = "Administrator",
					NormalizedName = "ADMINISTRATOR"
				});
		}
	}
}
