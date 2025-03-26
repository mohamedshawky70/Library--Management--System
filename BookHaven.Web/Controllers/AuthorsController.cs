using Microsoft.AspNetCore.Authorization;

namespace BookHaven.Web.Controllers
{
	[Authorize(Roles = AppRoles.Achieve)]
	public class AuthorsController : Controller
	{
		private readonly IUnitOfWord _unitOfWord;
		private readonly IMapper _mapper;

		public AuthorsController(IMapper mapper, IUnitOfWord unitOfWord)
		{
			_mapper = mapper;
			_unitOfWord = unitOfWord;
		}
		public IActionResult Index()
		{
			var Authors = _unitOfWord.Authors.GetAll();
			var ViewModel = _mapper.Map<IEnumerable<AuthorVM>>(Authors);
			return View(ViewModel);
		}
		public IActionResult Create()
		{
			return View();
		}
		[HttpPost]
		[IgnoreAntiforgeryToken]
		public IActionResult Create(AuthorFormVM model)
		{
			if (!ModelState.IsValid) return BadRequest();
			Author OldAuthor = _unitOfWord.Authors.FindMatch(c => c.Name == model.Name);
			if (OldAuthor != null)
			{
				TempData["ErrorMessage"] = "Author that you try to add existed!";   //SweetAlert
				return View();
			}
			var author = _mapper.Map<Author>(model);
			_unitOfWord.Authors.Create(author);
			_unitOfWord.Commit();
			TempData["SuccessMessage"] = "Saved successfully";              //SweetAlert
			return RedirectToAction(nameof(Index));
		}
		[HttpGet]
		public IActionResult Edite(int id)
		{

			Author author = _unitOfWord.Authors.GetById(id);
			if (author is null) return NotFound();
			var createAuthorVM = _mapper.Map<AuthorFormVM>(author);
			return View(nameof(Create), createAuthorVM);
		}
		[HttpPost]
		[IgnoreAntiforgeryToken]
		public IActionResult Edite(AuthorFormVM model)
		{
			if (!ModelState.IsValid) return View(nameof(Create), model);
			Author OldAuthor = _unitOfWord.Authors.FindMatch(c => c.Name == model.Name);
			if (OldAuthor != null && OldAuthor.Id != model.Id)
			{
				TempData["ErrorMessage"] = "Author that you editing existed!";
				return View(nameof(Create), model);
			}
			Author author = _unitOfWord.Authors.GetById(model.Id);
			author.Name = model.Name;
			author.LastUpdatedOn = DateTime.Now;
			_unitOfWord.Authors.Update(author);
			_unitOfWord.Commit();
			TempData["SuccessMessage"] = "Edite successfully";
			return RedirectToAction(nameof(Index));
		}
		[HttpPost]
		[IgnoreAntiforgeryToken]
		public IActionResult ToggleStatus(int id)
		{
			Author author = _unitOfWord.Authors.GetById(id);
			if (author is null) return NotFound();
			author.IsDeleted = !author.IsDeleted;
			author.LastUpdatedOn = DateTime.Now;
			_unitOfWord.Authors.Update(author);
			_unitOfWord.Commit();
			return Ok(author.LastUpdatedOn.ToString());
		}
	}
}
