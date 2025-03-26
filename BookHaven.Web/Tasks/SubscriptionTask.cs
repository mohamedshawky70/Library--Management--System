using Hangfire;

namespace BookHaven.Web.Tasks
{
	public class SubscriptionTask
	{
		private readonly IUnitOfWord _unitOfWord;
		private readonly IWebHostEnvironment _webHostEnvironment;
		private readonly IEmailSender _emailSender;

		public SubscriptionTask(IUnitOfWord unitOfWord, IWebHostEnvironment webHostEnvironment, IEmailSender emailSender)
		{
			_unitOfWord = unitOfWord;
			_webHostEnvironment = webHostEnvironment;
			_emailSender = emailSender;
		}

		public async Task SubscribtionExpirationAlerts()
		{
			var subscribers = _unitOfWord.Subscriper.GetAll().Include(s => s.subcribtions)
				.Where(s => !s.IsDeleted && s.subcribtions.OrderByDescending(s => s.EndDate).First().EndDate == DateTime.Today.AddDays(5))
				.ToList();
			foreach (var subscriber in subscribers)
			{
				var TempPath = $"{_webHostEnvironment.WebRootPath}/Templates/{EmailTemplates.Notification}.html";
				StreamReader streamReader = new StreamReader(TempPath);
				var body = streamReader.ReadToEnd();
				streamReader.Close();
				body = body
					.Replace("[ImgUrl]", "https://res.cloudinary.com/moshawky/image/upload/v1736589395/schedule_lfv0dk.png")//TODO:Calender image
					.Replace("[Header]", $"Hey {subscriber.LName}")
					.Replace("[Body]", $"Your subscribtion will be expired in {subscriber.subcribtions.Last().EndDate.ToString("dd MMM yyyy")}");
				//ضيفه في الكيو علشان يشتغل مع نفسه في الباك جراوند والتطبيق يوشف اكل عيشه
				BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(subscriber.Email, "Expiration Message", body));

			}

		}
	}
}
