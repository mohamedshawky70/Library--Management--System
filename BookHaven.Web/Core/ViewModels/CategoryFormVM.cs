

namespace BookHaven.Web.Core.ViewModels
{
	public class CategoryFormVM
	{
		public int Id { get; set; }
		[MaxLength(100, ErrorMessage = "max lenth is 100 character")]
		[RegularExpression(RejexPatterns.OnlyEnglishLitter, ErrorMessage = "Only English letter allowed")]
		public string Name { get; set; } = null!;
	}
}
