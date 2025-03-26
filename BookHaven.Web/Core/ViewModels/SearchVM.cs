namespace BookHaven.Web.Core.ViewModels
{
	public class SearchVM
	{
		public string? Id { get; set; }
		public string value { get; set; } = null!;
		public string? FullName { get; set; }
		public string? Email { get; set; }
		public string? ImgUrl { get; set; }
	}
}
