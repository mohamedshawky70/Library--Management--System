

namespace BookHaven.Web.Core.ViewModels
{
	public class AuthorVM
	{
		public int Id { get; set; }
		[RegularExpression(RejexPatterns.OnlyEnglishLitter, ErrorMessage = "Only English letter allowed")]
		public string Name { get; set; } = null!;
		public bool IsDeleted { get; set; }
		public DateTime CreatedOn { get; set; } = DateTime.Now;
		public DateTime? LastUpdatedOn { get; set; }
	}
}
