namespace BookHaven.Web.Core.ViewModels
{
	public class ReportRentalsVM
	{
		public IEnumerable<Rental>? rentals { get; set; } = new List<Rental>();
		public string? Range { get; set; }
		public List<int>? Delay { get; set; } = new List<int>();
	}
}
