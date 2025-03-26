namespace BookHaven.Web.Core.ViewModels
{
	/*public class CopyRentalVM
	{
		public CopyBookVM CopyBook { get; set; }
		public DateTime RentalDate { get; set; } 
		public DateTime? ReturnDate { get; set; } 
		public DateTime? ExtendedOn { get; set; }
		public DateTime EndDate { get; set; }

		public int DelatyInDays
		{
			get
			{
				var delay = 0; //1/11/2025 12:00:00 AM (value===>12:00:00)
				if (ReturnDate.HasValue && ReturnDate.Value > EndDate) //الكتاب رجع متأخر
					delay = (int)(ReturnDate.Value - EndDate).TotalDays;
				else if(!ReturnDate.HasValue &&EndDate<DateTime.Today)//الكتاب مرجعش
					delay= (int)(DateTime.Today - EndDate).TotalDays;
				return delay;

			}
		}
	}*/
}
