using Microsoft.AspNetCore.DataProtection;
using System.Diagnostics;

namespace BookHaven.Web.Controllers
{
	public class HomeController : Controller
	{
		private readonly IUnitOfWord _unitOfWord;
		private readonly IMapper _mapper;
		private readonly ILogger<HomeController> _logger;
		private readonly IDataProtector _dataProtector;
		public HomeController(ILogger<HomeController> logger, IUnitOfWord unitOfWord, IMapper mapper, IDataProtectionProvider dataProtector)
		{
			_logger = logger;
			_unitOfWord = unitOfWord;
			_mapper = mapper;
			_dataProtector = dataProtector.CreateProtector("MySecurKey"); ;
		}

		public IActionResult Index()
		{
			if (User.Identity.IsAuthenticated)
				return RedirectToAction("Index", "Dashboards");
			var books = _unitOfWord.Books.GetAll().Count(b => !b.IsDeleted);
			var Copies = _unitOfWord.CopiesBook.GetAll().Count(b => !b.IsDeleted);
			var subscribers = _unitOfWord.Subscriper.GetAll().Count();
			var BookAndCopies = (books + Copies) / 10 * 10; //Floor
			var LastAdded = _unitOfWord.Books.GetAll()
				.Include(b => b.Author)
				.Where(b => !b.IsDeleted)
				.OrderByDescending(b => b.Id)
				.Take(9).ToList();
			var rentals = _unitOfWord.Rental.GetAll();
			var dictionary = new Dictionary<string, int>();

			foreach (var rental in rentals)
			{
				if (!dictionary.ContainsKey(rental.BookTitle)) dictionary.Add(rental.BookTitle, 1);
				else dictionary[rental.BookTitle] += 1;
			}
			var TopTitles = dictionary.OrderByDescending(d => d.Value).Take(9).ToList();
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
			foreach (var book in dashboardVM.TopBooks)
			{
				book.Key = _dataProtector.Protect(book.Id.ToString());
			}
			return View(dashboardVM);
		}
		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
