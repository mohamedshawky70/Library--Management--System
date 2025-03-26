
using Microsoft.AspNetCore.Identity;

namespace BookHaven.Web.Seeds
{
	public class DefaultRoles
	{
		public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
		{
			if (!roleManager.Roles.Any())
			{
				await roleManager.CreateAsync(new IdentityRole(AppRoles.Admin));
				await roleManager.CreateAsync(new IdentityRole(AppRoles.Achieve));
				await roleManager.CreateAsync(new IdentityRole(AppRoles.Reception));
			}
		}
	}
}
