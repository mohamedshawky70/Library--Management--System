namespace BookHaven.Domain.Entities
{
	public class CopyBook : BaseModel
	{
		public int Id { get; set; }
		public bool IsAvailableForRental { get; set; }
		public byte EditionNumber { get; set; }
		public int SerialNumber { get; set; }//(squance) auto increament
		public int BookId { get; set; }
		public Book? book { get; set; }
		public bool IsDeleted { get; set; } //by defual false
		public DateTime CreatedOn { get; set; } = DateTime.Now;
	}
}
