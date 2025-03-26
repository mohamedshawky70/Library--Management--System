namespace BookHaven.Web.Core.ViewModels
{
	public class CopyBookVM
	{
		public int Id { get; set; }
		public bool IsAvailableForRental { get; set; }
		public byte EditionNumber { get; set; }
		public int SerialNumber { get; set; }//(squance) auto increament
		public bool IsDeleted { get; set; } //by defual false
		public DateTime CreatedOn { get; set; } = DateTime.Now;
		public string BookTitle { get; set; }
	}
}
