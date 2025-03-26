namespace BookHaven.Domain.Const
{
	public static class RejexPatterns
	{
		//the defualt in server side (IdentityUser)
		public const string StrongPassword = "(?=(.*[0-9]))(?=.*[\\!@#$%^&*()\\\\[\\]{}\\-_+=~`|:;\"'<>,./?])(?=.*[a-z])(?=(.*[A-Z]))(?=(.*)).{8,}";
		public const string InvalidUserName = "^[a-zA-Z0-9]+$";
		public const string OnlyEnglishLitter = "^[a-zA-Z\\s]+$";
		public const string ValidphoneNumber = "^01[0,1,2,15]{1}[0-9]{8}$";
		public const string ValidNationalId = "^[2,3]{1}[0-9]{13}$";
	}
}
