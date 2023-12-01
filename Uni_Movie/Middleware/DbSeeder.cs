using Microsoft.AspNetCore.Identity;
using Uni_Movie.Areas.Identity.Data;

namespace Uni_Movie.Middleware
{
	public static class DbSeeder
	{
		public static async Task IdentityInitializer(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
		{
			List<string> roles = new() { "admins", "customers" };
			foreach (var item in roles)
			{
				if ((await roleManager.RoleExistsAsync(item) == false))
				{
					IdentityRole identityRole = new(item);
					await roleManager.CreateAsync(identityRole);
				}
			}
			ApplicationUser admin = await userManager.FindByNameAsync("admin@gmail.com");
			if (admin == null)
			{
				admin = new ApplicationUser()
				{
					UserName = "admin@gmail.com",
					Email = "admin@gmail.com",
					EmailConfirmed = true,
					PhoneNumber = "09397826040",
					PhoneNumberConfirmed = true,
					firstName = "owner",
					lastName = "owner",
				};
				await userManager.CreateAsync(admin, "rezA=-0987");
				if (await userManager.IsInRoleAsync(admin, "admins") == false)
				{
					await userManager.AddToRoleAsync(admin, "admins");
				}
			}
		}
	}
}

