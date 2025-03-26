namespace BookHaven.Domain.Entities
{
	public class Rental : BaseModel
	{
		public int Id { get; set; }
		public DateTime StartDate { get; set; } = DateTime.Today;
		public DateTime EndDate { get; set; } = DateTime.Today.AddDays(7);
		public DateTime? ReturnDate { get; set; }
		public bool PenaltyPaid { get; set; }
		public int SubscriperId { get; set; }
		public Subscriper? Subscriper { get; set; }
		public string? BookTitle { get; set; }


	}
}
