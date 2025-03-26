namespace BookHaven.Domain.Entities
{
	[Index(nameof(Email), IsUnique = true)]
	[Index(nameof(NationalId), IsUnique = true)]
	[Index(nameof(MobileNumber), IsUnique = true)]
	public class Subscriper : BaseModel
	{
		public int Id { get; set; }
		[MaxLength(20)]
		public string FName { get; set; }
		[MaxLength(20)]
		public string LName { get; set; }
		[MaxLength(14)]
		public string NationalId { get; set; }
		[MaxLength(11)]
		public string MobileNumber { get; set; }
		public bool HasWatsApp { get; set; }
		public bool IsBlackList { get; set; }
		public string? ImgUrl { get; set; }
		[MaxLength(50)]
		[EmailAddress]
		public string Email { get; set; }
		[MaxLength(200)]
		public string Address { get; set; }
		public DateTime DateOfBirth { get; set; }
		public int AreaId { get; set; }
		public Area? Area { get; set; }
		public int GovernorateId { get; set; }
		public Governorate? Governorate { get; set; }
		public ICollection<Subcribtion>? subcribtions { get; set; } = new List<Subcribtion>();
		public ICollection<Rental>? Rentals { get; set; } = new List<Rental>();


	}
}
