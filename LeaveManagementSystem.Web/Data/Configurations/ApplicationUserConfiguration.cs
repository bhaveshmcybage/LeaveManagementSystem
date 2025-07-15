using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeaveManagementSystem.Web.Data.Configurations
{
	public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
	{
		public void Configure(EntityTypeBuilder<ApplicationUser> builder)
		{
			var hasher = new PasswordHasher<ApplicationUser>();
			builder.HasData(
				new ApplicationUser
				{
					Id = "68d15c4b-b198-4a2d-ac2c-a9097a2b7859",
					Email = "admin@localhost.om",
					NormalizedEmail = "ADMIN@LOCALHOST.COM",
					UserName = "admin@localhost.om",
					NormalizedUserName = "ADMIN@LOCALHOST.COM",
					PasswordHash = hasher.HashPassword(null, "Pa$$w0rd"),
					EmailConfirmed = true,
					FirstName = "Default",
					LastName = "Admin",
					DateOfBirth = new DateOnly(1950, 12, 01)
				});
		}
	}
}
