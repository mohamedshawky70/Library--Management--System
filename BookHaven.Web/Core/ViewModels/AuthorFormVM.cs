namespace BookHaven.Web.Core.ViewModels
{
	public class AuthorFormVM
	{
		public int Id { get; set; }
		[MaxLength(100)]
		public string Name { get; set; } = null!;
	}
}
