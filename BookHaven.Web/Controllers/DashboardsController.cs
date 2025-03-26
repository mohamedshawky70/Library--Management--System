using Microsoft.AspNetCore.Authorization;
namespace BookHaven.Web.Controllers
{
	[Authorize]
	public class DashboardsController : Controller
	{
		private readonly IUnitOfWord _unitOfWord;
		private readonly IMapper _mapper;
		public DashboardsController(IUnitOfWord unitOfWord, IMapper mapper)
		{
			_unitOfWord = unitOfWord;
			_mapper = mapper;
		}
		public IActionResult Index()
		{
			var books = _unitOfWord.Books.GetAll().Count(b => !b.IsDeleted);
			var Copies = _unitOfWord.CopiesBook.GetAll().Count(b => !b.IsDeleted);
			var subscribers = _unitOfWord.Subscriper.GetAll().Count();
			var BookAndCopies = (books + Copies) / 10 * 10; //Floor
			var LastAdded = _unitOfWord.Books.GetAll()
				.Include(b => b.Author)
				.Where(b => !b.IsDeleted)
				.OrderByDescending(b => b.Id)
				.Take(8).ToList();
			var rentals = _unitOfWord.Rental.GetAll();
			var dictionary = new Dictionary<string, int>();

			foreach (var rental in rentals)
			{
				if (!dictionary.ContainsKey(rental.BookTitle)) dictionary.Add(rental.BookTitle, 1);
				else dictionary[rental.BookTitle] += 1;
			}
			var TopTitles = dictionary.OrderByDescending(d => d.Value).Take(6).ToList();
			List<Book> TopBooks = new List<Book>();
			foreach (var title in TopTitles)
			{
				TopBooks.Add(_unitOfWord.Books.GetAll().Include(b => b.Author)
					.FirstOrDefault(b => b.Title == title.Key));

			}
			DashboardVM dashboardVM = new DashboardVM()
			{
				BookAndCopies = BookAndCopies,
				subscribers = subscribers,
				ListAddedBooks = _mapper.Map<IEnumerable<BookVM>>(LastAdded),
				TopBooks = _mapper.Map<IEnumerable<BookVM>>(TopBooks)
			};
			return View(dashboardVM);
		}
		public IActionResult RentalChart()
		{
			var StartDate = DateTime.Today.AddDays(-29);//Last 30 day
			var EndDate = DateTime.Today;
			var rentals = _unitOfWord.Rental
				.FindAllMatch(r => r.StartDate >= StartDate && r.StartDate <= EndDate)
				.GroupBy(r => new { r.StartDate })
				.Select(g => new ChartVM
				{
					Lable = g.Key.StartDate.ToString("dd MMM yyyy"),
					value = g.Count().ToString()
				}).ToList();
			return Ok(rentals);
		}
		public IActionResult SubscribersChart()
		{
			var Subscriber = _unitOfWord.Subscriper.GetAll().Include(s => s.Governorate)
				.GroupBy(s => new { GovernorateName = s.Governorate.Name })
				.Select(g => new ChartVM
				{
					Lable = g.Key.GovernorateName.ToString(),
					value = g.Count().ToString()
				}).ToList();
			return Ok(Subscriber);
		}
	}
}
