using Microsoft.AspNetCore.Authorization;

namespace BookHaven.Web.Controllers
{
	[Authorize(Roles = AppRoles.Achieve)]
	public class CopiesBookController : Controller
	{
		private readonly IUnitOfWord _unitOfWord;
		private readonly IMapper _mapper;
		public CopiesBookController(IUnitOfWord unitOfWord, IMapper mapper)
		{
			_unitOfWord = unitOfWord;
			_mapper = mapper;
		}

		public IActionResult Create(int id)
		{
			var book = _unitOfWord.Books.GetById(id);
			if (book is null) return NotFound();
			var copyBookFormVm = new CopyBookFormVM()
			{
				bookId = book.Id,
				ShowRentalInput = book.IsAvailableForRental
			};
			return View(copyBookFormVm);

		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Create(CopyBookFormVM copyBookFormVm)
		{
			if (!ModelState.IsValid) return BadRequest();
			var book = _unitOfWord.Books.GetById(copyBookFormVm.bookId);
			if (book is null) return NotFound();
			var copyBook = new CopyBook()
			{
				BookId = copyBookFormVm.bookId,
				EditionNumber = copyBookFormVm.Edition,
				IsAvailableForRental = copyBookFormVm.IsAvailableForRental
			};
			_unitOfWord.CopiesBook.Create(copyBook);
			_unitOfWord.Commit();
			TempData["SuccessMessage"] = "Saved successfully";              //SweetAlert
			return RedirectToAction("Details", "Books", new { id = copyBook.BookId });

		}
		public IActionResult Index()
		{
			return View();
		}
		[HttpPost]
		public IActionResult ToggleStatus(int id)
		{
			var copyBook = _unitOfWord.CopiesBook.GetById(id);
			if (copyBook == null) return NotFound();
			copyBook.IsDeleted = !copyBook.IsDeleted;
			copyBook.LastUpdatedOn = DateTime.Now;
			_unitOfWord.CopiesBook.Update(copyBook);
			_unitOfWord.Commit();
			return Ok();
		}
		[HttpGet]
		public IActionResult Edite(int id)
		{
			if (!ModelState.IsValid) return BadRequest();
			var copyBook = _unitOfWord.CopiesBook.GetAll().Include(c => c.book).SingleOrDefault(c => c.Id == id);
			var book = _unitOfWord.Books.FindMatch(b => b.Id == copyBook.BookId);
			if (copyBook is null) return NotFound();
			var copyBookFormVM = _mapper.Map<CopyBookFormVM>(copyBook);
			copyBookFormVM.ShowRentalInput = book.IsAvailableForRental;

			return View(nameof(Create), copyBookFormVM);
		}
		[HttpPost]
		public IActionResult Edite(CopyBookFormVM copyBookFormVM)
		{
			if (!ModelState.IsValid) return BadRequest();
			var book = _unitOfWord.Books.GetById(copyBookFormVM.bookId);
			if (book is null) return NotFound();
			var Copybook = _mapper.Map<CopyBook>(copyBookFormVM);
			_unitOfWord.CopiesBook.Update(Copybook);
			_unitOfWord.Commit();
			TempData["SuccessMessage"] = "Saved successfully";              //SweetAlert
			return RedirectToAction("Details", "Books", new { id = Copybook.BookId });
		}
	}
}
