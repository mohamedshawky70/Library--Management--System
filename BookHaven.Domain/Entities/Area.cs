namespace BookHaven.Domain.Entities
{
	[Index(nameof(Name), nameof(GovernorateId), IsUnique = true)]//اسماء المناطق متتكررش مع نفس المحافظة
	public class Area : BaseModel
	{
		public int Id { get; set; }
		[MaxLength(60)]
		public string Name { get; set; }
		public int GovernorateId { get; set; }
		public Governorate? Governorate { get; set; }
		public ICollection<Subscriper>? subscripers { get; set; } = new List<Subscriper>();
	}
}
