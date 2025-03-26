using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace BookHaven.Web.Helper
{
	public class CustomClaim : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>
	{
		public CustomClaim(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<IdentityOptions> options) : base(userManager, roleManager, options)
		{
		}
		protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
		{
			var identity = await base.GenerateClaimsAsync(user); //genarat claim while login
			identity.AddClaim(new Claim("FullName", user.FullName)); //add new claim
			return identity;
		}
	}
}
