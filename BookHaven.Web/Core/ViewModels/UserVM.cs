namespace BookHaven.Web.Core.ViewModels
{
	public class UserVM
	{
		public string Id { get; set; }//in ASPNetUser id column is nvachar
		public string FullName { get; set; }
		public string UserName { get; set; }
		public string Email { get; set; }
		public bool IsDeleted { get; set; }
		public DateTimeOffset LockedOutEnd { get; set; }
		public DateTime CreatedOn { get; set; } = DateTime.Now;
		public DateTime? LastUpdatedOn { get; set; }
	}
}
