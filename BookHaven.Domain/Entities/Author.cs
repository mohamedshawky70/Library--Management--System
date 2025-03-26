
namespace BookHaven.Domain.Entities
{
	public class Author : BaseModel
	{
		public int Id { get; set; }
		[MaxLength(50)]                   //Don't work I don't know whyyyyyyyyyyyyyyyyyyyyyyy
		public string Name { get; set; } = null!; //(null!) إطمن مش هسيبه فاضي
	}
}
