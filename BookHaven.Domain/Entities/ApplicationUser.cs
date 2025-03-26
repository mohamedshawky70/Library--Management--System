namespace BookHaven.Domain.Entities
{
	//Prevent duplicate in db(server side)
	[Index(nameof(Email), IsUnique = true)]
	public class ApplicationUser : IdentityUser
	{
		public string? LastUpdatedById { get; set; }
		public string? CreatedById { get; set; }
		[MaxLength(100)]
		public string FullName { get; set; }
		public bool IsDeleted { get; set; }
		public DateTime CreatedOn { get; set; } = DateTime.Now;
		public DateTime? LastUpdatedOn { get; set; }
		public string? Img { get; set; }
	}
}
