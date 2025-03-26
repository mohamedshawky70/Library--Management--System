

namespace BookHaven.Web.Core.ViewModels
{
	public class SubscriperFormVM
	{
		public string? Id { get; set; }
		[MaxLength(20)]
		public string FName { get; set; }
		[MaxLength(20)]
		public string LName { get; set; }
		[MaxLength(14)]
		[RegularExpression(RejexPatterns.ValidNationalId, ErrorMessage = "Invalid national id!")]
		public string NationalId { get; set; }
		[MaxLength(11)]
		[RegularExpression(RejexPatterns.ValidphoneNumber, ErrorMessage = "Invalid phone number!")]
		public string MobileNumber { get; set; }
		public bool HasWatsApp { get; set; }
		public bool IsBlackList { get; set; }
		public string? ImgUrl { get; set; }
		[RequiredIf("Id==''", ErrorMessage = "image field is required")]//Create subscriper
		public IFormFile? Img { get; set; }
		[MaxLength(50)]
		[EmailAddress]
		public string Email { get; set; }
		[MaxLength(400)]
		public string Address { get; set; }
		[AssertThat("DateOfBirth < Today()", ErrorMessage = "Date Of Birth must be less than today.")]
		public DateTime DateOfBirth { get; set; }
		public int AreaId { get; set; }
		public Area? Area { get; set; }
		public int GovernorateId { get; set; }
		public Governorate? Governorate { get; set; }
	}
}
