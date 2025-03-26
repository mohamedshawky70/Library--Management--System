
namespace BookHaven.Web.Core.ViewModels
{
	public class BookFormVM
	{
		public int Id { get; set; }
		[MaxLength(500)]
		public string Title { get; set; }
		public IFormFile? FormImg { get; set; }
		public string? Img { get; set; }
		[MaxLength(200)]
		//save publicId from cloudinary
		public string? ImgPublicId { get; set; }
		public string Publisher { get; set; } = null!;
		public string Description { get; set; } = null!;
		public string Hall { get; set; } = null!;
		[Display(Name = "Is available for rental")]
		public bool IsAvailableForRental { get; set; }
		[Display(Name = "Publishing Date")]
		[AssertThat("PublishingDate <= Today()", ErrorMessage = "Publishing date must be less than today.")]
		public DateTime PublishingDate { get; set; } = DateTime.Now;
		//For selectedList
		[Display(Name = "Author")]
		public int AuthorId { get; set; }
		public Author? Author { get; set; }
		public IList<int> CategoryId { get; set; } = new List<int>();
		public Category? Category { get; set; }
		//For selectedList

	}
}
