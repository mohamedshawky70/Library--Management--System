namespace BookHaven.Web.Core.ViewModels
{
	public class CopyBookFormVM
	{
		public int Id { get; set; }
		public int bookId { get; set; }
		[Range(minimum: 1, maximum: 100, ErrorMessage = "Edition must be between 1 and 100")]
		public byte Edition { get; set; }
		public int SerialNumber { get; set; }
		[Display(Name = "Is Available For Rental")]
		public bool IsAvailableForRental { get; set; }
		public bool ShowRentalInput { get; set; }

	}
}
