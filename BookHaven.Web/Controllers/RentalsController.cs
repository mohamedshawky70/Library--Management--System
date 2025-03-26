using Microsoft.AspNetCore.DataProtection;

namespace BookHaven.Web.Controllers
{
	public class RentalsController : Controller
	{
		private readonly IMapper _mapper;
		private readonly IDataProtector _dataProtector;
		private readonly IUnitOfWord _unitOfWord;
		public RentalsController(IUnitOfWord unitOfWord, IDataProtectionProvider dataProtector, IMapper mapper)
		{
			_unitOfWord = unitOfWord;
			_dataProtector = dataProtector.CreateProtector("MySecurKey"); ;
			_mapper = mapper;
		}
		public IActionResult Create(string id)
		{
			var book = _unitOfWord.Books.GetAll().Include(b => b.Author).Include(b => b.CopiesBook);
			var ViewModel = _mapper.Map<IEnumerable<RentalFormVM>>(book);
			foreach (var item in ViewModel)
			{
				item.subsciberId = id;
			}
			return View(ViewModel);
		}
		public IActionResult CreatePost(string title, string subsciberid)
		{
			var subscriperId = int.Parse(_dataProtector.Unprotect(subsciberid));
			var subscriper = _unitOfWord.Subscriper.GetAll()
				.Include(s => s.Rentals)
				.SingleOrDefault(s => s.Id == subscriperId);
			if (subscriper is null) return NotFound();
			//var book = _unitOfWord.Subscriper.FindMatch(b => b.Rentals.Any(r=>r.BookTitle== title));
			var book = subscriper.Rentals.Any(r => r.BookTitle == title);
			if (book)
			{
				TempData["ErrorMessage"] = "This book or Book's Copy has been  rental!";
				return RedirectToAction(nameof(Create), new { id = subsciberid });
			}
			Rental rental = new Rental()
			{
				SubscriperId = subscriperId,
				BookTitle = title
			};
			_unitOfWord.Rental.Create(rental);
			_unitOfWord.Commit();
			TempData["SuccessMessage"] = "Saved successfully";              //SweetAlert
			return RedirectToAction("Details", "Subscripers", new { id = subsciberid });
		}
		public IActionResult Delete(int rentalid, string subscriberid)
		{
			var rental = _unitOfWord.Rental.GetById(rentalid);
			if (rental is null) return NotFound();
			_unitOfWord.Rental.Delete(rental);
			_unitOfWord.Commit();
			TempData["SuccessMessage"] = "Canceled successfully";              //SweetAlert
			return RedirectToAction("Details", "Subscripers", new { id = subscriberid });

		}
		public IActionResult Copies(int bookid, string subsciberid)
		{
			var book = _unitOfWord.Books.GetAll()
				.Include(b => b.CopiesBook)
				.SingleOrDefault(b => b.Id == bookid);
			if (book is null) return NotFound();
			var viewModel = _mapper.Map<RentalCopyBookVM>(book);
			viewModel.subscriberId = subsciberid;
			return View(viewModel);
		}
		public IActionResult ReturnDate(int id, string subsciberid)
		{
			if (!ModelState.IsValid) return BadRequest();
			var rental = _unitOfWord.Rental.GetById(id);
			rental.ReturnDate = DateTime.Now;
			_unitOfWord.Rental.Update(rental);
			_unitOfWord.Commit();
			TempData["SuccessMessage"] = "Saved successfully";
			return RedirectToAction("Details", "Subscripers", new { id = subsciberid });

		}
		public IActionResult PenaltyPaid(int id, string subsciberid)
		{
			if (!ModelState.IsValid) return BadRequest();
			var rental = _unitOfWord.Rental.GetById(id);
			rental.PenaltyPaid = true;
			_unitOfWord.Rental.Update(rental);
			_unitOfWord.Commit();
			TempData["SuccessMessage"] = "Saved successfully";
			return RedirectToAction("Details", "Subscripers", new { id = subsciberid });

		}
	}
}
