namespace BookHaven.Web.Core.ViewModels
{
	public class RentalFormVM
	{
		public int Id { get; set; }
		[MaxLength(500)]
		public string Title { get; set; }
		public string? Img { get; set; }
		public string Publisher { get; set; } = null!;
		public bool IsAvailableForRental { get; set; }
		public bool IsDeleted { get; set; } //by defual false
		public int AuthorId { get; set; }
		public Author? Author { get; set; }
		public string? subsciberId { get; set; }
		public ICollection<CopyBook> CopiesBook { get; set; } = new List<CopyBook>();

	}
}
