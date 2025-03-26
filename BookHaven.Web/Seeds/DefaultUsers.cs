
using Microsoft.AspNetCore.Identity;

namespace BookHaven.Web.Seeds
{
	public static class DefaultUsers
	{
		public static async Task SeedAdminUserAsync(UserManager<ApplicationUser> userManager)
		{
			ApplicationUser Admin = new ApplicationUser()
			{
				UserName = "MoAdmin",
				FullName = "MohmedShawky",
				Email = "MoAdmin@BookHaven.Com",
				EmailConfirmed = true     //To don't ask to confirme
			};
			var user = await userManager.FindByEmailAsync(Admin.Email);
			if (user is null)
			{
				var result = await userManager.CreateAsync(Admin, "P@$$0Rd123");//must strong passowrd
				await userManager.AddToRoleAsync(Admin, AppRoles.Admin);
			}
		}
	}
}
