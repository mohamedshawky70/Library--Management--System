

namespace BookHaven.Web.Core.ViewModels
{
	public class SubscriperVM
	{
		public int UcryptId { get; set; }
		public string? Id { get; set; }
		[MaxLength(40)]
		public string FullName { get; set; }
		[MaxLength(14)]
		[RegularExpression(RejexPatterns.ValidNationalId, ErrorMessage = "Invalid national id!")]
		public string NationalId { get; set; }
		[MaxLength(11)]
		[RegularExpression(RejexPatterns.ValidphoneNumber, ErrorMessage = "Invalid phone number!")]
		public string MobileNumber { get; set; }
		public bool HasWatsApp { get; set; }
		public bool IsBlackList { get; set; }
		public string? ImgUrl { get; set; }
		[MaxLength(50)]
		[EmailAddress]
		public string Email { get; set; }
		[MaxLength(400)]
		public string Address { get; set; }
		public DateTime DateOfBirth { get; set; }
		public string Area { get; set; }
		public string Governorate { get; set; }

		public DateTime CreatedOn { get; set; } = DateTime.Now;
		public ICollection<SubscribtionVM>? subcribtions { get; set; } = new List<SubscribtionVM>();
		public ICollection<RentalVM>? Rentals { get; set; } = new List<RentalVM>();

	}
}
