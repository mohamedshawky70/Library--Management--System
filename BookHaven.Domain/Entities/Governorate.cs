

namespace BookHaven.Domain.Entities
{
	[Index(nameof(Name), IsUnique = true)]
	public class Governorate : BaseModel
	{
		public int Id { get; set; }
		[MaxLength(60)]
		public string Name { get; set; }
		public ICollection<Area>? Areaes { get; set; } = new List<Area>();
		public ICollection<Subscriper>? subscripers { get; set; } = new List<Subscriper>();

	}
}
