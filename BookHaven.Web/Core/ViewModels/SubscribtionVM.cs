

namespace BookHaven.Web.Core.ViewModels
{
	public class SubscribtionVM
	{
		public int Id { get; set; }
		public DateTime CreatedOn { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public string Status
		{
			get
			{
				return DateTime.Today > EndDate ? SubcribtionStatus.Expire : DateTime.Today < StartDate ? SubcribtionStatus.NotStart : SubcribtionStatus.Active;
			}
		}
	}
}
