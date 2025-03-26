namespace BookHaven.Web.Core.ViewModels
{
	public class RentalVM
	{
		public int Id { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public DateTime? ReturnDate { get; set; }
		public bool PenaltyPaid { get; set; }
		public SubscriperVM? Subscriper { get; set; }
		public bool IsDeleted { get; set; } //by defual false
		public string? BookTitle { get; set; }

		public int DelayInDays
		{
			get
			{
				var delay = 0; //1/11/2025 12:00:00 AM (value===>12:00:00)
				if (ReturnDate.HasValue && ReturnDate.Value > EndDate) //الكتاب رجع متأخر
					delay = (int)(ReturnDate.Value - EndDate).TotalDays;
				else if (!ReturnDate.HasValue && EndDate < DateTime.Today)//الكتاب مرجعش
					delay = (int)(DateTime.Today - EndDate).TotalDays;
				return delay;
			}
		}

	}
}
