namespace BookHaven.Web.Core.ViewModels
{
	public class RentalCopyBookVM
	{
		public int Id { get; set; }
		public string? Img { get; set; }
		public string Title { get; set; }
		public string subscriberId { get; set; }

		public IEnumerable<CopyBook>? CopiesBook { get; set; } = new List<CopyBook>();

	}
}
