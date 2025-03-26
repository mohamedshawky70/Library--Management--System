namespace BookHaven.Web.Core.ViewModels
{
	public class ReportBooksVM
	{
		public IEnumerable<Book> Books { get; set; }
		public IList<int>? AuthorId { get; set; } = new List<int>();
		public Author? Author { get; set; }
		public IList<int>? CategoryId { get; set; } = new List<int>();
		public Category? Category { get; set; }

	}
}
