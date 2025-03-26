namespace BookHaven.Domain.Entities
{
	//For relationship between book and category M to M
	public class Book : BaseModel
	{
		public int Id { get; set; }
		[MaxLength(500)]
		public string Title { get; set; }
		public string? Img { get; set; }
		//save publicId from cloudinary		
		public string? ImgPublicId { get; set; }
		[MaxLength(200)]
		public string Publisher { get; set; } = null!;
		public string Description { get; set; } = null!;
		public string Hall { get; set; } = null!;
		[Display(Name = "Is available for rental")]
		public bool IsAvailableForRental { get; set; }
		public bool IsDeleted { get; set; } //by defual false

		[Display(Name = "Publishing Date")]
		public DateTime PublishingDate { get; set; } = DateTime.Now;
		public int AuthorId { get; set; }
		public Author? Author { get; set; }
		public ICollection<BookCategory>? Categories { get; set; } = new List<BookCategory>();
		public ICollection<CopyBook> CopiesBook { get; set; } = new List<CopyBook>();
	}
}
