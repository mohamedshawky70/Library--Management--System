using Microsoft.AspNetCore.Authorization;

namespace BookHaven.Web.Controllers
{
	[Authorize(Roles = AppRoles.Achieve)]
	public class CategoriesController : Controller
	{
		private readonly IUnitOfWord _unitOfWord;
		private readonly IMapper _mapper;

		public CategoriesController(IMapper mapper, IUnitOfWord unitOfWord)
		{

			_mapper = mapper;
			_unitOfWord = unitOfWord;
		}

		public IActionResult Index()
		{
			var categories = _unitOfWord.Categories.GetAll(); //No tracking in db because i readOnly.
			var ViewModel = _mapper.Map<IEnumerable<CategoryVM>>(categories);
			return View(ViewModel);
		}
		[HttpGet] //by defualt Get because this action empty parameter
		public IActionResult Create()
		{

			//_Context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('categories', RESEED, 0)"); //love it
			return View();
		}
		[HttpPost]
		[IgnoreAntiforgeryToken]
		public IActionResult Create(CategoryFormVM model)
		{
			if (!ModelState.IsValid) return BadRequest();
			var CategoryIsEsited = _unitOfWord.Categories.FindMatch(c => c.Name == model.Name);
			if (CategoryIsEsited != null)
			{
				TempData["ErrorMessage"] = "Category that you try to add existed!";   //SweetAlert
				return View();
			}
			var category = _mapper.Map<Category>(model);
			_unitOfWord.Categories.Create(category);
			_unitOfWord.Commit();
			TempData["SuccessMessage"] = "Saved successfully";              //SweetAlert
			return RedirectToAction(nameof(Index));
		}
		[HttpGet]
		public IActionResult Edite(int id)
		{

			Category category = _unitOfWord.Categories.GetById(id);
			if (category is null) return NotFound();
			/*CategoryFormVM createCategoryVM = new CategoryFormVM()
			{
				Name = category.Name,
				Id = id                       //ممكن اعمل انبوت مخفي للاي دي في فيو الاندكس ومعملش هنا ال Initialization
			};*/
			var createCategoryVM = _mapper.Map<CategoryFormVM>(category);
			return View(nameof(Create), createCategoryVM);
		}
		[HttpPost]
		[IgnoreAntiforgeryToken]
		public IActionResult Edite(CategoryFormVM model)
		{
			if (!ModelState.IsValid) return BadRequest();
			var CategoryIsEsited = _unitOfWord.Categories.FindMatch(c => c.Name == model.Name);
			if (CategoryIsEsited != null)
			{
				TempData["ErrorMessage"] = "Category that you editing existed!";   //SweetAlert
				return View(nameof(Create), model);
			}
			Category category = _unitOfWord.Categories.FindMatch(x => x.Id == model.Id);
			category.Name = model.Name;
			category.LastUpdatedOn = DateTime.Now;
			_unitOfWord.Commit();
			TempData["SuccessMessage"] = "Edite successfully";   //SweetAlert
			return RedirectToAction(nameof(Index));
		}
		[HttpPost] //[HttpDelete] بتعمل مشاكل لما تدبلوي الموقع وكمان إحنا مش بنحذف بالظبط
		[IgnoreAntiforgeryToken]
		public IActionResult ToggleStatus(int id)
		{
			Category category = _unitOfWord.Categories.GetById(id);
			if (category is null) return NotFound();
			category.IsDeleted = !category.IsDeleted;  //إعكس
			category.LastUpdatedOn = DateTime.Now;
			_unitOfWord.Commit();
			return Ok(category.LastUpdatedOn.ToString());
		}
	}
}
