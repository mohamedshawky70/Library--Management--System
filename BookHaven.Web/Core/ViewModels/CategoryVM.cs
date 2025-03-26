namespace BookHaven.Web.Core.ViewModels
{
	public class CategoryVM
	{
		public int Id { get; set; }
		public string Name { get; set; } = null!; //(null!) إطمن مش هسيبه فاضي
		public bool IsDeleted { get; set; } //by defual false
		public DateTime CreatedOn { get; set; } = DateTime.Now;
		public DateTime? LastUpdatedOn { get; set; }
	}
}
