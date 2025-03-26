namespace BookHaven.Web.Core.ViewModels
{
	public class BookVM
	{
		public string? Key { get; set; }
		public int Id { get; set; }
		[MaxLength(500)]
		public string Title { get; set; }
		public string? Img { get; set; }
		public string Publisher { get; set; } = null!;
		public string Description { get; set; } = null!;
		public string Hall { get; set; } = null!;
		public bool IsAvailableForRental { get; set; }
		public DateTime PublishingDate { get; set; } = DateTime.Now;
		public string Author { get; set; }
		public IEnumerable<string> Categories { get; set; }
		public bool IsDeleted { get; set; } //by defual false
		public IEnumerable<CopyBookVM> CopiesBook { get; set; }
		public DateTime CreatedOn { get; set; } = DateTime.Now;
	}
}
