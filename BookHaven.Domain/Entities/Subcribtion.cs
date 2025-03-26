namespace BookHaven.Domain.Entities
{
	public class Subcribtion
	{
		public int Id { get; set; }
		public DateTime CreatedOn { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public int SubscriperId { get; set; }
		public Subscriper? Subscriper { get; set; }
	}
}
