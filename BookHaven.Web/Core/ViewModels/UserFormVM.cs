namespace BookHaven.Web.Core.ViewModels
{
	public class UserFormVM
	{
		public string? Id { get; set; }
		[RegularExpression(RejexPatterns.OnlyEnglishLitter, ErrorMessage = "Only English letter allowed")]
		[MaxLength(30, ErrorMessage = "max length of name is 30 character"), Display(Name = "Full Name")]
		public string FullName { get; set; }
		[MaxLength(30, ErrorMessage = "max length of name is 30 character"), Display(Name = "User Name")]
		[RegularExpression(RejexPatterns.InvalidUserName, ErrorMessage = "{0} can only character or digit without spaces")]
		public string UserName { get; set; }

		[EmailAddress]
		[Display(Name = "Email")]
		public string Email { get; set; }

		[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 8)]
		[DataType(DataType.Password)]
		[RegularExpression(RejexPatterns.StrongPassword, ErrorMessage = "Passwords require contain an [A-Z], [a-z], [0-9], and a non-alphanumeric character, at least 8 characters long")]
		[RequiredIf("Id==null")]
		public string? Password { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Confirm password")]
		[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
		[RequiredIf("Id==null")]
		public string? ConfirmPassword { get; set; }
		public IList<string> RolesId { get; set; } = new List<string>();
	}
}
