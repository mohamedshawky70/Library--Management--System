using Microsoft.AspNetCore.DataProtection;

namespace BookHaven.Web.Controllers
{
	public class SearchController : Controller
	{
		private readonly IUnitOfWord _unitOfWord;
		private readonly IDataProtector _dataProtector;
		private readonly IMapper _mapper;
		public SearchController(IUnitOfWord unitOfWord, IMapper mapper, IDataProtectionProvider dataProtector)
		{
			_unitOfWord = unitOfWord;
			_mapper = mapper;
			_dataProtector = dataProtector.CreateProtector("MySecurKey");
		}

		public IActionResult Index()
		{
			return View();
		}
		public IActionResult Details(string key)
		{
			var bookId = int.Parse(_dataProtector.Unprotect(key));
			var book = _unitOfWord.Books.GetAll()
				.Include(b => b.Author)
				.Include(b => b.CopiesBook)
				.Include(b => b.Categories)
				.ThenInclude(b => b.Category)
				.SingleOrDefault(b => b.Id == bookId && !b.IsDeleted);
			if (book is null) return NotFound();
			var bookVm = _mapper.Map<BookVM>(book);
			return View(bookVm);
		}
		public IActionResult Find(string value)
		{
			var books = _unitOfWord.Books.GetAll()
				.Include(b => b.Author)
				.Where(b => !b.IsDeleted && (b.Title.Contains(value) || b.Author!.Name.Contains(value)))
				.Select(b => new { b.Title, key = _dataProtector.Protect(b.Id.ToString()) })
				.ToList();
			if (books is null) return NotFound();
			return Ok(books);
		}
	}
}

