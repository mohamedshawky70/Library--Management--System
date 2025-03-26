

namespace BookHaven.Web.Core.ViewModels
{
	public class ResetPassword
	{
		public string Id { get; set; }
		[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 8)]
		[DataType(DataType.Password)]
		[RegularExpression(RejexPatterns.StrongPassword, ErrorMessage = "Passwords require contain an [A-Z], [a-z], [0-9], and a non-alphanumeric character, at least 8 characters long")]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Confirm password")]
		[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; }
	}
}
