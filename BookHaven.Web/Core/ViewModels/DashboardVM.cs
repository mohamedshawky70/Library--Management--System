namespace BookHaven.Web.Core.ViewModels
{
	public class DashboardVM
	{
		public int BookAndCopies { get; set; }
		public int subscribers { get; set; }
		public IEnumerable<BookVM> ListAddedBooks { get; set; } = new List<BookVM>();
		public IEnumerable<BookVM> TopBooks { get; set; } = new List<BookVM>();
	}
}
